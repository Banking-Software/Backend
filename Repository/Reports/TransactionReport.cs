using System.Linq.Expressions;
using MicroFinance.DBContext;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Reports
{
    public class TransactionReport : ITransactionReport
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionReport(ApplicationDbContext dbContext)
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

        public async Task<List<DepositAccountTransactionReportWrapper>> GetDepositAccountTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int depositAccountId)
        {
            Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            var depositAccountTransactionReport =
            await (from dat in _dbContext.DepositAccountTransactions
                   join transac in _dbContext.Transactions on dat.TransactionId equals transac.Id
                   where dat.DepositAccountId.Equals(depositAccountId) && dateFilterPredicate.Compile()(transac)
                   select new DepositAccountTransactionReportWrapper
                   {
                       BaseTransactionId = transac.Id,
                       DepositAccountTransactionId = dat.Id,
                       VoucherNumber = transac.VoucherNumber,
                       TransactionRemarks = transac.Remarks,
                       TransactionType = dat.TransactionType,
                       Narration = dat.Narration,
                       Description = dat.Remarks,
                       RealWorldCreationDate = transac.RealWorldCreationDate,
                       CompanyCalendarCreationDate = transac.CompanyCalendarCreationDate,
                       BalanceAfterTransaction = dat.BalanceAfterTransaction,
                       TransactionDoneBy = transac.CreatedBy
                   }).AsNoTracking().ToListAsync();
            
            return depositAccountTransactionReport;
        }

        public async Task<List<ShareTransactionReportWrappr>> GetShareTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int shareAccountId)
        {
            Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            var shareTransactionReport =
            await (from st in _dbContext.ShareTransactions
                   join trasc in _dbContext.Transactions on st.TransactionId equals trasc.Id
                   where st.ShareAccountId.Equals(shareAccountId) && dateFilterPredicate.Compile()(trasc)
                   select new ShareTransactionReportWrappr
                   {
                       BaseTransactionId = trasc.Id,
                       ShareTransactionId = st.Id,
                       VoucherNumber = trasc.VoucherNumber,
                       TransactionRemarks = trasc.Remarks,
                       TransactionType = st.TransactionType,
                       Narration = st.Narration,
                       Description = st.Remarks,
                       RealWorldCreationDate = trasc.RealWorldCreationDate,
                       CompanyCalendarCreationDate = trasc.CompanyCalendarCreationDate,
                       BalanceAfterTransaction = st.BalanceAfterTransaction,
                       TransactionDoneBy = trasc.CreatedBy
                   }).AsNoTracking().ToListAsync();

            return shareTransactionReport;
        }
    }
}