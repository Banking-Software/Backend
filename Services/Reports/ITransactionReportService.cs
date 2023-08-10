using MicroFinance.Dtos;
using MicroFinance.Models.Wrapper.Reports;

namespace MicroFinance.Services.Reports;
public interface ITransactionReportService
{
    Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReportService(string fromDate, string toDate, int depositAccountId, TokenDto decodedToken);
    Task<ShareTransactionReportWrapper> GetShareAccountTransactionReportService(string fromDate, string toDate, int shareAccountId, TokenDto decodedToken);
}