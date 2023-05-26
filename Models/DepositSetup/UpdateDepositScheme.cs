using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Models.DepositSetup
{
    public class UpdateDepositScheme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MinimumBalance { get; set; }
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal MinimumInterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal MaximumInterestRate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}