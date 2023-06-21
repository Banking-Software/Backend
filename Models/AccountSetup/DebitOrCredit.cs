using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.AccountSetup
{
    public class DebitOrCredit
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // public virtual ICollection<GroupType> GroupTypes { get; set; }
    }
}