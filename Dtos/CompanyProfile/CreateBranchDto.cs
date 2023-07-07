using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.CompanyProfile
{
    public class CreateBranchDto
    {
        [Required]
        public string BranchCode { get; set; }
        public string? BranchName { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}