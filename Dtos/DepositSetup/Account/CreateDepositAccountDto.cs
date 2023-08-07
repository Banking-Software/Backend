using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.DepositSetup
{
    public class CreateDepositAccountDto : IValidatableObject
    {
        [Required]
        public int DepositSchemeId { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public string OpeningDate { get; set; }
        [Required]
        public int Period { get; set; }
        [Required]
        public PeriodTypeEnum PeriodType { get; set; }
        [Required]
        public AccountTypeEnum AccountType { get; set; }
        public List<int>? JointClientIds { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }
        public int? ReferredByEmployeeId { get; set; }
        public string? NomineeName { get; set; }
        public RelationTypeEnum? Relation { get; set; }
        public int? InterestPostingAccountId { get; set; }
        public int? MatureInterestPostingAccountId { get; set; }
        public string? Description { get; set; }
        public AccountStatusEnum Status { get; set; }
        public bool IsSMSServiceActive { get; set; }
        public int? ExpectedDailyDepositAmount { get; set; }
        public int? ExpectedTotalDepositDay { get; set; }
        public int? ExpectedTotalDepositAmount { get; set; }
        public int? ExpectedTotalReturnAmount { get; set; }
        public int? ExpectedTotalInterestAmount { get; set; }
        public IFormFile? SignaturePhoto { get; set; }

         public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(AccountType == AccountTypeEnum.Joint && JointClientIds.Count<1)
            {
                yield return new ValidationResult("No Joint Client Received. Please provide Client Id if you want to create joint account, otherwise select single account type");
            }
            if(PeriodType==PeriodTypeEnum.Month && (Period < 1 || Period > 12))
            {
                yield return new ValidationResult("1<=Months<=12 constraint dosesn't meet in the Period and PeriodType");
            }
            if(PeriodType==PeriodTypeEnum.Day && (Period<1 || Period > 32))
            {
                yield return new ValidationResult("1<=Days<=32 constraint dosesn't meet in the Period and PeriodType");
            }
        }
    }
}