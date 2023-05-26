using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Models.DepositSetup
{
    public class DepositScheme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NameNepali { get; set; }
        [Required]
        public string DepositType { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public int MinimumBalance { get; set; } //Balance required to open account
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRateOnMinimumBalance { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal InterestRate { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MinimumInterestRate { get; set; }
        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal MaximumInterestRate { get; set; }
        [Required]
        public string Calculation { get; set; } = "Daily Balance";

        public virtual PostingScheme PostingScheme { get; set; }
        public int PostingSchemeId { get; set; }

        public int? ClosingCharge { get; set; }

        public int LedgerAsLiabilityAccountId { get; set; }
        public int LedgerAsInterestAccountId { get; set; }
        public Ledger LedgerAsLiabilityAccount { get; set; }
        public Ledger LedgerAsInterestAccount { get; set; }

        public int? FineAmount { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<DepositAccount> DepositAccounts { get; set; }
        public virtual ICollection<FlexibleInterestRate> FlexibleInterestRates { get; set; }

    }
}