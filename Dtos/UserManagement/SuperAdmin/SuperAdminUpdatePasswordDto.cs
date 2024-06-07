using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.UserManagement
{
    public class SuperAdminUpdatePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}