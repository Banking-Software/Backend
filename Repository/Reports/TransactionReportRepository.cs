using System.Linq.Expressions;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Reports
{
    public class TransactionReportrepository : ITransactionReportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionReportrepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        // private static Expression<Func<BaseTransaction, bool>> GetDateFilterPredicate(DateWrapper dateFrom, DateWrapper dateTo)
        // {
        //     return trasc =>
        //     (trasc.TransactionYear == dateFrom.Year && trasc.TransactionMonth == dateFrom.Month && trasc.TransactionDay >= dateFrom.Day) ||
        //     (trasc.TransactionYear == dateTo.Year && trasc.TransactionMonth == dateTo.Month && trasc.TransactionDay <= dateTo.Day) ||
        //     (trasc.TransactionYear == dateFrom.Year && trasc.TransactionMonth > dateFrom.Month && trasc.TransactionMonth < dateTo.Month) ||
        //     (trasc.TransactionYear > dateFrom.Year && trasc.TransactionYear < dateTo.Year) ||
        //     (trasc.TransactionYear == dateTo.Year && trasc.TransactionMonth < dateTo.Month);
        // }

        public async Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReport(Expression<Func<DepositAccountTransaction, bool>> expressionOnDepositAccountTransaction)
        {
            DepositAccountTransactionReportWrapper depositAccountTransactionReportWrapper = new();
            // Get the report based on provided date
            List<DepositAccountTransactionReport> depositAccountTransactionReport =
            await _dbContext.DepositAccountTransactions.Include(dat => dat.Transaction).Include(dat => dat.DepositAccount)
            .Where(expressionOnDepositAccountTransaction)
            .OrderBy(dat=>dat.Transaction.RealWorldCreationDate)
            .Select
            (dat=>
                new DepositAccountTransactionReport
                {
                    BaseTransactionId = dat.Transaction.Id,
                    DepositAccountTransactionId = dat.Id,
                    VoucherNumber = dat.Transaction.VoucherNumber,
                    TransactionRemarks = dat.Transaction.Remarks,
                    TransactionType = dat.TransactionType,
                    TransactionAmount = dat.Transaction.TransactionAmount,
                    Narration = dat.Narration,
                    Description = dat.Remarks,
                    RealWorldCreationDate = dat.Transaction.RealWorldCreationDate,
                    NepaliCreationDate = dat.Transaction.NepaliCreationDate,
                    EnglishCreationDate = dat.Transaction.EnglishCreationDate,
                    BalanceAfterTransaction = dat.BalanceAfterTransaction,
                    TransactionDoneBy = dat.Transaction.CreatedBy,
                    Status=dat.DepositAccount.Status
                }
            ).AsNoTracking().ToListAsync();

            if (depositAccountTransactionReport != null && depositAccountTransactionReport.Any())
            {
                // Find the total amount in debit and credit side
                var transactionSum = depositAccountTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });
                // Get the BalanceAfterTransaction exact before our first entry of report: OpeningBalance
                var firstReport = depositAccountTransactionReport[0];
                var previousAccount = await _dbContext.DepositAccountTransactions
                .Include(dat => dat.Transaction)
                .Where(dat => dat.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(dat => dat.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                depositAccountTransactionReportWrapper.DepositAccountTransactionReports = depositAccountTransactionReport;
                depositAccountTransactionReportWrapper.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                depositAccountTransactionReportWrapper.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                depositAccountTransactionReportWrapper.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;

            }

            return depositAccountTransactionReportWrapper;
        }

        public async Task<LedgerTransactionReportWrapper> GetLedgerTransactionReport(Expression<Func<LedgerTransaction, bool>> expressionOnLedgerTransaction)
        {
            LedgerTransactionReportWrapper ledgerTransactionReportWrapper = new();
            var ledgerTransactionReport =  await _dbContext.LedgerTransactions
            .Include(lt=>lt.Transaction)
            .Include(lt=>lt.Ledger)
            .Where(expressionOnLedgerTransaction)
            .OrderBy(lt=>lt.Transaction.RealWorldCreationDate)
            .Select
            (lt=>
                new LedgerTransactionReport
                {
                    BaseTransactionId = lt.Transaction.Id,
                    LedgerTransactionId = lt.Id,
                    VoucherNumber = lt.Transaction.VoucherNumber,
                    TransactionRemarks = lt.Transaction.Remarks,
                    TransactionType = lt.TransactionType,
                    TransactionAmount = lt.Transaction.TransactionAmount,
                    Narration = lt.Narration,
                    Description = lt.Remarks,
                    RealWorldCreationDate = lt.Transaction.RealWorldCreationDate,
                    NepaliCreationDate = lt.Transaction.NepaliCreationDate,
                    EnglishCreationDate = lt.Transaction.EnglishCreationDate,
                    BalanceAfterTransaction = lt.BalanceAfterTransaction,
                    TransactionDoneBy = lt.Transaction.CreatedBy
                }
            ).AsNoTracking().ToListAsync();

             if (ledgerTransactionReport != null && ledgerTransactionReport.Any())
            {
                var transactionSum = ledgerTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = ledgerTransactionReport[0];

                var previousAccount = await _dbContext.LedgerTransactions
                .Include(st => st.Transaction)
                .Where(st => st.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(st => st.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                ledgerTransactionReportWrapper.LedgerTransactionReports = ledgerTransactionReport;
                ledgerTransactionReportWrapper.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                ledgerTransactionReportWrapper.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                ledgerTransactionReportWrapper.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;
            }
            return ledgerTransactionReportWrapper;
        }

        public async Task<ShareTransactionReportWrapper> GetShareTransactionReport(Expression<Func<ShareTransaction, bool>> expressionOnShareTransaction)
        {
            // Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            ShareTransactionReportWrapper shareTransactionReportWrappr = new();
            var shareTransactionReport = await _dbContext.ShareTransactions.Include(st=>st.Transaction).Include(st=>st.ShareAccount)
            .Where(expressionOnShareTransaction).OrderBy(st=>st.Transaction.RealWorldCreationDate)
            .Select(st=>new ShareTransactionReport
            {
                BaseTransactionId=st.TransactionId,
                ShareTransactionId = st.Id,
                VoucherNumber=st.Transaction.VoucherNumber,
                TransactionRemarks = st.Transaction.Remarks,
                TransactionType = st.TransactionType,
                TransactionAmount = st.Transaction.TransactionAmount,
                BalanceAfterTransaction = st.BalanceAfterTransaction,
                EnglishCreationDate = st.Transaction.EnglishCreationDate,
                NepaliCreationDate = st.Transaction.NepaliCreationDate,
                RealWorldCreationDate = st.Transaction.RealWorldCreationDate,
                Narration = st.Narration,
                Description = st.Remarks,
                TransactionDoneBy = st.Transaction.CreatedBy

            }).AsNoTracking().ToListAsync();

            if (shareTransactionReport != null && shareTransactionReport.Any())
            {
                var transactionSum = shareTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = shareTransactionReport[0];

                var previousAccount = await _dbContext.ShareTransactions
                .Include(st => st.Transaction)
                .Where(st => st.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(st => st.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                shareTransactionReportWrappr.ShareTransactionReports = shareTransactionReport;
                shareTransactionReportWrappr.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                shareTransactionReportWrappr.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                shareTransactionReportWrappr.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;
            }
            return shareTransactionReportWrappr;
        }

        public async Task<SubLedgerTransactionReportWrapper> GetSubLedgerTransactionReport(Expression<Func<SubLedgerTransaction, bool>> expressionOnSubLedgerTransaction)
        {
            SubLedgerTransactionReportWrapper subLedgerTransactionReportWrapper = new();
            var subLedgerTransactionReport =  await _dbContext.SubLedgerTransactions
            .Include(lt=>lt.Transaction)
            .Include(lt=>lt.SubLedger)
            .Where(expressionOnSubLedgerTransaction)
            .OrderBy(lt=>lt.Transaction.RealWorldCreationDate)
            .Select
            (lt=>
                new SubLedgerTransactionReport
                {
                    BaseTransactionId = lt.Transaction.Id,
                    SubLedgerTransactionId = lt.Id,
                    VoucherNumber = lt.Transaction.VoucherNumber,
                    TransactionRemarks = lt.Transaction.Remarks,
                    TransactionType = lt.TransactionType,
                    TransactionAmount = lt.Transaction.TransactionAmount,
                    Narration = lt.Narration,
                    Description = lt.Remarks,
                    RealWorldCreationDate = lt.Transaction.RealWorldCreationDate,
                    NepaliCreationDate = lt.Transaction.NepaliCreationDate,
                    EnglishCreationDate = lt.Transaction.EnglishCreationDate,
                    BalanceAfterTransaction = lt.BalanceAfterTransaction,
                    TransactionDoneBy = lt.Transaction.CreatedBy
                }
            ).AsNoTracking().ToListAsync();

             if (subLedgerTransactionReport != null && subLedgerTransactionReport.Any())
            {
                var transactionSum = subLedgerTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = subLedgerTransactionReport[0];

                var previousAccount = await _dbContext.SubLedgerTransactions
                .Include(st => st.Transaction)
                .Where(st => st.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(st => st.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                subLedgerTransactionReportWrapper.SubLedgerTransactionReports = subLedgerTransactionReport;
                subLedgerTransactionReportWrapper.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                subLedgerTransactionReportWrapper.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                subLedgerTransactionReportWrapper.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;
            }
            return subLedgerTransactionReportWrapper;
        }
    }
}