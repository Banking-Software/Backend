using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Models.AccountSetup
{
    public class GroupSubLedger
    {
        [Required]
        public SubLedger SubLedger { get; set; }
        [Required]
        public GroupType GroupType { get; set; }
    }
}