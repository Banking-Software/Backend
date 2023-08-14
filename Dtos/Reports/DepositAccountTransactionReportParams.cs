using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.Reports;

public class DepositAccountTransactionReportParams : DateFilterDto
{
    public int DepositAccountId { get; set; }
    public AccountStatusEnum? AccountStatus { get; set; }
}