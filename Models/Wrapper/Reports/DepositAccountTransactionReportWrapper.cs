using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Models.Wrapper.Reports;


public class DepositAccountTransactionReport : BaseReportWrapper
{
    public int DepositAccountTransactionId { get; set; } // Current TransactionId
    public AccountStatusEnum Status { get; set; }
}
public class DepositAccountTransactionReportWrapper
{
    // Opening Balance
   public decimal? PreviousBalanceAfterTransaction { get; set; }
   public decimal? DebitSum {get; set;}
   public decimal? CreditSum{get; set;}
   public IList<DepositAccountTransactionReport> DepositAccountTransactionReports;
}