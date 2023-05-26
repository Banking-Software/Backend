using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientContactInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? MobileNumber1 { get; set; }
        public string? MobileNumber2 { get; set; }
        public string? TelephoneNumber1 { get; set; }
        public string? TelephoneNumber2 { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public virtual Client Client { get; set; }
    }
}