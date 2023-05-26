using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Enums;

namespace MicroFinance.Dtos.DepositSetup
{
    public class DepositAccountDto
    {

        public int Id { get; set; }
        public DepositSchemeDto DepositScheme { get; set; }
        public string AccountNumber { get; set; }
        public ClientDto Client { get; set; }
        public DateTime OpeningDate { get; set; }
        public int? Period { get; set; }
        public string PeriodType { get; set; }
        public string AccountType { get; set; }
        public ClientDto JointClient { get; set; }
        public DateTime? MatureDate { get; set; }
        public decimal InterestRate { get; set; }
        public int MinimumBalance { get; set; }
        public string ReferredBy { get; set; }
        public string InterestPostingAccountNumber { get; set; }
        public string? MatureInterestPostingAccountNumber { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public bool IsSMSServiceActive { get; set; }
        public int? DailyDepositAmount { get; set; }
        public int? TotalDepositDay { get; set; }
        public int? TotalDepositAmount { get; set; }
        public int? TotalReturnAmount { get; set; }
        public int? TotalInterestAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}