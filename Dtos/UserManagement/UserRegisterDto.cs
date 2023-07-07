using System.ComponentModel.DataAnnotations;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{
    public class UserRegisterDto
    {
        // START: Required Fields
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsActive { get; set; } =false;
        [Required]
        public UserRole Role { get; set; }

        // END: Optional Field
        public double? DepositLimit { get; set; }
        public double? LoanLimit { get; set; }
    }
}