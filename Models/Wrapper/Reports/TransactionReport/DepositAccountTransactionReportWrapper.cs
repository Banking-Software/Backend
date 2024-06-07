using MicroFinance.Enums;

namespace MicroFinance.Models.Wrapper.Reports;


public class DepositAccountTransactionReport : BaseReportWrapper
{
    public int DepositAccountTransactionId { get; set; } // Current TransactionId
    public int DepositAccountId { get; set; }
    public AccountStatusEnum Status { get; set; }
}
public class DepositAccountTransactionReportWrapper
{
    // Opening Balance
    public decimal? PreviousBalanceAfterTransaction { get; set; }
    public decimal? DebitSum { get; set; }
    public decimal? CreditSum { get; set; }
    public DepositAccountDetails depositAccountDetails { get; set; }
    public List<DepositAccountTransactionReport> DepositAccountTransactionReports { get; set; }
}

public class DepositAccountDetails
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