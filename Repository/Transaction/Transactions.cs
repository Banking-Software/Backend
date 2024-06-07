using Humanizer;
using MicroFinance.DBContext;
using MicroFinance.Exceptions;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Transaction
{
    public class Transactions : ITransactions
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<Transactions> _logger;

        public Transactions(ApplicationDbContext dbContext, ILogger<Transactions> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

        }
        // BASE TRANSACTION: START
        private string GetAmountInWords(decimal transactionAmount)
        {
            int firstHalfInterestAmonut = (int)transactionAmount;
            string unFormatedSecondHalfInterestAmount = ((int)(transactionAmount - firstHalfInterestAmonut) * 100).ToString();
            unFormatedSecondHalfInterestAmount = unFormatedSecondHalfInterestAmount.TrimEnd(new char[] { '0' });
            int secondHalfInterestAmount = 0;
            _ = int.TryParse(unFormatedSecondHalfInterestAmount, out secondHalfInterestAmount);
            return $"{firstHalfInterestAmonut.ToWords()} point {secondHalfInterestAmount.ToWords()}";
        }
        /// <summary>
        /// Generate the Voucher Number according to the Branch code
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="voucherIndex"></param>
        /// <returns></returns>
        public async Task<TransactionVoucher> GenerateTransactionVoucher(string branchCode, int voucherIndex)
        {
            if (voucherIndex <= 0)
            {
                var lastRecord = await _dbContext.TransactionVouchers.OrderBy(tv => tv.Id).LastOrDefaultAsync();
                voucherIndex = lastRecord==null?1:lastRecord.Id + 1;
            }
            else
                voucherIndex++;
            if (await _dbContext.TransactionVouchers.FindAsync(voucherIndex) != null)
                throw new Exception("Voucher Number Exist.");
            var financialYear = await _dbContext.CompanyDetails.FirstOrDefaultAsync();
            if (financialYear == null)
                throw new NotImplementedException("No Records found for financial year");
            string voucherNumber = $"{financialYear.CurrentFiscalYear}VCH{voucherIndex}{branchCode}";
            TransactionVoucher transactionVoucher = new TransactionVoucher() { VoucherNumber = voucherNumber };
            await _dbContext.AddAsync(transactionVoucher);
            return transactionVoucher;
        }
        /// <summary>
        /// Base Transaction of Every Transaction. Every Transaction has the foreign key reference of this Base Transaction.
        /// </summary>
        /// <param name="baseTransaction"></param>
        /// <returns></returns>
        public async Task GenerateBaseTransaction(BaseTransaction baseTransaction)
        {
            if (string.IsNullOrEmpty(baseTransaction.AmountInWords))
                baseTransaction.AmountInWords = GetAmountInWords(baseTransaction.TransactionAmount);
            await _dbContext.AddAsync(baseTransaction);
        }
        // BASE TRANSACTION: END
        public async Task TransactionOnDepositAccount(DepositAccountTransaction depositAccountTransaction, bool isDeposit)
        {
            DepositAccount depositAccount = depositAccountTransaction.DepositAccount;
            if(!isDeposit && !depositAccount.IsWithDrawalAllowed)
                throw new Exception("WithDrawal is blocked for this account");
            decimal transactionAmount = depositAccountTransaction.Transaction.TransactionAmount;
            depositAccount.PrincipalAmount = isDeposit ? depositAccount.PrincipalAmount + transactionAmount : depositAccount.PrincipalAmount - transactionAmount;
            if (depositAccount.PrincipalAmount < 0)
                throw new NegativeBalanceExceptionHandler($"Transaction on {depositAccount.AccountNumber} led to negative balance. Whole Transaction is aborted");
            depositAccountTransaction.BalanceAfterTransaction = depositAccount.PrincipalAmount;
            await _dbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
        }
        public async Task TransactionOnShareAccount(ShareAccount shareAccount, decimal transactionAmount, bool isDeposit)
        {
            shareAccount.CurrentShareBalance = isDeposit ? shareAccount.CurrentShareBalance + transactionAmount
            : shareAccount.CurrentShareBalance - transactionAmount;
            if(shareAccount.CurrentShareBalance<=0)
                throw new NegativeBalanceExceptionHandler("Transaction led to share balance negative. Transaction Aborted...");
        }
        /// <summary>
        /// Update Balance of Ledger
        /// </summary>
        /// <param name="ledger"></param>
        /// <param name="transactionAmount"></param>
        /// <param name="isDeposit"></param>
        /// <returns></returns>

        public async Task TransactionOnLedger(LedgerTransaction ledgerTransaction, bool isDeposit)
        {
            Ledger ledger = ledgerTransaction.Ledger;
            decimal transactionAmount = ledgerTransaction.Transaction.TransactionAmount;
            ledger.CurrentBalance = isDeposit ? ledger.CurrentBalance + transactionAmount : ledger.CurrentBalance - transactionAmount;
            if (ledger.CurrentBalance < 0)
                throw new NegativeBalanceExceptionHandler($"Transaction led to negative balance of ledger {ledger.Name}. Whole Transaction is rejected.");
            ledgerTransaction.BalanceAfterTransaction = ledger.CurrentBalance;
            await _dbContext.LedgerTransactions.AddAsync(ledgerTransaction);
        }
        /// <summary>
        /// Update Balance Of SubLedger
        /// </summary>
        /// <param name="subLedger"></param>
        /// <param name="transactionAmount"></param>
        /// <param name="isDeposit"></param>
        /// <returns></returns>
        public async Task TransactionOnSubLedger(SubLedgerTransaction subLedgerTransaction, bool isDeposit)
        {
            SubLedger subLedger = subLedgerTransaction.SubLedger;
            decimal transactionAmount = subLedgerTransaction.Transaction.TransactionAmount;
            subLedger.CurrentBalance = isDeposit ? subLedger.CurrentBalance + transactionAmount : subLedger.CurrentBalance - transactionAmount;
            if (subLedger.CurrentBalance < 0)
                throw new NegativeBalanceExceptionHandler($"Transaction led to negative balance of subledger {subLedger.Name}. Whole Transaction is rejected.");
            subLedgerTransaction.BalanceAfterTransaction = subLedger.CurrentBalance;
            await _dbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        }
        public async Task TransactionEntryForDepositAccount(DepositAccountTransaction depositAccountTransaction)
        {
            await _dbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
        }
        public async Task TransactionEntryForShareAccount(ShareTransaction shareTransaction)
        {
            await _dbContext.ShareTransactions.AddAsync(shareTransaction);
        }
        /// <summary>
        /// Add Transactional Entry Record for the ledger. 
        /// It stores the information about the Transactional Type, BalanceAfterTransaction, Remarks, Narration, e.t.c
        /// See Model LedgerTransaction for other detail
        /// </summary>
        /// <param name="ledgerTransaction"></param>
        /// <returns></returns>
        public async Task TransactionEntryForLedger(LedgerTransaction ledgerTransaction)
        {
            await _dbContext.LedgerTransactions.AddAsync(ledgerTransaction);
        }
        /// <summary>
        /// Add Transactional Entry Record for the subledger. 
        /// It stores the information about the Transactional Type, BalanceAfterTransaction, Remarks, Narration, e.t.c
        /// See Model SubLedgerTransaction for other detail
        /// </summary>
        /// <param name="ledgerTransaction"></param>
        /// <returns></returns>
        public async Task TransactionEntryForSubLedger(SubLedgerTransaction subLedgerTransaction)
        {
            await _dbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        }

        public async Task TransactionEntriesForLedger(List<LedgerTransaction> ledgerTransactions)
        {
            await _dbContext.LedgerTransactions.AddRangeAsync(ledgerTransactions);
        }
    }
}