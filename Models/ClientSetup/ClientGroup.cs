using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public virtual  ICollection<Client> Clients { get; set; }
    }
}