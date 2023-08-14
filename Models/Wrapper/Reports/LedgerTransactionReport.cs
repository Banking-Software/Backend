namespace MicroFinance.Models.Wrapper.Reports;
public class LedgerTransactionReport : BaseReportWrapper
{
    public int LedgerTransactionId { get; set; }
}

public class LedgerTransactionReportWrapper
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum {get; set;}
    public decimal? CreditSum{get; set;}
    public List<LedgerTransactionReport> LedgerTransactionReports { get; set; }
}