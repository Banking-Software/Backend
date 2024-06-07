using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.CompanyProfile
{
    public class UpdateBranchDto
    {
        [Required]
        public int Id { get; set; }
        public string? BranchName { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}