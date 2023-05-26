namespace MicroFinance.Dtos.ClientSetup
{
    public class ClientAddressDto
    {
        public int? Id { get; set; }
        public string PermanentVdcMunicipality { get; set; }
        public string? PermanentVdcMunicipalityNepali { get; set; }
        public string? PermanentToleVillage { get; set; }
        public string? PermanentToleVillageNepali { get; set; }
        public string PermanentWardNumber { get; set; }
        public string? PermanentWardNumberNepali { get; set; }
        public string PermanentDistrict { get; set; }
        public string? PermanentDistrictNepali { get; set; }
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
    }
}