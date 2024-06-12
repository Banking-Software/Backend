using MicroFinance.Dtos;
using MicroFinance.Dtos.Reports;
using MicroFinance.Models.Wrapper.Reports;
using MicroFinance.Models.Wrapper.Reports.TrailBalance;

namespace MicroFinance.Services.Reports;
public interface ITransactionReportService
{
    Task<DepositAccountTransactionReportWrapperDto> GetDepositAccountTransactionReportService(DepositAccountTransactionReportParams depositAccountTransactionReportParams, TokenDto decodedToken);
    Task<LedgerTransactionReportWrapperDto> GetLedgerTransactionReportService(LedgerTransactionReportParams ledgerTransactionReportParams, TokenDto decodedToken);
    Task<SubLedgerTransactionReportWrapperDto> GetSubLedgerTransactionReportService(SubLedgerTransactionReportParams suLedgerTransactionReportParams, TokenDto decodedToken);
    Task<ShareAccountTransactionReportWrapperDto> GetShareAccountTransactionReportService(ShareTransactionReportParams shareTransactionReportParams ,TokenDto decodedToken);
    Task<TrailBalance> GetTrailBalanceService(string fromDate, string toDate);
}