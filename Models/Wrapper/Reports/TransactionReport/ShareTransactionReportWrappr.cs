namespace MicroFinance.Models.Wrapper.Reports;


public class ShareTransactionReport : BaseReportWrapper
{
    public int ShareTransactionId { get; set; } // Current TransactionId
    public int ShareAccountId { get; set; }
}
public class ShareTransactionReportWrapper
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum {get; set;}
    public decimal? CreditSum{get; set;}
    public List<ShareTransactionReport> ShareTransactionReports { get; set; }
}