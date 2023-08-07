using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Models.AccountSetup
{
    public class BankSetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual Ledger Ledger { get; set; }
        public int LedgerId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? NepaliName { get; set; }
        [Required]
        public string BankBranch { get; set; }
        [Required]
        public string AccountNumber { get; set; }

        public virtual BankType BankType { get; set; }
        public int BankTypeId { get; set; }

        [Column(TypeName ="decimal(5,2)")]
        public decimal? InterestRate { get; set; }
        [Required]
        public string BranchCode { get; set; }
        [Column(TypeName ="decimal(18,4)")]
        public decimal TotalInterestBalance { get; set; }

        public virtual ICollection<BaseTransaction> BaseTransactions { get; set; }
    }
}