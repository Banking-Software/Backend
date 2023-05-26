namespace MicroFinance.Dtos.ClientSetup
{
    public class ClientFamilyDto
    {
        public int? Id { get; set; }
        public string MotherName { get; set; }
        public string? MotherNameNepali { get; set; }
        public string FatherName { get; set; }
        public string? FatherNameNepali { get; set; }
        public string GrandFatherName { get; set; }
        public string? GrandFatherNameNepali { get; set; }
        public string? SpouseName { get; set; }
        public string? SpouseOccupation { get; set; }
        public string? NameOfSons { get; set; }
        public string? NameOfDaughters { get; set; }
        public string? FatherInLaw { get; set; }
        public string? MotherInLaw { get; set; }
    }
}