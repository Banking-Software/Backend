namespace MicroFinance.Dtos.Reports;

public class ShareTransactionReportParams : DateFilterDto
{
    public int? ShareId { get; set; }
    public string? ClientMemberId { get; set; }
}