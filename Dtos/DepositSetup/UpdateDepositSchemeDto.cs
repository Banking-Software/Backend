using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.DepositSetup
{
    public class UpdateDepositSchemeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinimumBalance { get; set; }
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "Minimum Interest Rate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Minimum Interest Rate must have up to two decimal places.")]
        public decimal MinimumInterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "Maximum Interest Rate must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Maximum Interest Rate must have up to two decimal places.")]
        public decimal MaximumInterestRate { get; set; }
        [Range(0, 100, ErrorMessage = "Interest Rate On Minimum Balance must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest Rate on Minimum Balance must have up to two decimal places.")]
        public decimal InterestRateOnMinimumBalance { get; set; }
    }
}