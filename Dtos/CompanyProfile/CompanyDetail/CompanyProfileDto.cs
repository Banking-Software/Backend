using MicroFinance.Enums;

namespace MicroFinance.Dtos.CompanyProfile
{
    public class CompanyProfileDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string? CompanyNameNepali { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyAddressNepali { get; set; }
        public string? PANNo { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string? PhoneNo { get; set; }
        public string? CompanyEmailAddress { get; set; }
        public DateTime CompanyValidityStartDate { get; set; }
        public DateTime CompanyValidityEndDate { get; set; }
        public string? LogoFileData { get; set; }
        public string? LogoFileName { get; set; }
        public FileType? LogoFileType { get; set; }
        public decimal CurrentTax { get; set; }
    }
}