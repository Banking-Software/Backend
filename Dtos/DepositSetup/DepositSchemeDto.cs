using MicroFinance.Dtos.AccountSetup.MainLedger;

namespace MicroFinance.Dtos.DepositSetup
{
    public class DepositSchemeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? NameNepali { get; set; }
        public string DepositType { get; set; }
        public string Symbol { get; set; }
        public int MinimumBalance { get; set; }
        public decimal InterestRateOnMinimumBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MinimumInterestRate { get; set; }
        public decimal MaximumInterestRate { get; set; }
        public string Calculation { get; set; }
        public int? FineAmount { get; set; }
        public int? ClosingCharge { get; set; }
        public string PostingScheme { get; set; }
        public LedgerDto LiabilityAccount { get; set; }
        public LedgerDto InterestAccount { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}