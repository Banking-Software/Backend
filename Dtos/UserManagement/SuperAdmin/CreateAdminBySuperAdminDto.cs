using System.ComponentModel.DataAnnotations;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{
    public class CreateAdminBySuperAdminDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string BranchCode {get;set;}
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}