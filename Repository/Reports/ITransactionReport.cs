using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;

namespace MicroFinance.Repository.Reports;
public interface ITransactionReport
{
    Task<List<ShareTransactionReportWrappr>> GetShareTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int shareAccountId);
    Task<List<DepositAccountTransactionReportWrapper>> GetDepositAccountTransactionReport(DateWrapper dateFrom, DateWrapper dateTo, int depositAccountId);

}