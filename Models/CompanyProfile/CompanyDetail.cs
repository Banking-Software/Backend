using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroFinance.Enums;

namespace MicroFinance.Models.CompanyProfile
{
    public class CompanyDetail
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
        public DateTime CompanyValidityStartDate { get; set; } = DateTime.Now;
        public DateTime CompanyValidityEndDate { get; set; } = DateTime.Now.AddYears(1);
        public byte[]? LogoFileData { get; set; }
        public string? LogoFileName { get; set; }
        public FileType? LogoFileType { get; set; }
        public decimal CurrentTax { get; set; }=0;
        public string CurrentFiscalYear { get; set; } = "8081";
        // LOGO
    }
}