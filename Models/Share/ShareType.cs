using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Models.Share
{
    public class ShareType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
    }
}