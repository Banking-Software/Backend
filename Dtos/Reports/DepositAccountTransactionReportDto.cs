using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.Reports;


public class DepositAccountTransactionReportWrapperDto
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum { get; set; }
    public decimal? CreditSum { get; set; }
    public List<DepositAccountTransactionReportDto> DepositAccountTransactionDtos {get;set;}
}

public class DepositAccountTransactionReportDto : BaseReportDto
{
    public int DepositAccountTransactionId { get; set; } // Current TransactionId
    public AccountStatusEnum Status { get; set; }
}