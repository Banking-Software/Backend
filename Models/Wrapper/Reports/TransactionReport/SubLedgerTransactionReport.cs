namespace MicroFinance.Models.Wrapper.Reports;
public class SubLedgerTransactionReport : BaseReportWrapper
{
    public int SubLedgerTransactionId { get; set; }
    public int SubLedgerId { get; set; }
}

public class SubLedgerTransactionReportWrapper
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum {get; set;}
    public decimal? CreditSum{get; set;}
    public List<SubLedgerTransactionReport> SubLedgerTransactionReports { get; set; }
}