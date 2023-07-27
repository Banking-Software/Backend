using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using Microsoft.EntityFrameworkCore;



namespace MicroFinance.Repository.Transaction
{
    public class DepositAccountTransactionRepository : IDepositAccountTransactionRepository
    {
        private readonly ILogger<DepositAccountTransactionRepository> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _transactionDbContext;

        public DepositAccountTransactionRepository
        (
            ILogger<DepositAccountTransactionRepository> logger,
            ApplicationDbContext transactionDbContext,
            IMapper mapper
        )
        {
            _logger = logger;
            _mapper = mapper;
            _transactionDbContext = transactionDbContext;
        }

        private async Task<BaseTransaction> GenerateVoucherNumber(BaseTransaction baseTransaction)
        {
            var existingBaseTransaction = await _transactionDbContext.Transactions.FindAsync(baseTransaction.Id);
            _transactionDbContext.Entry(existingBaseTransaction).State = EntityState.Detached;
            baseTransaction.VoucherNumber = "080/081" + "VCH" + baseTransaction.Id + baseTransaction.BranchCode;
            _transactionDbContext.Transactions.Attach(baseTransaction);
            _transactionDbContext.Entry(baseTransaction).State = EntityState.Modified;
            var voucherStatus = await _transactionDbContext.SaveChangesAsync();
            if (voucherStatus < 1) throw new Exception("Failed to Create Voucher Number");
            return baseTransaction;

        }
        private async Task<BaseTransaction> MakeBaseTransaction(object transactionData)
        {
            BaseTransaction baseTransaction = new BaseTransaction();
            if (transactionData is MakeDepositWrapper depositWrapper)
            {
                baseTransaction = _mapper.Map<BaseTransaction>(depositWrapper);
                baseTransaction.Remarks = $"Deposit Transaction on {depositWrapper.AccountNumber}";
            }
            else if (transactionData is MakeWithDrawalWrapper withDrawalWrapper)
            {
                baseTransaction = _mapper.Map<BaseTransaction>(withDrawalWrapper);
                baseTransaction.Remarks = $"WithDrawal Transaction on {withDrawalWrapper.AccountNumber}";
            }
            else throw new Exception("Invalid Object is Passed");
            await _transactionDbContext.Transactions.AddAsync(baseTransaction);
            var transactionAddStatus = await _transactionDbContext.SaveChangesAsync();
            if (transactionAddStatus < 1) throw new Exception("Failed to create Transaction");
            return await GenerateVoucherNumber(baseTransaction);
        }
        private async Task<DepositAccountTransaction> MakeDepositAccountTransaction(dynamic depositOrWithDrawalTrasactionWrapper, BaseTransaction baseTransaction)
        {
            var depositAccount = await _transactionDbContext.DepositAccounts.FindAsync(depositOrWithDrawalTrasactionWrapper.DepositAccountId);
            DepositAccountTransaction depositAccountTransaction = new DepositAccountTransaction()
            {
                Transaction = baseTransaction,
                DepositAccount = depositAccount,
                TransactionType = depositOrWithDrawalTrasactionWrapper.TransactionType,
                PaymentType = depositOrWithDrawalTrasactionWrapper.PaymentType,
                CollectedByEmployeeId = depositOrWithDrawalTrasactionWrapper.CollectedByEmployeeId,
                Narration = depositOrWithDrawalTrasactionWrapper.Narration,
                Source = depositOrWithDrawalTrasactionWrapper.Source,
                BalanceAfterTransaction = depositAccount.PrincipalAmount
            };
            if (depositOrWithDrawalTrasactionWrapper is MakeWithDrawalWrapper withDrawalWapper)
            {
                depositAccount.PrincipalAmount -= baseTransaction.TransactionAmount;
                depositAccountTransaction.Remarks = $"{baseTransaction.TransactionAmount} is withdrawn on {depositAccount.AccountNumber}";
                depositAccountTransaction.BankChequeNumber = withDrawalWapper.BankChequeNumber;
                depositAccountTransaction.WithDrawalType = withDrawalWapper.WithDrawalType;
            }
            else
            {
                depositAccount.PrincipalAmount += baseTransaction.TransactionAmount;
                depositAccountTransaction.Remarks = $"{baseTransaction.TransactionAmount} is deposited on {depositAccount.AccountNumber}";
            }
            if (depositAccount.PrincipalAmount < 0) throw new Exception("Negative Transaction not allowed");
            if (depositOrWithDrawalTrasactionWrapper.PaymentType == PaymentTypeEnum.Bank)
            {
                var bankDetail = await _transactionDbContext.BankSetups.FindAsync(depositOrWithDrawalTrasactionWrapper.BankDetailId);
                depositAccountTransaction.BankDetail = bankDetail;
                depositAccountTransaction.BankChequeNumber = depositOrWithDrawalTrasactionWrapper.BankChequeNumber;
            }
            await _transactionDbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
            return depositAccountTransaction;
        }

        private async Task MakeSubLedgerTransaction(dynamic depositOrWithDrawalTrasactionWrapper, BaseTransaction baseTransaction)
        {
            int depositSchemeSubLedgerId = depositOrWithDrawalTrasactionWrapper.DepositSchemeSubLedgerId;
            var depositSchemeDepositSubledger = await _transactionDbContext.SubLedgers.Include(sl => sl.Ledger).Where(sl => sl.Id == depositSchemeSubLedgerId).FirstOrDefaultAsync();
            SubLedgerTransaction subLedgerTransaction = new SubLedgerTransaction()
            {
                Transaction = baseTransaction,
                SubLedger = depositSchemeDepositSubledger,
                TransactionType = depositOrWithDrawalTrasactionWrapper.TransactionType,
            };
            if (depositOrWithDrawalTrasactionWrapper is MakeDepositWrapper)
            {
                subLedgerTransaction.Remarks = $"Deposit of {depositOrWithDrawalTrasactionWrapper.TransactionAmount} on {depositOrWithDrawalTrasactionWrapper.AccountNumber}";
                depositSchemeDepositSubledger.CurrentBalance += depositOrWithDrawalTrasactionWrapper.TransactionAmount;
                depositSchemeDepositSubledger.Ledger.CurrentBalance += depositOrWithDrawalTrasactionWrapper.TransactionAmount;
            }
            else
            {
                subLedgerTransaction.Remarks = $"WithDrawal of {depositOrWithDrawalTrasactionWrapper.TransactionAmount} on {depositOrWithDrawalTrasactionWrapper.AccountNumber}";
                depositSchemeDepositSubledger.CurrentBalance -= depositOrWithDrawalTrasactionWrapper.TransactionAmount;
                depositSchemeDepositSubledger.Ledger.CurrentBalance -= depositOrWithDrawalTrasactionWrapper.TransactionAmount;
            }
            if(depositSchemeDepositSubledger.CurrentBalance < 0 || depositSchemeDepositSubledger.Ledger.CurrentBalance<0)
                throw new Exception($"Negative Transaction on '{depositSchemeDepositSubledger.Name}' or '{depositSchemeDepositSubledger.Ledger.Name}' is not allowed");
            subLedgerTransaction.BalanceAfterTransaction = depositSchemeDepositSubledger.CurrentBalance;
            await _transactionDbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        }

        private async Task MakeLedgerTransaction(dynamic depositOrWithDrawalTrasactionWrapper, BaseTransaction baseTransaction)
        {
            Ledger ledger = depositOrWithDrawalTrasactionWrapper.PaymentType == PaymentTypeEnum.Cash
            ?
            await _transactionDbContext.Ledgers.Where(l => l.LedgerCode == 1).SingleOrDefaultAsync()
            :
            await _transactionDbContext.Ledgers.FindAsync(depositOrWithDrawalTrasactionWrapper.BankLedgerId);
            LedgerTransaction ledgerTransaction = new LedgerTransaction()
            {
                Ledger = ledger,
                Transaction = baseTransaction,
            };
            if(depositOrWithDrawalTrasactionWrapper is MakeDepositWrapper)
            {
                ledgerTransaction.Remarks = $"Deposit Transaction of {baseTransaction.TransactionAmount} on {depositOrWithDrawalTrasactionWrapper.AccountNumber}";
                ledger.CurrentBalance += baseTransaction.TransactionAmount;
                ledgerTransaction.TransactionType = TransactionTypeEnum.Debit;
            }
            else
            {
                ledgerTransaction.Remarks = $"WithDrawal Transaction of {baseTransaction.TransactionAmount} on {depositOrWithDrawalTrasactionWrapper.AccountNumber}";
                ledger.CurrentBalance -= baseTransaction.TransactionAmount;
                ledgerTransaction.TransactionType = TransactionTypeEnum.Credit;
            }
            if(ledger.CurrentBalance<0) throw new Exception($"Negative Transaction on {ledger.Name} not allowed");
            ledgerTransaction.BalanceAfterTransaction = ledger.CurrentBalance;
            await _transactionDbContext.LedgerTransactions.AddAsync(ledgerTransaction);
            //var ledgerTransactionStatus = await _transactionDbContext.SaveChangesAsync();
        }
        private async Task<string> MakeTransactionOnDepositAccount(dynamic depositAccountTransactionWrapper)
        {
            using (var processTransaction = await _transactionDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if(!(depositAccountTransactionWrapper is MakeDepositWrapper) && !(depositAccountTransactionWrapper is MakeWithDrawalWrapper))
                        throw new Exception("Invalid Model Received For Transaction");

                    BaseTransaction baseTransaction = await MakeBaseTransaction(depositAccountTransactionWrapper);
                    DepositAccountTransaction depositAccountTransaction = await MakeDepositAccountTransaction(depositAccountTransactionWrapper, baseTransaction);
                    await MakeSubLedgerTransaction(depositAccountTransactionWrapper, baseTransaction);
                    await MakeLedgerTransaction(depositAccountTransactionWrapper, baseTransaction);
                    int transactionStatus = await _transactionDbContext.SaveChangesAsync();
                    if (transactionStatus < 1) throw new Exception("Unable to make Deposit Transaction");
                    await processTransaction.CommitAsync();
                    return baseTransaction.VoucherNumber;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{DateTime.Now}: {ex.Message} {ex?.InnerException?.Message}");
                    await processTransaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }

            }
        }
        public async Task<string> MakeDeposit(MakeDepositWrapper depositWrapper)
        {
            _logger.LogInformation($"{DateTime.Now}: Depositing {depositWrapper.TransactionAmount} by {depositWrapper.CreatedBy} on Deposit Account Id: {depositWrapper.DepositAccountId}");
            return await MakeTransactionOnDepositAccount(depositWrapper);
        }
        public async Task<string> MakeWithDrawal(MakeWithDrawalWrapper withDrawalWrapper)
        {
           _logger.LogInformation($"{DateTime.Now}: WithDrawal {withDrawalWrapper.TransactionAmount} by {withDrawalWrapper.CreatedBy} on Deposit Account Id: {withDrawalWrapper.DepositAccountId}");
            return await MakeTransactionOnDepositAccount(withDrawalWrapper);
        }
    }
}