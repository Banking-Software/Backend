namespace MicroFinance.Dtos.Reports;

public class ShareAccountTransactionReportWrapperDto
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum { get; set; }
    public decimal? CreditSum { get; set; }
    public List<ShareAccountTransactionReportDto> ShareAccountTransactionReportDtos {get;set;}
}

public class ShareAccountTransactionReportDto : BaseReportDto
{
    public int ShareTransactionId { get; set; } // Current TransactionId
}