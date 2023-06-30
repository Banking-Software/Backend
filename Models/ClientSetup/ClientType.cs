using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}