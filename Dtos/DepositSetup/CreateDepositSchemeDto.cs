using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;

namespace MicroFinance.Dtos.DepositSetup
{
    
    public class CreateDepositSchemeDto
    {
        public string Name { get; set; }
        public string? NameNepali { get; set; }
        public DepositTypeEnum DepositType { get; set; }
        public string Symbol { get; set; }
        public int MinimumBalance { get; set; }
        [Range(0, 100, ErrorMessage = "Interest Rate On Minimum Balance must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest Rate on must have up to two decimal places.")]
        public decimal InterestRateOnMinimumBalance { get; set; }

        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal InterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal MinimumInterestRate { get; set; }

        [Range(0, 100, ErrorMessage = "Interest must be a decimal value between 0 and 100.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Interest must have up to two decimal places.")]
        public decimal MaximumInterestRate { get; set; }
        public int? FineAmount { get; set; }
        public int? ClosingCharge { get; set; }
        public PostingSchemeEnum Posting { get; set; }
        public string? LiabilityAccount { get; set; }
        public string? InterestAccount { get; set; }
    }
}