namespace MicroFinance.Dtos.Reports;
public class LedgerTransactionReportParams : DateFilterDto
{
    public int? LedgerId { get; set; }
}