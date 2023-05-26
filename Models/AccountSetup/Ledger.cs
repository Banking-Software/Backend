using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.DepositSetup;

namespace MicroFinance.Models.AccountSetup
{
    public class Ledger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        [Column(TypeName ="decimal(5,2)")]
        public decimal? DepreciationRate { get; set; }
        public string? HisabNumber { get; set; }
        [Required]
        public bool IsSubLedgerActive { get; set; } // If True then allow to create Sub Ledger        
        public virtual GroupTypeAndLedgerMap GroupTypeAndLedgerMap { get; set; }
        public virtual ICollection<SubLedger> SubLedger { get; set; } 

        public virtual DepositScheme LiabilityAccount { get; set; }
        public virtual DepositScheme InterestAccount { get; set; }
    }
}