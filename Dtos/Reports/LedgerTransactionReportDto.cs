namespace MicroFinance.Dtos.Reports;


public class LedgerTransactionReportWrapperDto
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum { get; set; }
    public decimal? CreditSum { get; set; }
    public List<LedgerTransactionReportDto> LedgerTransactionReportDtos {get;set;}
}

public class LedgerTransactionReportDto : BaseReportDto
{
    public int LedgerTransactionId { get; set; } // Current TransactionId
}