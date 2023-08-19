using System.Data;
using System.Linq.Expressions;
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
        private readonly ICommonExpression _commonExpression;

        public DepositAccountTransactionRepository
        (
            ILogger<DepositAccountTransactionRepository> logger,
            ApplicationDbContext transactionDbContext,
            IMapper mapper,
            ICommonExpression commonExpression
        )
        {
            _logger = logger;
            _mapper = mapper;
            _transactionDbContext = transactionDbContext;
            _commonExpression=commonExpression;
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
                    var depositAccount = await BaseTransactionOnDepositAccount(depositAccountTransactionWrapper, baseTransaction, depositAccountTransactionWrapper.TransactionType);
                    LedgerTransactionWrapper ledgerTransactionWrapper = new()
                    {
                        BaseTransaction = baseTransaction,
                        LedgerTransactionType = ledgerTransactionType,
                        PaymentType=depositAccountTransactionWrapper.PaymentType,
                        IsDeposit = isDeposit,
                        ledgerRemarks=$"{depositAccount.AccountNumber} - {depositAccount.Client.ClientFirstName} {depositAccount.Client.ClientLastName}",
                        LedgerNarration = depositAccountTransactionWrapper.Narration
                    };
                    
                    await BaseTransactionOnLedger(ledgerTransactionWrapper);
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
      

        public async Task<DepositAccount> BaseTransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper, BaseTransaction baseTransaction, TransactionTypeEnum subLedgerTransactionType)
        {
            var depositAccount = await TransactionOnDepositAccount(depositAccountTransactionWrapper);
            await CreateDepositAccountTransactionEntry(depositAccountTransactionWrapper, baseTransaction, depositAccount);
            SubLedgerTransactionWrapper subLedgerTransactionWrapper = new()
            {
                BaseTransaction=baseTransaction,
                LedgerTransactionType = subLedgerTransactionType,
                PaymentType = depositAccountTransactionWrapper.PaymentType,
                IsDeposit = depositAccountTransactionWrapper.TransactionType==TransactionTypeEnum.Credit?true:false,
                ledgerRemarks = $"{depositAccount.AccountNumber} - {depositAccount.Client.ClientFirstName} {depositAccount.Client.ClientLastName}",
                LedgerNarration = depositAccountTransactionWrapper.Narration,
                SubLedgerId = depositAccountTransactionWrapper.DepositSchemeSubLedgerId
            };
            await BaseTransactionOnSubLedger(subLedgerTransactionWrapper);
            return depositAccount;
        }

        private async Task<DepositAccount> TransactionOnDepositAccount(DepositAccountTransactionWrapper depositAccountTransactionWrapper)
        {
            Expression<Func<DepositAccount, bool>> expressionToGetAccountDetail = await _commonExpression.GetExpressionOfDepositAccountForTransaction
            (
                depositAccountId:depositAccountTransactionWrapper.DepositAccountId,
                isDeposit: depositAccountTransactionWrapper.TransactionType == TransactionTypeEnum.Credit?true:false
            );
           
            DepositAccount depositAccount = await _transactionDbContext.DepositAccounts
            .Include(da=>da.Client)
            .Include(da=>da.DepositScheme)
            .Where(expressionToGetAccountDetail).SingleOrDefaultAsync();
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
            throw new Exception("No account found to make a transaction");
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

        private async Task BaseTransactionOnSubLedger(SubLedgerTransactionWrapper subLedgerTransactionWrapper )
        {
            var depositSchemeSubLedger = await TransactionOnSubLedger(subLedgerTransactionWrapper);
            await CreateDepositSchemeSubLedgerTransactionEntry(subLedgerTransactionWrapper, depositSchemeSubLedger);
        }

        private async Task<SubLedger> TransactionOnSubLedger(SubLedgerTransactionWrapper subLedgerTransactionWrapper )
        {
            SubLedger depositSchemeSubledger = await _transactionDbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .Where(sl => sl.Id == subLedgerTransactionWrapper.SubLedgerId)
            .SingleOrDefaultAsync();
            if (depositSchemeSubledger != null)
            {
                if (subLedgerTransactionWrapper.LedgerTransactionType == TransactionTypeEnum.Credit)
                {
                    depositSchemeSubledger.CurrentBalance += subLedgerTransactionWrapper.BaseTransaction.TransactionAmount;
                    depositSchemeSubledger.Ledger.CurrentBalance += subLedgerTransactionWrapper.BaseTransaction.TransactionAmount;
                }
                else
                {
                    depositSchemeSubledger.CurrentBalance -= subLedgerTransactionWrapper.BaseTransaction.TransactionAmount;
                    depositSchemeSubledger.Ledger.CurrentBalance -= subLedgerTransactionWrapper.BaseTransaction.TransactionAmount;
                }
                if (depositSchemeSubledger.CurrentBalance < 0 || depositSchemeSubledger.Ledger.CurrentBalance < 0)
                {
                    throw new Exception("(Negative Balance: Current Transaction will lead SubLedger and Ledger to negative balance)");
                }
                return depositSchemeSubledger;
            }
            throw new Exception("No subLedger found for given deposit account");
        }
        private async Task CreateDepositSchemeSubLedgerTransactionEntry(SubLedgerTransactionWrapper subLedgerTransactionWrapper ,SubLedger depositSchemeSubLedger)
        {
            SubLedgerTransaction subLedgerTransaction = new()
            {
                Transaction = subLedgerTransactionWrapper.BaseTransaction,
                SubLedger = depositSchemeSubLedger,
                TransactionType = subLedgerTransactionWrapper.LedgerTransactionType,
                Remarks = subLedgerTransactionWrapper.ledgerRemarks,
                BalanceAfterTransaction = depositSchemeSubLedger.CurrentBalance,
                Narration=subLedgerTransactionWrapper.LedgerNarration
            };
            await _transactionDbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        }

        public async Task BaseTransactionOnLedger(LedgerTransactionWrapper ledgerTransactionWrapper)
        {
            Ledger paymentMethodLedger = await TransactionOnLedger(ledgerTransactionWrapper);
            await CreateLedgerTransactionEntry(ledgerTransactionWrapper, paymentMethodLedger);
        }
        private async Task<Ledger> TransactionOnLedger(LedgerTransactionWrapper ledgerTransactionWrapper)
        {
            Ledger ledger = ledgerTransactionWrapper.PaymentType == PaymentTypeEnum.Cash
            ?
            await _transactionDbContext.Ledgers.Where(l => l.LedgerCode == 1).SingleOrDefaultAsync() // In Cash of Cash Transaction
            :
            await _transactionDbContext.Ledgers.Where(l => l.Id == ledgerTransactionWrapper.BaseTransaction.BankDetail.LedgerId).SingleOrDefaultAsync();

            if (ledger != null)
            {
                ledger.CurrentBalance = ledgerTransactionWrapper.IsDeposit
                ?
                ledger.CurrentBalance + ledgerTransactionWrapper.BaseTransaction.TransactionAmount
                :
                ledger.CurrentBalance - ledgerTransactionWrapper.BaseTransaction.TransactionAmount;
                if (ledger.CurrentBalance < 0)
                {
                    throw new Exception("Negative Balance: Your payment method leads to negative balance in ledger");
                }
                return ledger;
            }
            throw new Exception("No Ledger Found that satisfy your payment method");
        }

        private async Task CreateLedgerTransactionEntry(LedgerTransactionWrapper ledgerTransactionWrapper, Ledger paymentMethodLedger)
        {
            LedgerTransaction ledgerTransaction = new()
            {
                Transaction = ledgerTransactionWrapper.BaseTransaction,
                Ledger = paymentMethodLedger,
                TransactionType = ledgerTransactionWrapper.LedgerTransactionType,
                Remarks = ledgerTransactionWrapper.ledgerRemarks,
                BalanceAfterTransaction = paymentMethodLedger.CurrentBalance,
                Narration = ledgerTransactionWrapper.LedgerNarration
            };
            await _transactionDbContext.LedgerTransactions.AddAsync(ledgerTransaction);
        }

        public async Task<BaseTransaction> BaseTransaction(BaseTransaction baseTransaction, PaymentTypeEnum? paymentType, int? bankDetailId, string? bankChequeNumber)
        {
            // var transactionDate = baseTransaction.CompanyCalendarCreationDate.Split("/");
            // baseTransaction.TransactionYear = Int32.Parse(transactionDate[0]);
            // baseTransaction.TransactionMonth = Int32.Parse(transactionDate[1]);
            // baseTransaction.TransactionDay = Int32.Parse(transactionDate[0]);
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