using System.Data;
using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Enums.Transaction;
using MicroFinance.Exceptions;
using MicroFinance.Helper;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
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
        public async Task<string> MakeTransaction(DepositAccountTransactionWrapper transactionData)
        {
            _transactionDbContext.ChangeTracker.Clear();
            using var processTransaction = _transactionDbContext.Database.BeginTransaction();
            _logger.LogInformation($"{DateTime.Now}: Locking required accounts for the Deposit transaction...");
            int accountId = transactionData.DepositAccountId;
            int depositSchemeSubLedgerId = transactionData.DepositSchemeSubLedgerId;
            int depositSchemeLedgerId = transactionData.DepositSchemeLedgerId;
            int paymentTypeLedgerId = transactionData.PaymentType == PaymentTypeEnum.Cash ? 1 : (int)transactionData.BankLedgerId;
            SemaphoreSlim accountLock = LockManager.Instance.GetAccountLock(accountId);
            await accountLock.WaitAsync();
            SemaphoreSlim depositSchemeSubLedgerLock = LockManager.Instance.GetSubLedgerLock(depositSchemeSubLedgerId);
            await depositSchemeSubLedgerLock.WaitAsync();
            SemaphoreSlim depositSchemeLedgerLock = LockManager.Instance.GetLedgerLock(depositSchemeLedgerId);
            await depositSchemeLedgerLock.WaitAsync();
            SemaphoreSlim paymentTypeLedgerLock = LockManager.Instance.GetLedgerLock(paymentTypeLedgerId);
            await paymentTypeLedgerLock.WaitAsync();
            try
            {
                string voucherNumber = await MakeTransactionOnDepositAccount(transactionData);
                _transactionDbContext.ChangeTracker.Clear();
                await processTransaction.CommitAsync();
                return voucherNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: {ex.Message} {ex?.InnerException?.Message}");
                processTransaction.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                _logger.LogInformation($"{DateTime.Now}: Releasing the locks...");
                accountLock.Release();
                depositSchemeSubLedgerLock.Release();
                depositSchemeLedgerLock.Release();
                paymentTypeLedgerLock.Release();
            }
        }
        private async Task<string> MakeTransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            // using (var processTransaction = _transactionDbContext.Database.BeginTransaction())
            // {
            //     _transactionDbContext.ChangeTracker.Clear();
            //     _logger.LogInformation($"{DateTime.Now}: Locking required accounts for the Deposit transaction...");
            //     int accountId = depositAccountTransactionWrapper.DepositAccountId;
            //     int depositSchemeSubLedgerId = depositAccountTransactionWrapper.DepositSchemeSubLedgerId;
            //     int depositSchemeLedgerId = depositAccountTransactionWrapper.DepositSchemeLedgerId;
            //     int paymentTypeLedgerId = depositAccountTransactionWrapper.PaymentType == PaymentTypeEnum.Cash ? 1 : (int)depositAccountTransactionWrapper.BankLedgerId;
            //     SemaphoreSlim accountLock = LockManager.Instance.GetAccountLock(accountId);
            //     await accountLock.WaitAsync();
            //     SemaphoreSlim depositSchemeSubLedgerLock = LockManager.Instance.GetSubLedgerLock(depositSchemeSubLedgerId);
            //     await depositSchemeSubLedgerLock.WaitAsync();
            //     SemaphoreSlim depositSchemeLedgerLock = LockManager.Instance.GetLedgerLock(depositSchemeLedgerId);
            //     await depositSchemeLedgerLock.WaitAsync();
            //     SemaphoreSlim paymentTypeLedgerLock = LockManager.Instance.GetLedgerLock(paymentTypeLedgerId);
            //     await paymentTypeLedgerLock.WaitAsync();
            //     try
            //     {
                    bool isDeposit = false;
                    TransactionTypeEnum ledgerTransactionType = TransactionTypeEnum.Credit;
                    if (depositAccountTransactionWrapper.TransactionType == TransactionTypeEnum.Credit)
                    {
                        isDeposit = true;
                        ledgerTransactionType = TransactionTypeEnum.Debit;
                    }
                    BaseTransaction baseTransaction = await MakeBaseTransaction(depositAccountTransactionWrapper);
                    await BaseTransactionOnDepositAccount(depositAccountTransactionWrapper, baseTransaction);
                    await BaseTransactionOnLedger(baseTransaction, depositAccountTransactionWrapper.PaymentType, ledgerTransactionType, isDeposit);
                    int transactionStatus = await _transactionDbContext.SaveChangesAsync();
                    if (transactionStatus < 1) throw new Exception("Unable to make Deposit Transaction");
                    // processTransaction.Commit();
                    //_transactionDbContext.ChangeTracker.Clear();
                    return baseTransaction.VoucherNumber;
                // }
                // catch (Exception ex)
                // {
                //     _logger.LogError($"{DateTime.Now}: {ex.Message} {ex?.InnerException?.Message}");
                //     processTransaction.Rollback();
                //     throw new Exception(ex.Message);
                // }
                // finally
                // {
                //     _logger.LogInformation($"{DateTime.Now}: Releasing the locks...");
                //     accountLock.Release();
                //     depositSchemeSubLedgerLock.Release();
                //     depositSchemeLedgerLock.Release();
                //     paymentTypeLedgerLock.Release();
                // }
            //}
        }
      

        public async Task<DepositAccount> BaseTransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction)
        {
            var depositAccount = await TransactionOnDepositAccount(depositAccountTransactionWrapper);
            await CreateDepositAccountTransactionEntry(depositAccountTransactionWrapper, baseTransaction, depositAccount);
            await BaseTransactionOnSubLedger(baseTransaction, depositAccountTransactionWrapper);
            return depositAccount;
        }

        private async Task<DepositAccount> TransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            DepositAccount depositAccount = await _transactionDbContext.DepositAccounts
            .Where(da => da.Id == depositAccountTransactionWrapper.DepositAccountId && da.Status != AccountStatusEnum.Close)
            .SingleOrDefaultAsync();
            if (depositAccount != null)
            {
                depositAccount.PrincipalAmount = depositAccountTransactionWrapper.TransactionType == TransactionTypeEnum.Credit
                ?
                depositAccount.PrincipalAmount + depositAccountTransactionWrapper.TransactionAmount
                :
                depositAccount.PrincipalAmount - depositAccountTransactionWrapper.TransactionAmount;
                if (depositAccount.PrincipalAmount < 0)
                {
                    throw new BadRequestExceptionHandler($"Negative Transaction: current balance of {depositAccount.AccountNumber} will go to negative");
                }
                return depositAccount;
            }
            throw new Exception("No Deposit Account found");
        }
        private async Task CreateDepositAccountTransactionEntry(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction, DepositAccount depositAccount)
        {
            DepositAccountTransaction depositAccountTransaction = new DepositAccountTransaction()
            {
                Transaction = baseTransaction,
                DepositAccount = depositAccount,
                TransactionType = depositAccountTransactionWrapper.TransactionType,
                WithDrawalType = depositAccountTransactionWrapper.WithDrawalType,
                WithDrawalChequeNumber = depositAccountTransactionWrapper.WithDrawalChequeNumber,
                CollectedByEmployeeId = depositAccountTransactionWrapper.CollectedByEmployeeId,
                Narration = depositAccountTransactionWrapper.Narration,
                Source = depositAccountTransactionWrapper.Source,
                BalanceAfterTransaction = depositAccount.PrincipalAmount,
                Remarks = $"Transaction done as {baseTransaction.Remarks}"
            };
            await _transactionDbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
        }

        private async Task BaseTransactionOnSubLedger(BaseTransaction baseTransaction, DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            var depositSchemeSubLedger = await TransactionOnSubLedger(baseTransaction, depositAccountTransactionWrapper);
            await CreateDepositSchemeSubLedgerTransactionEntry(baseTransaction, depositSchemeSubLedger, depositAccountTransactionWrapper);
        }

        private async Task<SubLedger> TransactionOnSubLedger(BaseTransaction baseTransaction, DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            SubLedger depositSchemeSubledger = await _transactionDbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .Where(sl => sl.Id == depositAccountTransactionWrapper.DepositSchemeSubLedgerId)
            .SingleOrDefaultAsync();
            if (depositSchemeSubledger != null)
            {
                if (depositAccountTransactionWrapper.TransactionType == TransactionTypeEnum.Credit)
                {
                    depositSchemeSubledger.CurrentBalance += depositAccountTransactionWrapper.TransactionAmount;
                    depositSchemeSubledger.Ledger.CurrentBalance += depositAccountTransactionWrapper.TransactionAmount;
                }
                else
                {
                    depositSchemeSubledger.CurrentBalance -= depositAccountTransactionWrapper.TransactionAmount;
                    depositSchemeSubledger.Ledger.CurrentBalance -= depositAccountTransactionWrapper.TransactionAmount;
                }
                if (depositSchemeSubledger.CurrentBalance < 0 || depositSchemeSubledger.Ledger.CurrentBalance < 0)
                {
                    throw new Exception("(Negative Balance: Current Transaction will lead SubLedger and Ledger to negative balance)");
                }
                return depositSchemeSubledger;
            }
            throw new Exception("No subLedger found for given deposit account");
        }
        private async Task CreateDepositSchemeSubLedgerTransactionEntry(BaseTransaction baseTransaction, SubLedger depositSchemeSubLedger, DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            SubLedgerTransaction subLedgerTransaction = new()
            {
                Transaction = baseTransaction,
                SubLedger = depositSchemeSubLedger,
                TransactionType = depositAccountTransactionWrapper.TransactionType,
                Remarks = $"Transaction done as {baseTransaction.Remarks}",
                BalanceAfterTransaction = depositSchemeSubLedger.CurrentBalance
            };
            await _transactionDbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        }

        public async Task BaseTransactionOnLedger(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, TransactionTypeEnum ledgerTransactionType, bool isDeposit)
        {
            Ledger paymentMethodLedger = await TransactionOnLedger(baseTransaction, paymentType, ledgerTransactionType, isDeposit);
            await CreateLedgerTransactionEntry(baseTransaction, paymentMethodLedger, ledgerTransactionType);
        }
        private async Task<Ledger> TransactionOnLedger(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, TransactionTypeEnum ledgerTransactionType, bool isDeposit)
        {
            Ledger ledger = paymentType == PaymentTypeEnum.Cash
            ?
            await _transactionDbContext.Ledgers.Where(l => l.LedgerCode == 1).SingleOrDefaultAsync() // In Cash of Cash Transaction
            :
            await _transactionDbContext.Ledgers.Where(l => l.Id == baseTransaction.BankDetail.LedgerId).SingleOrDefaultAsync();

            if (ledger != null)
            {
                ledger.CurrentBalance = isDeposit
                ?
                ledger.CurrentBalance + baseTransaction.TransactionAmount
                :
                ledger.CurrentBalance - baseTransaction.TransactionAmount;
                if (ledger.CurrentBalance < 0)
                {
                    throw new Exception("Negative Balance: Your payment method leads to negative balance in ledger");
                }
                return ledger;
            }
            throw new Exception("No Ledger Found that satisfy your payment method");
        }

        private async Task CreateLedgerTransactionEntry(BaseTransaction baseTransaction, Ledger paymentMethodLedger, TransactionTypeEnum ledgerTransactionType)
        {
            LedgerTransaction ledgerTransaction = new()
            {
                Transaction = baseTransaction,
                Ledger = paymentMethodLedger,
                TransactionType = ledgerTransactionType,
                Remarks = $"Transaction done as {baseTransaction.Remarks}",
                BalanceAfterTransaction = paymentMethodLedger.CurrentBalance
            };
            await _transactionDbContext.LedgerTransactions.AddAsync(ledgerTransaction);
        }

        public async Task<BaseTransaction> BaseTransaction(BaseTransaction baseTransaction, PaymentTypeEnum paymentType, int? bankDetailId, string? bankChequeNumber)
        {
            var transactionDate = baseTransaction.CompanyCalendarCreationDate.Split("/");
            baseTransaction.TransactionYear = Int32.Parse(transactionDate[0]);
            baseTransaction.TransactionMonth = Int32.Parse(transactionDate[1]);
            baseTransaction.TransactionDay = Int32.Parse(transactionDate[0]);
            if (paymentType == PaymentTypeEnum.Bank && bankDetailId!=null)
            {
                var bankDetail = await _transactionDbContext.BankSetups.FindAsync(bankDetailId);
                baseTransaction.BankDetail = bankDetail;
                baseTransaction.BankChequeNumber = bankChequeNumber;
            }
            await _transactionDbContext.Transactions.AddAsync(baseTransaction);
            var status = await _transactionDbContext.SaveChangesAsync();
            if(status<1) throw new Exception("Unable to Make Transaction");
            return await GenerateVoucherNumber(baseTransaction);
        }

        private async Task<BaseTransaction> GenerateVoucherNumber(BaseTransaction baseTransaction)
        {
            var existingBaseTransaction = await _transactionDbContext.Transactions.FindAsync(baseTransaction.Id);
            string fiscalYear = (await _transactionDbContext.CompanyDetails.FirstOrDefaultAsync()).CurrentFiscalYear;
            _transactionDbContext.Entry(existingBaseTransaction).State = EntityState.Detached;
            baseTransaction.VoucherNumber = $"{fiscalYear}VCH{baseTransaction.Id}{baseTransaction.BranchCode}";
            _transactionDbContext.Transactions.Attach(baseTransaction);
            _transactionDbContext.Entry(baseTransaction).State = EntityState.Modified;
            var voucherStatus = await _transactionDbContext.SaveChangesAsync();
            if (voucherStatus < 1) throw new Exception("Failed to Create Voucher Number");
            return baseTransaction;
        }
        private async Task<BaseTransaction> MakeBaseTransaction(DepositAccountTransactionWrapper transactionData)
        {
            BaseTransaction baseTransaction = _mapper.Map<BaseTransaction>(transactionData);
            baseTransaction.Remarks = transactionData.WithDrawalType != null
            ?
            TransactionRemarks.WithdrawalTransaction.ToString()
            :
            TransactionRemarks.DepositTransaction.ToString();
            return await BaseTransaction(baseTransaction, transactionData.PaymentType, transactionData.BankDetailId, transactionData.BankChequeNumber);

            // if (transactionData.PaymentType == PaymentTypeEnum.Bank)
            // {
            //     var bankDetail = await _transactionDbContext.BankSetups.FindAsync(transactionData.BankDetailId);
            //     baseTransaction.BankDetail = bankDetail;
            //     baseTransaction.BankChequeNumber = transactionData.BankChequeNumber;
            // }
            // await _transactionDbContext.Transactions.AddAsync(baseTransaction);
            // var transactionAddStatus = await _transactionDbContext.SaveChangesAsync();
            // if (transactionAddStatus < 1) throw new Exception("Failed to create Transaction");
            // return await GenerateVoucherNumber(baseTransaction);
        }
    }
}