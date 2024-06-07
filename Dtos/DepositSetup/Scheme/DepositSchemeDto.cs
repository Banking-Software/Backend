using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Scheme;

namespace MicroFinance.Dtos.DepositSetup
{
    public class DepositSchemeDto : BaseDepositDto
    {
        public string SchemeName { get; set; }
        public string? SchemeNameNepali { get; set; }
        public string SchemeType { get; set; }
        public int SchemeTypeId { get; set; }
        public string Symbol { get; set; }
        public int MinimumBalance { get; set; }
        public decimal InterestRateOnMinimumBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MinimumInterestRate { get; set; }
        public decimal MaximumInterestRate { get; set; }
        public CalculationTypeEnum Calculation { get; set; }
        public PostingSchemeEnum PostingScheme { get; set; }
        public int InterestSubLedgerId { get; set; }
        public string InterestSubledger { get; set; }
        public int DepositSubledgerId { get; set; }
        public string DepositSubLedger { get; set; }
        public int TaxSubledgerId { get; set; }
        public string TaxSubledger { get; set; }
    }
}