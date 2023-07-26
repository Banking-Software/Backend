using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Services;

namespace MicroFinance.Models.DepositSetup
{
    public class DepositAccount : BaseDeposit
    {
        public int DepositSchemeId { get; set; }
        public virtual DepositScheme DepositScheme { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime OpeningDate { get; set; }
        public int Period { get; set; }
        public PeriodTypeEnum PeriodType { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public DateTime MatureDate { get; set; }
        public decimal InterestRate { get; set; }
        [ValidationForNegativeBalanceService]
        public decimal PrincipalAmount { get; set; } = 0;
        [ValidationForNegativeBalanceService]
        public decimal InterestAmount { get; set; } = 0;
        public int ReferredByEmployeeId { get; set; }
        public RelationTypeEnum? Relation { get; set; }
        public string? NomineeName { get; set; }
        public string? Description { get; set; }
        public AccountStatusEnum Status { get; set; }
        public bool IsSMSServiceActive { get; set; }
        public int? ExpectedDailyDepositAmount { get; set; }
        public int? ExpectedTotalDepositDay { get; set; }
        public int? ExpectedTotalDepositAmount { get; set; }
        public int? ExpectedTotalReturnAmount { get; set; }
        public int? ExpectedTotalInterestAmount { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int? InterestPostingAccountNumberId { get; set; }
        public DepositAccount? InterestPostingAccountNumber { get; set; }
        public int? MatureInterestPostingAccountNumberId { get; set; }
        public DepositAccount? MatureInterestPostingAccountNumber { get; set; }
        public virtual ICollection<JointAccount> JointAccounts { get; set; }
        public virtual ICollection<DepositAccountTransaction> DepositAccountTransactions { get; set; }
    }
}