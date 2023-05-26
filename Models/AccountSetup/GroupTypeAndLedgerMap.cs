using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Models.AccountSetup
{
    [Index(nameof(GroupTypeId), nameof(LedgerId), IsUnique = true)]
    public class GroupTypeAndLedgerMap
    {
        [Key]
        public int Id { get; set; }
        public int GroupTypeId { get; set; }
        public int LedgerId { get; set; }
        public virtual GroupType GroupType { get; set; }
        public virtual Ledger Ledger { get; set; }

    }
}