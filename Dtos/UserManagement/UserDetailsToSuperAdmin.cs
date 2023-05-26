namespace MicroFinance.Dtos.UserManagement
{
    public class UserDetailsToSuperAdmin
    {
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
    }
}