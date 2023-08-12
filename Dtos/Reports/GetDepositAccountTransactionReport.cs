using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.Reports;

public class GetDepositAccountTransactionReport
{
    public string FromDate { get; set; }
    public string ToDate { get; set; }
    public int DepositAccountId { get; set; }
    public AccountStatusEnum? AccountStatus { get; set; }
}