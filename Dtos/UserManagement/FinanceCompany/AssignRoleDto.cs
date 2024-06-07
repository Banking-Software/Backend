using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums;
using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{  
    public class AssignRoleDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public RoleEnum Role { get; set; }
    }
}