using System.ComponentModel.DataAnnotations;

namespace MicroFinance.Dtos.UserManagement
{
    public class SuperAdminUpdatePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}