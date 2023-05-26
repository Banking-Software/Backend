using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientNomineeInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? NepaliName { get; set; }
        public string? Relation { get; set; }
        public string? NepaliRelation { get; set; }
        public string? IntroducedBy { get; set; }

        public virtual Client Client { get; set; }
    }
}