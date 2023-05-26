using MicroFinance.Role;

namespace MicroFinance.Dtos.UserManagement
{  
    public class AssignRoleDto
    {
        public string UserName { get; set; }
        public UserRole Role { get; set; }
    }
}