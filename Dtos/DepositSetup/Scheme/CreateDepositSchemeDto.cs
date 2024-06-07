using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Scheme;

namespace MicroFinance.Dtos.DepositSetup
{
    
    public class CreateDepositSchemeDto : IValidatableObject
    {
        [Required]
        public string SchemeName { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string? SchemeNameNepali { get; set; }
        [Required]
        public DepositSchemeTypeEnum SchemeType { get; set; }
        [Required]
        public CalculationTypeEnum Calculation { get; set; }
        [Required]
        public PostingSchemeEnum PostingScheme { get; set; }

        [Required]
        public string Symbol { get; set; }
        [Required]
        public int MinimumBalance { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Interest Rate On Minimum Balance must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest Rate on must have up to two decimal places.")]
        public decimal InterestRateOnMinimumBalance { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal MinimumInterestRate { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal MaximumInterestRate { get; set; }
        public string? DepositSubledger { get; set; }
        public string? InterestSubledger { get; set; }
        public string? TaxSubledger { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(MinimumInterestRate > InterestRate || InterestRate > MaximumInterestRate)
            {
                yield return new ValidationResult("MinimumInterestRate<=InterestRate<=MaximumInterestRate constraint doesnot match");
            }
        }
    }


}