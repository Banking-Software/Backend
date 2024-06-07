using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Helpers;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using Microsoft.EntityFrameworkCore;




namespace MicroFinance.Repository.Transaction
{
    public class DepositAccountTransactionRepository : IDepositAccountTransactionRepository
    {
        private readonly ILogger<DepositAccountTransactionRepository> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly ICommonExpression _commonExpression;
        private readonly ITransactions _transactions;
        private Dictionary<int, Ledger> allLedger = new();
        private Dictionary<int, SubLedger> allSubLedger = new();

        public DepositAccountTransactionRepository
        (
            ILogger<DepositAccountTransactionRepository> logger,
            ApplicationDbContext dbContext,
            IMapper mapper,
            ICommonExpression commonExpression,
            ITransactions transactions
        )
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
            _commonExpression = commonExpression;
            _transactions=transactions;
        }
        private async Task<DepositAccountTransaction> GenerateDepositAccountTransactionTemplate(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction, DepositAccount depositAccount)
        {
            DepositAccountTransaction depositAccountTransaction = new DepositAccountTransaction()
            {
                Transaction = baseTransaction,
                TransactionType = depositAccountTransactionWrapper.TransactionType,
                WithDrawalType = depositAccountTransactionWrapper.WithDrawalType,
                WithDrawalChequeNumber = depositAccountTransactionWrapper.WithDrawalChequeNumber,
                CollectedByEmployeeId = depositAccountTransactionWrapper.CollectedByEmployeeId,
                Narration = depositAccountTransactionWrapper.Narration,
                Source = depositAccountTransactionWrapper.Source,
                Remarks = $"Transaction done as {baseTransaction.Remarks}",
                DepositAccount = depositAccount
            };
            return depositAccountTransaction;
        }
        private async Task<SubLedgerTransaction> GenerateSubLedgerTransactionTemplate(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction, SubLedger subLedger)
        {
            SubLedgerTransaction subLedgerTransaction = new()
            {
                Transaction = baseTransaction,
                TransactionType = depositAccountTransactionWrapper.TransactionType,
                Narration = depositAccountTransactionWrapper.Narration,
                SubLedger = subLedger
            };
            return subLedgerTransaction;
        }
       
        private async Task<LedgerTransaction> GenerateLedgerTransactionTemplate(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction, TransactionTypeEnum transactionType, Client client, Ledger ledger)
        {
            LedgerTransaction ledgerTransaction = new LedgerTransaction()
            {
                Transaction = baseTransaction,
                Ledger = ledger,
                TransactionType = transactionType,
                Narration = depositAccountTransactionWrapper.Narration,
                Remarks = $"{client.ClientFirstName} {client.ClientLastName} -- {depositAccountTransactionWrapper.AccountNumber}"
            };
            return ledgerTransaction;
            
        }

        public async Task<string> HandleDepositAccountTransaction(DepositAccountTransactionWrapper transactionData)
        {
            TransactionVoucher transactionVoucher = await _transactions.GenerateTransactionVoucher(transactionData.BranchCode, 0);
            BaseTransaction baseTransaction = await GenerateBaseTransaction(transactionVoucher, transactionData);
            bool isDeposit = transactionData.TransactionType == TransactionTypeEnum.Credit?true:false;
            DepositAccount depositAccount = await UpdateDepositAccount(transactionData, baseTransaction, isDeposit);

            transactionData.AccountNumber = depositAccount.AccountNumber;
            var client = await _dbContext.Clients.Where(c=>c.Id==depositAccount.ClientId).AsNoTracking().SingleOrDefaultAsync();
            await UpdateSubLedger(transactionData, baseTransaction, isDeposit, client);
            await UpdateLedger(transactionData, baseTransaction, isDeposit, client);
            int numberOfRowsAffected = await _dbContext.SaveChangesAsync();
            allLedger.Clear();
            allSubLedger.Clear();
            if(numberOfRowsAffected<=0) throw new Exception("Failed to perform the transaction.");

            return transactionVoucher.VoucherNumber;
        }

        private async Task<DepositAccount> UpdateDepositAccount(DepositAccountTransactionWrapper transactionData, BaseTransaction baseTransaction, bool isDeposit)
        {
            Expression<Func<DepositAccount, bool>> expressionToGetAccountDetail = await _commonExpression.GetExpressionOfDepositAccountForTransaction
            (
                depositAccountId: (int) transactionData.DepositAccountId,
                isDeposit: isDeposit
            );
            DepositAccount depositAccount = await _dbContext.DepositAccounts
            .Include(da => da.Client)
            .Include(da => da.DepositScheme)
            .Where(expressionToGetAccountDetail).SingleOrDefaultAsync();
            if(depositAccount==null) throw new KeyNotFoundException($"No such deposit account exist");
            
            DepositAccountTransaction depositAccountTransaction = await GenerateDepositAccountTransactionTemplate(transactionData, baseTransaction, depositAccount);
            await _transactions.TransactionOnDepositAccount(depositAccountTransaction, isDeposit);
            return depositAccount;
        }

        private async Task UpdateSubLedger(DepositAccountTransactionWrapper transactionData, BaseTransaction baseTransaction, bool isDeposit, Client client)
        {
            await UpdateSubLedgerRecord(transactionData.DepositSchemeSubLedgerId);
            SubLedger subLedger = allSubLedger[transactionData.DepositSchemeSubLedgerId];
            await UpdateLedgerRecord(subLedger.LedgerId, false);
            Ledger ledger = allLedger[subLedger.LedgerId];
            SubLedgerTransaction subLedgerTransaction = await GenerateSubLedgerTransactionTemplate(transactionData, baseTransaction, subLedger);
            LedgerTransaction ledgerTransactionDuringSubledger = await GenerateLedgerTransactionTemplate(transactionData, baseTransaction, subLedgerTransaction.TransactionType, client, ledger);
            await _transactions.TransactionOnSubLedger(subLedgerTransaction, isDeposit);
            await _transactions.TransactionOnLedger(ledgerTransactionDuringSubledger, isDeposit);
        }

        private async Task UpdateLedger(DepositAccountTransactionWrapper transactionData, BaseTransaction baseTransaction, bool isDeposit, Client client)
        {
            bool isLedgerCode = transactionData.PaymentType==PaymentTypeEnum.Cash?true:false;
            int ledgerId = transactionData.PaymentType==PaymentTypeEnum.Cash?1:baseTransaction.BankDetail.LedgerId;
            TransactionTypeEnum transactionType = transactionData.TransactionType==TransactionTypeEnum.Credit?TransactionTypeEnum.Debit:TransactionTypeEnum.Credit;
            await UpdateLedgerRecord(ledgerId, isLedgerCode);
            Ledger ledger = allLedger[ledgerId];
            LedgerTransaction ledgerTransaction = await GenerateLedgerTransactionTemplate(transactionData, baseTransaction, transactionType, client, ledger);
            await _transactions.TransactionOnLedger(ledgerTransaction, isDeposit);
        }

        private async Task UpdateLedgerRecord(int ledgerId, bool isLedgerCode)
        {
            if(!allLedger.ContainsKey(ledgerId))
            {
                Ledger ledger = isLedgerCode? await _dbContext.Ledgers.Where(x=>x.LedgerCode==ledgerId).SingleOrDefaultAsync():
                await _dbContext.Ledgers.FindAsync(ledgerId);
                if(ledger==null) throw new Exception("No Ledger Found");
                allLedger.Add(ledgerId, ledger);
            }
        }

        private async Task UpdateSubLedgerRecord(int subLedgerId)
        {
            if(!allSubLedger.ContainsKey(subLedgerId))
            {
                SubLedger subLedger = await _dbContext.SubLedgers.FindAsync(subLedgerId);
                if(subLedger==null) throw new Exception("No SubLedger Found");
                allSubLedger.Add(subLedgerId, subLedger);
            }
        }


        private async Task<BaseTransaction> GenerateBaseTransaction(TransactionVoucher transactionVoucher, DepositAccountTransactionWrapper transactionData)
        {
            BaseTransaction baseTransaction = _mapper.Map<BaseTransaction>(transactionData);
            baseTransaction.TransactionVoucher = transactionVoucher;
            baseTransaction.Remarks = transactionData.WithDrawalType != null
            ?
            TransactionRemarks.WithdrawalTransaction.ToString()
            :
            TransactionRemarks.DepositTransaction.ToString();

            if (transactionData.PaymentType == PaymentTypeEnum.Bank && transactionData.BankDetailId != null)
            {
                var bankDetail = await _dbContext.BankSetups.FindAsync(transactionData.BankDetailId);
                baseTransaction.BankDetail = bankDetail;
                baseTransaction.BankChequeNumber = transactionData.BankChequeNumber;
            }
            await _transactions.GenerateBaseTransaction(baseTransaction);
            return baseTransaction;
        }
    }
}