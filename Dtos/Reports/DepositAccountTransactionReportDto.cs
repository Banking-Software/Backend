using MicroFinance.Enums;

namespace MicroFinance.Dtos.Reports;


public class DepositAccountTransactionReportWrapperDto
{
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum { get; set; }
    public decimal? CreditSum { get; set; }
    public DepositAccountDetailsDto DepositAccountDetailsDto { get; set; }
    public List<DepositAccountTransactionReportDto> DepositAccountTransactionDtos {get;set;}
}

public class DepositAccountTransactionReportDto : BaseReportDto
{
    public int DepositAccountTransactionId { get; set; } // Current TransactionId
    public AccountStatusEnum Status { get; set; }
}

public class DepositAccountDetailsDto
{
    public string ClientName { get; set; }
    public string? ClientMemberId { get; set; }
    public string AccountNumber { get; set; }
    public string NepaliOpeningDate { get; set; }
    public string NepaliMatureDate { get; set; }
    public decimal InterestRate { get; set; }
    public decimal CurrentBalance { get; set; }
    public string NepaliNextInterestPositngDate { get; set; }
    public string? PANNumber { get; set; }
    public string? MobileNumber { get; set; }
    public string DepositScheme { get; set; }
}