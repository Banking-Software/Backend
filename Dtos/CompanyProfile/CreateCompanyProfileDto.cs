using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.CompanyProfile  
{
    public class CreateCompanyProfileDto
    {
        [Required]
        public string CompanyName { get; set; }
        public string? CompanyNameNepali { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyAddressNepali { get; set; }
        public string? PANNo { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string? PhoneNo { get; set; }
        public string? CompanyEmailAddress { get; set; }
        public DateTime? FromDate { get; set; }
    }
}