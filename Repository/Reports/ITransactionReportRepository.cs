using System.Linq.Expressions;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;
using MicroFinance.Models.Wrapper.Reports.TrailBalance;

namespace MicroFinance.Repository.Reports;
public interface ITransactionReportRepository
{
    Task<ShareTransactionReportWrapper> GetShareTransactionReport(Expression<Func<ShareTransaction, bool>> expressionOnShareTransaction);
    Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReport(Expression<Func<DepositAccountTransaction, bool>> expressionOnDepositAccountTransaction);
    Task<LedgerTransactionReportWrapper> GetLedgerTransactionReport(Expression<Func<LedgerTransaction, bool>> expressionOnLedgerTransaction);
    Task<SubLedgerTransactionReportWrapper> GetSubLedgerTransactionReport(Expression<Func<SubLedgerTransaction, bool>> expressionOnSubLedgerTransaction);
    Task<TrailBalance> GenerateTrailBalanceReport(DateTime fromDate, DateTime toDate);

}