using System.Linq.Expressions;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
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



        private static Expression<Func<BaseTransaction, bool>> GetDateFilterPredicate(DateWrapper dateFrom, DateWrapper dateTo)
        {
            return trasc =>
            (trasc.TransactionYear == dateFrom.Year && trasc.TransactionMonth == dateFrom.Month && trasc.TransactionDay >= dateFrom.Day) ||
            (trasc.TransactionYear == dateTo.Year && trasc.TransactionMonth == dateTo.Month && trasc.TransactionDay <= dateTo.Day) ||
            (trasc.TransactionYear == dateFrom.Year && trasc.TransactionMonth > dateFrom.Month && trasc.TransactionMonth < dateTo.Month) ||
            (trasc.TransactionYear > dateFrom.Year && trasc.TransactionYear < dateTo.Year) ||
            (trasc.TransactionYear == dateTo.Year && trasc.TransactionMonth < dateTo.Month);
        }

        public async Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int depositAccountId)
        {
            Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            DepositAccountTransactionReportWrapper depositAccountTransactionReportWrapper = new();
            // Get the report based on provided date
            var depositAccountTransactionReport =
            await (from dat in _dbContext.DepositAccountTransactions
                   join transac in _dbContext.Transactions on dat.TransactionId equals transac.Id
                   where dat.DepositAccountId.Equals(depositAccountId) && dateFilterPredicate.Compile()(transac)
                   orderby transac.RealWorldCreationDate
                   select new DepositAccountTransactionReport
                   {
                       BaseTransactionId = transac.Id,
                       DepositAccountTransactionId= dat.Id,
                       VoucherNumber = transac.VoucherNumber,
                       TransactionRemarks = transac.Remarks,
                       TransactionType = dat.TransactionType,
                       TransactionAmount = transac.TransactionAmount,
                       Narration = dat.Narration,
                       Description = dat.Remarks,
                       RealWorldCreationDate = transac.RealWorldCreationDate,
                       CompanyCalendarCreationDate = transac.CompanyCalendarCreationDate,
                       BalanceAfterTransaction = dat.BalanceAfterTransaction,
                       TransactionDoneBy = transac.CreatedBy
                   }).AsNoTracking().ToListAsync();
            

            if(depositAccountTransactionReport!=null && depositAccountTransactionReport.Any())
            {
                // Find the total amount in debit and credit side
                var transactionSum = depositAccountTransactionReport
                .GroupBy(report=>report.TransactionType)
                .Select(grp=>new {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });
                // Get the BalanceAfterTransaction exact before our first entry of report: OpeningBalance
                var firstReport = depositAccountTransactionReport[0];
                var previousAccount = await _dbContext.DepositAccountTransactions
                .Include(dat=>dat.Transaction)
                .Where(dat=>dat.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(dat=>dat.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                depositAccountTransactionReportWrapper.DepositAccountTransactionReports = depositAccountTransactionReport;
                depositAccountTransactionReportWrapper.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                depositAccountTransactionReportWrapper.CreditSum = transactionSum.Where(ts=>ts.TransactionType==TransactionTypeEnum.Credit).SingleOrDefault().TransactionSumAmount;
                depositAccountTransactionReportWrapper.DebitSum = transactionSum.Where(ts=>ts.TransactionType==TransactionTypeEnum.Debit).SingleOrDefault().TransactionSumAmount;

            }

            return depositAccountTransactionReportWrapper;
        }

        public async Task<ShareTransactionReportWrapper> GetShareTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int shareAccountId)
        {
            Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            ShareTransactionReportWrapper shareTransactionReportWrappr = new();
            var shareTransactionReport =
            await (from st in _dbContext.ShareTransactions
                   join trasc in _dbContext.Transactions on st.TransactionId equals trasc.Id
                   where st.ShareAccountId.Equals(shareAccountId) && dateFilterPredicate.Compile()(trasc)
                   orderby trasc.RealWorldCreationDate
                   select new ShareTransactionReport
                   {
                       BaseTransactionId = trasc.Id,
                       ShareTransactionId= st.Id,
                       VoucherNumber = trasc.VoucherNumber,
                       TransactionRemarks = trasc.Remarks,
                       TransactionType = st.TransactionType,
                       TransactionAmount = trasc.TransactionAmount,
                       Narration = st.Narration,
                       Description = st.Remarks,
                       RealWorldCreationDate = trasc.RealWorldCreationDate,
                       CompanyCalendarCreationDate = trasc.CompanyCalendarCreationDate,
                       BalanceAfterTransaction = st.BalanceAfterTransaction,
                       TransactionDoneBy = trasc.CreatedBy
                   }).AsNoTracking().ToListAsync();

            if(shareTransactionReport!=null && shareTransactionReport.Any())
            {
                var transactionSum = shareTransactionReport
                .GroupBy(report=>report.TransactionType)
                .Select(grp=>new {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = shareTransactionReport[0];

                var previousAccount = await _dbContext.ShareTransactions
                .Include(dat=>dat.Transaction)
                .Where(dat=>dat.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate)
                .OrderByDescending(dat=>dat.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                shareTransactionReportWrappr.ShareTransactionReports = shareTransactionReport;
                shareTransactionReportWrappr.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                shareTransactionReportWrappr.CreditSum = transactionSum.Where(ts=>ts.TransactionType==TransactionTypeEnum.Credit).SingleOrDefault().TransactionSumAmount;
                shareTransactionReportWrappr.DebitSum = transactionSum.Where(ts=>ts.TransactionType==TransactionTypeEnum.Debit).SingleOrDefault().TransactionSumAmount;
            }
            return shareTransactionReportWrappr;
        }
    }
}