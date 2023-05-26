using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientFamilyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string MotherName { get; set; }
        public string? MotherNameNepali { get; set; }
        [Required]
        public string FatherName { get; set; }
        public string? FatherNameNepali { get; set; }
        [Required]
        public string GrandFatherName { get; set; }
        public string? GrandFatherNameNepali { get; set; }
        public string? SpouseName { get; set; }
        public string? SpouseOccupation { get; set; }
        public string? NameOfSons { get; set; }
        public string? NameOfDaughters { get; set; }
        public string? FatherInLaw { get; set; }
        public string? MotherInLaw { get; set; }

        public virtual Client Client { get; set; }
    }
}