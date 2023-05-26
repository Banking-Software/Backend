using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.DepositSetup
{
    public class UpdateInterestRateByDepositSchemeDto
    {
        [Range(0, 99, ErrorMessage = "Old Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Old Interest must have up to two decimal places.")]
        public decimal InterestRateChangeValue { get; set; } 
        public int DepositSchemeId { get; set; }
    }
}