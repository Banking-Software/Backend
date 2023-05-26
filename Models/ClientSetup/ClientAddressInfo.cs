using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFinance.Models.ClientSetup
{
    public class ClientAddressInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string PermanentVdcMunicipality { get; set; }
        public string? PermanentVdcMunicipalityNepali { get; set; }
        public string? PermanentToleVillage { get; set; }
        public string? PermanentToleVillageNepali { get; set; }
        [Required]
        public string PermanentWardNumber { get; set; }
        public string? PermanentWardNumberNepali { get; set; }
        [Required]
        public string PermanentDistrict { get; set; }
        public string? PermanentDistrictNepali { get; set; }
        [Required]
        public string PermanentState { get; set; }

        public string? TemporaryVdcMunicipality { get; set; }
        public string? TemporaryVdcMunicipalityNepali { get; set; }
        public string? TemporaryToleVillage { get; set; }
        public string? TemporaryToleVillageNepali { get; set; }
        public string? TemporaryWardNumber { get; set; }
        public string? TemporaryWardNumberNepali { get; set; }
        public string? TemporaryDistrict { get; set; }
        public string? TemporaryDistrictNepali { get; set; }
        public string? TemporaryState { get; set; }

        public virtual Client Client { get; set; }

    }
}