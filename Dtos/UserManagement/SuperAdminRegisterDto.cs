using System.ComponentModel.DataAnnotations;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{
    public class SuperAdminRegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public FintexRole Role { get; set; }
    }
}