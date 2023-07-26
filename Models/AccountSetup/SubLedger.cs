using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Services;

namespace MicroFinance.Models.AccountSetup
{
    public class SubLedger
    {
        [Key]
        public int Id { get; set; }
        public int? SubLedgerCode { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName ="decimal(18,4)")]
        [ValidationForNegativeBalanceService]
        public decimal CurrentBalance { get; set; }=0;
        public int LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }
        public virtual DepositScheme InterestSchemes { get; set; }
        public virtual DepositScheme DepositSchemes { get; set; }
        public virtual DepositScheme TaxSchemes { get; set; }
        public virtual ICollection<SubLedgerTransaction> SubLedgerTransactions { get; set; }
    }
}