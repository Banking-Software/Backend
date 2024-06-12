namespace MicroFinance.Models.UserManagement;

public class UserRole
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RoleCode { get; set; }
    public virtual ICollection<User> Users { get; set; }
}