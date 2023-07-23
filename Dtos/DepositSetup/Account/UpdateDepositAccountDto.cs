using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.DepositSetup
{
    public class UpdateDepositAccountDto
    {
         public int Id { get; set; }
        public int? Period { get; set; }
        public PeriodTypeEnum? PeriodType { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public int? JointClientId { get; set; }
        public DateTime? MatureDate { get; set; }

        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }
        public string? MatureInterestPostingAccountNumber { get; set; }
        public string? Description { get; set; }
        public AccountStatusEnum Status { get; set; }
        public bool IsSMSServiceActive { get; set; }
        public int? DailyDepositAmount { get; set; }
        public int? TotalDepositDay { get; set; }
        public int? TotalDepositAmount { get; set; }
        public int? TotalReturnAmount { get; set; }
        public int? TotalInterestAmount { get; set; }
    }
}