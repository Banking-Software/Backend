using System.Linq.Expressions;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;

namespace MicroFinance.Repository.Reports;
public interface ITransactionReportRepository
{
    Task<ShareTransactionReportWrapper> GetShareTransactionReport(Expression<Func<BaseTransaction, bool>> expressionOnBaseTransaction, Expression<Func<ShareAccount, bool>> expressionOnShareAccount);
    Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReport(Expression<Func<DepositAccountTransaction, bool>> expressionOnDepositAccountTransaction);

}