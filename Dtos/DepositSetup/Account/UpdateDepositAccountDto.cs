using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;

namespace MicroFinance.Dtos.DepositSetup
{
    public class UpdateDepositAccountDto : IValidatableObject
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal InterestRate { get; set; }
        public AccountStatusEnum AccountStatus { get; set; }
        public int? InterestPostingAccountId { get; set; }
        public int? MatureInterestPostingAccountId { get; set; }
        public string? Description { get; set; }
        public string? NomineeName { get; set; }
        public RelationTypeEnum? Relation { get; set; }
        [Required]
        public bool IsSignatureChanged { get; set; }
        public IFormFile? SignaturePhoto { get; set; }
        [Required]
        public bool IsSMSServiceActive { get; set; }
        public int? ExpectedDailyDepositAmount { get; set; }
        public int? ExpectedTotalDepositDay { get; set; }
        public int? ExpectedTotalDepositAmount { get; set; }
        public int? ExpectedTotalReturnAmount { get; set; }
        public int? ExpectedTotalInterestAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MatureInterestPostingAccountId!=null && MatureInterestPostingAccountId==Id)
            {
                yield return new ValidationResult("Cannot be eqaul to current account", new[] { nameof(MatureInterestPostingAccountId)});
            }
            if (InterestPostingAccountId!=null && InterestPostingAccountId==Id)
            {
                yield return new ValidationResult("Cannot be eqaul to current account", new[] { nameof(MatureInterestPostingAccountId) });
            }
        }
    }
}