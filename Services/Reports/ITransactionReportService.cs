using MicroFinance.Dtos;
using MicroFinance.Dtos.Reports;
using MicroFinance.Models.Wrapper.Reports;

namespace MicroFinance.Services.Reports;
public interface ITransactionReportService
{
    Task<DepositAccountTransactionReportWrapperDto> GetDepositAccountTransactionReportService(GetDepositAccountTransactionReport getDepositAccountTransactionReport, TokenDto decodedToken);
    Task<ShareTransactionReportWrapper> GetShareAccountTransactionReportService(string fromDate, string toDate, int shareAccountId, TokenDto decodedToken);
}