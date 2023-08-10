namespace MicroFinance.Models.Wrapper.Reports;


public class DepositAccountTransactionReport : BaseReportWrapper
{
    public int DepositAccountTransactionId { get; set; } // Current TransactionId
}
public class DepositAccountTransactionReportWrapper
{
    // Opening Balance
   public decimal? PreviousBalanceAfterTransaction { get; set; }
   public decimal DebitSum {get; set;}
   public decimal CreditSum{get; set;}
   public List<DepositAccountTransactionReport> DepositAccountTransactionReports;
}