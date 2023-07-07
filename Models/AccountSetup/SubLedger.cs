using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }
    }
}