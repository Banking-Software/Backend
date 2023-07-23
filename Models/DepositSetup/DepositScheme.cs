using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Scheme;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.DepositSetup
{
    public class DepositScheme : BaseDeposit
    {
        
        public string SchemeName { get; set; }
        public string? SchemeNameNepali { get; set; }
        public virtual Ledger SchemeType { get; set; }
        public int? SchemeTypeId { get; set; }
        public string Symbol { get; set; }
        public int MinimumBalance { get; set; }
        public decimal InterestRateOnMinimumBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MinimumInterestRate { get; set; }
        public decimal MaximumInterestRate { get; set; }
        public CalculationTypeEnum Calculation { get; set; }
        public PostingSchemeEnum PostingScheme { get; set; }
        public int? InterestSubLedgerId { get; set; }
        public virtual SubLedger InterestSubledger { get; set; }
        public int? DepositSubledgerId { get; set; }
        public virtual SubLedger DepositSubLedger { get; set; }
        public int? TaxSubledgerId { get; set; }
        public virtual SubLedger TaxSubledger { get; set; }
        public virtual ICollection<DepositAccount> DepositAccounts { get; set; }
        public virtual ICollection<FlexibleInterestRate> FlexibleInterestRates { get; set; }

    }
}