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
            IList<DepositAccountTransactionReport> depositAccountTransactionReport =
            await _dbContext.DepositAccountTransactions.Include(dat => dat.Transaction).Include(dat => dat.DepositAccount)
            .Where(expressionOnDepositAccountTransaction).Select
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
                    TransactionDoneBy = dat.Transaction.CreatedBy
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

        public async Task<ShareTransactionReportWrapper> GetShareTransactionReport(Expression<Func<BaseTransaction, bool>> expressionOnBaseTransaction, Expression<Func<ShareAccount, bool>> expressionOnShareAccount)
        {
            // Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            ShareTransactionReportWrapper shareTransactionReportWrappr = new();
            var shareTransactionReport =
            await (from st in _dbContext.ShareTransactions
                   join trasc in _dbContext.Transactions on st.TransactionId equals trasc.Id
                   join sa in _dbContext.ShareAccounts on st.ShareAccountId equals sa.Id
                   where expressionOnShareAccount.Compile()(sa)
                   && expressionOnBaseTransaction.Compile()(trasc)
                   //&& dateFilterPredicate.Compile()(trasc)
                   orderby trasc.RealWorldCreationDate
                   select new ShareTransactionReport
                   {
                       BaseTransactionId = trasc.Id,
                       ShareTransactionId = st.Id,
                       VoucherNumber = trasc.VoucherNumber,
                       TransactionRemarks = trasc.Remarks,
                       TransactionType = st.TransactionType,
                       TransactionAmount = trasc.TransactionAmount,
                       Narration = st.Narration,
                       Description = st.Remarks,
                       RealWorldCreationDate = trasc.RealWorldCreationDate,
                       NepaliCreationDate = trasc.NepaliCreationDate,
                       EnglishCreationDate = trasc.EnglishCreationDate,
                       BalanceAfterTransaction = st.BalanceAfterTransaction,
                       TransactionDoneBy = trasc.CreatedBy
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
                .Include(dat => dat.Transaction)
                .Where(dat => dat.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(dat => dat.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                shareTransactionReportWrappr.ShareTransactionReports = shareTransactionReport;
                shareTransactionReportWrappr.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                shareTransactionReportWrappr.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault().TransactionSumAmount;
                shareTransactionReportWrappr.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault().TransactionSumAmount;
            }
            return shareTransactionReportWrappr;
        }
    }
}