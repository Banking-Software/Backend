using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.DepositSetup
{
    public class FlexibleInterestRateSetupDto
    {
        public int? Id { get; set; }
        public int DepositSchemeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int FromAmount { get; set; }
        public int ToAmount { get; set; }
        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }
    }
}