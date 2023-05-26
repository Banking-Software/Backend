using System.ComponentModel.DataAnnotations;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{
    public class CreateAdminBySuperAdminDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
    }
}