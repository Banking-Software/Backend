using System.Linq.Expressions;
using MicroFinance.Dtos.Reports;
using MicroFinance.Enums;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Helpers;

public class CommonExpression : ICommonExpression
{
    private readonly IHelper _helper;
    public CommonExpression(IHelper helper)
    {
        _helper=helper;
    }
    public Task<Expression<Func<DepositAccountTransaction, bool>>> GetExpressionForDepositAccountTransactionReport(DepositAccountTransactionReportParams depositAccountTransactionReportParams, DateTime fromDate, DateTime toDate)
    {
        Expression<Func<DepositAccountTransaction, bool>> expression;
        if(depositAccountTransactionReportParams.AccountStatus!=null)
        {
            expression = dat=>dat.DepositAccountId==depositAccountTransactionReportParams.DepositAccountId 
            && dat.DepositAccount.Status==depositAccountTransactionReportParams.AccountStatus
            && dat.Transaction.EnglishCreationDate >= fromDate && dat.Transaction.EnglishCreationDate <= toDate;
        }
        else
        {
            expression= dat=>dat.DepositAccountId==depositAccountTransactionReportParams.DepositAccountId
            && dat.Transaction.EnglishCreationDate >= fromDate && dat.Transaction.EnglishCreationDate <= toDate;
        }
        return Task.FromResult(expression);
    }

    public Task<Expression<Func<DepositAccount, bool>>> GetExpressionForInterestPosting(int depositAccountId)
    {
        Expression<Func<DepositAccount, bool>> expression = da
        =>da.Id==depositAccountId 
        && (da.Status == AccountStatusEnum.Active || da.Status == AccountStatusEnum.Mature) 
        && da.DepositScheme.IsActive && da.Client.IsActive;
        return Task.FromResult(expression);

        
    }

    public Task<Expression<Func<LedgerTransaction, bool>>> GetExpressionForLedgerTransactionReport(LedgerTransactionReportParams ledgerTransactionReportParams, DateTime fromDate, DateTime toDate)
    {
        Expression<Func<LedgerTransaction, bool>> expression;
        if(ledgerTransactionReportParams.LedgerId!=null)
        {
            expression = lt => lt.LedgerId == ledgerTransactionReportParams.LedgerId 
            && lt.Transaction.EnglishCreationDate >= fromDate
            && lt.Transaction.EnglishCreationDate <= toDate;
        }
        else
        {
            expression = lt =>lt.Transaction.EnglishCreationDate >= fromDate
            && lt.Transaction.EnglishCreationDate <= toDate;
        }
        return Task.FromResult(expression);
    }

    public Task<Expression<Func<ShareTransaction, bool>>> GetExpressionForShareTransactionReport(ShareTransactionReportParams shareTransactionReportParams, DateTime fromDate, DateTime toDate)
    {
        Expression<Func<ShareTransaction, bool>> expression;
        if(shareTransactionReportParams.ClientMemberId!=null && shareTransactionReportParams.ShareId!=null)
            expression = sa=>sa.ShareAccountId==shareTransactionReportParams.ShareId 
            && sa.ShareAccount.Client.ClientId==shareTransactionReportParams.ClientMemberId
            && sa.Transaction.EnglishCreationDate >= fromDate && sa.Transaction.EnglishCreationDate <= toDate;
        else if(shareTransactionReportParams.ShareId!=null)
            expression = sa=>sa.ShareAccountId==shareTransactionReportParams.ShareId 
            && sa.Transaction.EnglishCreationDate >= fromDate && sa.Transaction.EnglishCreationDate <= toDate;
        else if(shareTransactionReportParams.ClientMemberId!=null)
            expression = sa=>sa.ShareAccount.Client.ClientId==shareTransactionReportParams.ClientMemberId 
            && sa.Transaction.EnglishCreationDate >= fromDate && sa.Transaction.EnglishCreationDate <= toDate;
        else
            expression = sa => sa.Transaction.EnglishCreationDate >= fromDate && sa.Transaction.EnglishCreationDate <= toDate;
        return Task.FromResult(expression);
    }

    public Task<Expression<Func<SubLedgerTransaction, bool>>> GetExpressionForSubLedgerTransactionReport(SubLedgerTransactionReportParams SubLedgerTransactionReportParams, DateTime fromDate, DateTime toDate)
    {
        Expression<Func<SubLedgerTransaction, bool>> expression;
        if(SubLedgerTransactionReportParams.SubLedgerId!=null)
        {
            expression = lt => lt.SubLedgerId == SubLedgerTransactionReportParams.SubLedgerId 
            && lt.Transaction.EnglishCreationDate >= fromDate
            && lt.Transaction.EnglishCreationDate <= toDate;
        }
        else
        {
            expression = lt =>lt.Transaction.EnglishCreationDate >= fromDate
            && lt.Transaction.EnglishCreationDate <= toDate;
        }
        return Task.FromResult(expression);
    }

    public Task<Expression<Func<DepositAccount, bool>>> GetExpressionOfDepositAccountForTransaction(int depositAccountId, bool isDeposit)
    {
        // Deposit Transaction: Status either should be Active or Mature
        // WithDrawal Transaction: Status should be Active, Mature, or Suspend. Cannot be Close
        Expression<Func<DepositAccount, bool>> expression;
        if(isDeposit)
            expression = da=>da.Id==depositAccountId && da.DepositScheme.IsActive && (da.Status == AccountStatusEnum.Active || da.Status==AccountStatusEnum.Mature);
        else
            expression =da=>da.Id==depositAccountId && da.DepositScheme.IsActive && da.Status!=AccountStatusEnum.Close;
        return Task.FromResult(expression);
    }

    public Task<Expression<Func<ShareAccount, bool>>> GetExpressionOfShareAccountForTransaction(int? shareAccountId, string? cliendMemberId)
    {
         Expression<Func<ShareAccount, bool>> expression;
        if(cliendMemberId!=null && shareAccountId!=null)
            expression = sa=>sa.Id==shareAccountId && sa.IsActive && !sa.IsClose && sa.Client.IsActive && sa.Client.ClientId==cliendMemberId;
        else if(shareAccountId!=null)
            expression = sa=>sa.Id==shareAccountId && sa.IsActive && !sa.IsClose && sa.Client.IsActive;
        else if(cliendMemberId!=null)
            expression = sa=>sa.Client.ClientId==cliendMemberId && sa.IsActive && !sa.IsClose && sa.Client.IsActive;
        else
            expression = sa=>sa.IsActive && !sa.IsClose && sa.Client.IsActive;
        return Task.FromResult(expression);
    }
}