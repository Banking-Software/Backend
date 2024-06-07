using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Enums.Transaction.ShareTransaction;
using MicroFinance.Helpers;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Transaction
{
    public class ShareAccountTransactionRepository : IShareAccountTransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ShareAccountTransactionRepository> _logger;
        private readonly IMapper _mapper;
        private readonly ICommonExpression _commonExpression;
        private readonly ITransactions _transactions;

        public ShareAccountTransactionRepository
        (
            ApplicationDbContext dbContext,
            ILogger<ShareAccountTransactionRepository> logger,
            IMapper mapper,
            ICommonExpression commonExpression,
            ITransactions transactions
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _commonExpression = commonExpression;
            _transactions = transactions;
        }

        private async Task<LedgerTransaction> GenerateLedgerTransactionTemplate( Ledger ledger, BaseTransaction baseTransaction, string? narration, TransactionTypeEnum ledgerTransactionType, Client client)
        {
            LedgerTransaction ledgerTransaction = new()
            {
                Transaction = baseTransaction,
                Ledger = ledger,
                TransactionType = ledgerTransactionType,
                Remarks = $"Share Transaction by {client.ClientId} - {client.ClientFirstName} {client.ClientLastName}",
                Narration = narration
            };
            return ledgerTransaction;
        }
        private async Task<DepositAccountTransaction> GetDepositAccountTransactionTemplate(DepositAccount depositAccount, BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, bool isDeposit)
        {
            DepositAccountTransaction depositAccountTransaction = new()
            {
                Transaction = baseTransaction,
                DepositAccount = depositAccount,
                TransactionType = isDeposit ? TransactionTypeEnum.Credit : TransactionTypeEnum.Debit,
                Narration = shareAccountTransactionWrapper.Narration,
                WithDrawalType = !isDeposit ? WithDrawalTypeEnum.ByShare : null,
                Source = "FROM SHARE TRANSACTION",
            };

            return depositAccountTransaction;
        }
        private async Task<SubLedgerTransaction> GenerateSubLedgerTransactionTemplate( SubLedger subLedger, BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, TransactionTypeEnum transactionType)
        {
            SubLedgerTransaction subLedgerTransaction = new()
            {
                Transaction = baseTransaction,
                TransactionType = transactionType,
                Narration = shareAccountTransactionWrapper.Narration,
            };
            return subLedgerTransaction;
        }

        private async Task<ShareTransaction> GenerateShareTransactionTemplate(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, DepositAccount paymentOrTransferDepositAccount)
        {
            ShareTransaction shareTransaction = new()
            {
                ShareAccountId = shareAccountTransactionWrapper.ShareAccountId,
                ShareTransactionType = shareAccountTransactionWrapper.ShareTransactionType,
                ShareCertificateNumber = shareAccountTransactionWrapper.ShareCertificateNumber,
                Narration = shareAccountTransactionWrapper.Narration,
                TransactionType = shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue ? TransactionTypeEnum.Credit : TransactionTypeEnum.Debit,
                Transaction = baseTransaction,
                Remarks = $"Share is {shareAccountTransactionWrapper.ShareTransactionType}",
            };
            if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer)
            {
                shareTransaction.TransferToAccount = paymentOrTransferDepositAccount;
                shareTransaction.Remarks = $"Share is Transfered to {paymentOrTransferDepositAccount.AccountNumber}";
            }
            else if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Account)
            {
                shareTransaction.PaymentDepositAccount = paymentOrTransferDepositAccount;
                shareTransaction.Remarks = $"Share is {shareAccountTransactionWrapper.ShareTransactionType} - {paymentOrTransferDepositAccount.AccountNumber}";
            }
            return shareTransaction;
        }

        private async Task ModifyShareKittaNumber(int shareKittaId, decimal TransactionAmount)
        {
            var shareKitta = await _dbContext.ShareKittas.FindAsync(shareKittaId);
            if (shareKitta == null) throw new Exception("No any available kitta");
            shareKitta.CurrentKitta += TransactionAmount / shareKitta.PriceOfOneKitta;
        }
        public async Task<string> HandleShareTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper)
        {
            TransactionVoucher transactionVoucher = await _transactions.GenerateTransactionVoucher(shareAccountTransactionWrapper.BranchCode, 0);
            var baseTransaction = await GenerateBaseTransaction(shareAccountTransactionWrapper, transactionVoucher);
            DepositAccount depositAccountForTransferOrPayment = null;
            var client = await _dbContext.Clients.Where(c => c.Id == shareAccountTransactionWrapper.ClientId).AsNoTracking().SingleOrDefaultAsync();
            if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer || shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Account)
                depositAccountForTransferOrPayment = await HandleDepositAccount(baseTransaction, shareAccountTransactionWrapper, depositAccountForTransferOrPayment, client);
            else
            {
                int ledgerId;
                bool isLedgerCode = true;
                if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Cash)
                    ledgerId = 1;
                else if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Bank)
                {
                    ledgerId = baseTransaction.BankDetail.LedgerId;
                    isLedgerCode = false;
                }
                else
                    throw new Exception("Payment Type not accepted");
                TransactionTypeEnum ledgerTransactionType = TransactionTypeEnum.Credit;
                bool isDeposit = false;
                if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue)
                {
                    isDeposit = true;
                    ledgerTransactionType = TransactionTypeEnum.Debit;
                }
                await UpdateLedger(baseTransaction, shareAccountTransactionWrapper.Narration, client, isDeposit, isLedgerCode, ledgerId, ledgerTransactionType);

            }
            await HandleShareAccountTransaction(baseTransaction, shareAccountTransactionWrapper, depositAccountForTransferOrPayment, client);
            if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue)
                await ModifyShareKittaNumber(shareAccountTransactionWrapper.ShareKittaId, shareAccountTransactionWrapper.TransactionAmount);
            int numberOfRowsAffected = await _dbContext.SaveChangesAsync();
            if (numberOfRowsAffected <= 0) throw new Exception("Failed to Perform the Transaction");
            return transactionVoucher.VoucherNumber;
        }

        private async Task<DepositAccount> HandleDepositAccount(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, DepositAccount depositAccount, Client client)
        {
            bool isDeposit = shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Refund || shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer;
            int subLedgerId;
            int depositAccountId;
            if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Transfer)
            {
                depositAccountId = (int)shareAccountTransactionWrapper.TransferToDepositAccountId;
                subLedgerId = (int)shareAccountTransactionWrapper.TransferToDepositSchemeSubLedgerId;
            }
            else
            {
                depositAccountId = (int)shareAccountTransactionWrapper.PaymentDepositAccountId;
                subLedgerId = (int)shareAccountTransactionWrapper.PaymentDepositSchemeSubLedgerId;
            }
            depositAccount = await UpdateDepositAccount(baseTransaction, shareAccountTransactionWrapper, depositAccountId, isDeposit, depositAccount);
            await UpdateSubLedger(baseTransaction, shareAccountTransactionWrapper, subLedgerId, isDeposit, client);
            return depositAccount;
        }

        private async Task<DepositAccount> UpdateDepositAccount(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, int depositAccountId, bool isDeposit, DepositAccount depositAccount)
        {
            Expression<Func<DepositAccount, bool>> expressionToGetAccountDetail = await _commonExpression.GetExpressionOfDepositAccountForTransaction
            (
                depositAccountId: depositAccountId,
                isDeposit: isDeposit
            );
            depositAccount = await _dbContext.DepositAccounts
            .Include(da => da.Client)
            .Include(da => da.DepositScheme)
            .Where(expressionToGetAccountDetail).SingleOrDefaultAsync();
            if(depositAccount==null) throw new Exception("No Deposit Account Found");
            DepositAccountTransaction depositAccountTransaction = await GetDepositAccountTransactionTemplate(depositAccount, baseTransaction, shareAccountTransactionWrapper, isDeposit);
            await _transactions.TransactionOnDepositAccount(depositAccountTransaction, isDeposit);
            return depositAccount;
        }

        private async Task UpdateSubLedger(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, int subLedgerId, bool isDeposit, Client client)
        {
            SubLedger subLedger = await _dbContext.SubLedgers.FindAsync(subLedgerId);
            if (subLedger == null) throw new Exception("SubLedger found for the account");
            TransactionTypeEnum transactionType = isDeposit ? TransactionTypeEnum.Credit : TransactionTypeEnum.Debit;
            SubLedgerTransaction subLedgerTransaction = await GenerateSubLedgerTransactionTemplate(subLedger, baseTransaction, shareAccountTransactionWrapper, transactionType);
            await _transactions.TransactionOnSubLedger(subLedgerTransaction, isDeposit);
            await UpdateLedger(baseTransaction, shareAccountTransactionWrapper.Narration, client, isDeposit, false, subLedger.LedgerId, transactionType);
        }

        private async Task UpdateLedger(BaseTransaction baseTransaction, string? narration, Client client, bool isDeposit, bool isLedgerCode, int ledgerId, TransactionTypeEnum transactionType)
        {
            Ledger ledger = isLedgerCode ? await _dbContext.Ledgers.Where(l=>l.LedgerCode==ledgerId).SingleOrDefaultAsync():
            await _dbContext.Ledgers.FindAsync(ledgerId);
            if(ledger==null) throw new Exception("No Ledger Found");
            LedgerTransaction ledgerTransaction = await GenerateLedgerTransactionTemplate(ledger, baseTransaction, narration, transactionType, client);
            await _transactions.TransactionOnLedger(ledgerTransaction, isDeposit);
        }

        private async Task HandleShareAccountTransaction(BaseTransaction baseTransaction, ShareAccountTransactionWrapper shareAccountTransactionWrapper, DepositAccount paymentOrTransferAccount, Client client)
        {
            bool isDeposit = shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue ? true : false;
            Expression<Func<ShareAccount, bool>> expression = await _commonExpression.GetExpressionOfShareAccountForTransaction(shareAccountTransactionWrapper.ShareAccountId, null);
            ShareAccount transactionShareAccount = await _dbContext.ShareAccounts.Include(sa => sa.Client).Where(expression).SingleOrDefaultAsync();
            await _transactions.TransactionOnShareAccount(transactionShareAccount, baseTransaction.TransactionAmount, isDeposit);
            ShareTransaction shareTransaction = await GenerateShareTransactionTemplate(baseTransaction, shareAccountTransactionWrapper, paymentOrTransferAccount);
            shareTransaction.ShareAccount = transactionShareAccount;
            shareTransaction.BalanceAfterTransaction = transactionShareAccount.CurrentShareBalance;
            await _transactions.TransactionEntryForShareAccount(shareTransaction);
            string narration = $"Share Transaction as {shareAccountTransactionWrapper.ShareTransactionType} on Client Number {client.ClientId}";
            await UpdateLedger(baseTransaction, narration, client, isDeposit, true, (int) client.ClientShareTypeInfoId, shareTransaction.TransactionType);
        }
        private async Task<BaseTransaction> GenerateBaseTransaction(ShareAccountTransactionWrapper shareAccountTransactionWrapper, TransactionVoucher transactionVoucher)
        {
            BaseTransaction baseTransaction = new BaseTransaction();
            baseTransaction = _mapper.Map<BaseTransaction>(shareAccountTransactionWrapper);
            baseTransaction.TransactionVoucher = transactionVoucher;
            if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Issue)
                baseTransaction.Remarks = TransactionRemarks.ShareIssueTransaction.ToString();
            else if (shareAccountTransactionWrapper.ShareTransactionType == ShareTransactionTypeEnum.Refund)
                baseTransaction.Remarks = TransactionRemarks.ShareRefundTransaction.ToString();
            else
                baseTransaction.Remarks = TransactionRemarks.ShareTransferTransaction.ToString();
            if (shareAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Bank)
            {
                var bankDetail = await _dbContext.BankSetups.FindAsync(shareAccountTransactionWrapper.BankDetailId);
                baseTransaction.BankDetail = bankDetail;
                baseTransaction.BankChequeNumber = shareAccountTransactionWrapper.BankChequeNumber;
            }
            await _transactions.GenerateBaseTransaction(baseTransaction);
            return baseTransaction;
        }
    }
}