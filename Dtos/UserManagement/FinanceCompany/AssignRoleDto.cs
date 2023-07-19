using System.ComponentModel.DataAnnotations;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{  
    public class AssignRoleDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public UserRole Role { get; set; }
    }
}