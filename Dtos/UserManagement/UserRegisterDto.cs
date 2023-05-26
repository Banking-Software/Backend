using System.ComponentModel.DataAnnotations;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{
    public class UserRegisterDto
    {
        // START: Required Fields
        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; } =false;
        public UserRole Role { get; set; }

        // END: Optional Field
        public double? DepositLimit { get; set; }
        public double? LoanLimit { get; set; }
    }
}