using System.ComponentModel.DataAnnotations;


///
// This Dto is for updating interest rate based on past interest rate
///
namespace MicroFinance.Dtos.DepositSetup
{
    public class ChangeInterestRateByDepositSchemeDto
    {
        [Range(0, 100, ErrorMessage = "Old Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Old Interest must have up to two decimal places.")]
        public decimal OldInterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "New Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "New Interest must have up to two decimal places.")]
        public decimal NewInterestRate { get; set; }
        public int DepositSchemeId { get; set; }
    }
}