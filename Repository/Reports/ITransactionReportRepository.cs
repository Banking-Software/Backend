using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;

namespace MicroFinance.Repository.Reports;
public interface ITransactionReportRepository
{
    Task<ShareTransactionReportWrapper> GetShareTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int shareAccountId);
    Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int depositAccountId);

}