using System.Linq.Expressions;
using MicroFinance.Dtos.Reports;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Helper;

public interface ICommonExpression
{
    Task<Expression<Func<DepositAccount, bool>>> GetExpressionOfDepositAccountForTransaction(int depositAccountId, bool isDeposit);
    Task<Expression<Func<ShareAccount, bool>>> GetExpressionOfShareAccountForTransaction(int? shareAccountId,  string? cliendMemberId);
    Task<Expression<Func<DepositAccountTransaction, bool>>> GetExpressionForDepositAccountTransactionReport(DepositAccountTransactionReportParams depositAccountTransactionReportParams, DateTime fromDate, DateTime toDate);
    Task<Expression<Func<ShareTransaction, bool>>> GetExpressionForShareTransactionReport(ShareTransactionReportParams shareTransactionReportParams, DateTime fromDate, DateTime toDate);
    Task<Expression<Func<LedgerTransaction, bool>>> GetExpressionForLedgerTransactionReport(LedgerTransactionReportParams ledgerTransactionReportParams, DateTime fromDate, DateTime toDate);
    Task<Expression<Func<SubLedgerTransaction, bool>>> GetExpressionForSubLedgerTransactionReport(SubLedgerTransactionReportParams SubLedgerTransactionReportParams, DateTime fromDate, DateTime toDate);



}