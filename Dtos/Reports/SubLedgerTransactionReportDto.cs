namespace MicroFinance.Dtos.Reports;


public class SubLedgerTransactionReportWrapperDto
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum { get; set; }
    public decimal? CreditSum { get; set; }
    public List<SubLedgerTransactionReportDto> SubLedgerTransactionReportDtos {get;set;}
}

public class SubLedgerTransactionReportDto : BaseReportDto
{
    public int SubLedgerTransactionId { get; set; } // Current TransactionId
}