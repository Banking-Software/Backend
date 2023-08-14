using MicroFinance.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.UserManagement;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne(usr=>usr.Role)
        .WithMany(rol=>rol.Users)
        .HasForeignKey(usr=>usr.RoleId)
        .IsRequired(true)
        .OnDelete(DeleteBehavior.ClientCascade);

        
        builder.HasOne(a=>a.Employee).WithOne(a=>a.User)
        .HasForeignKey<User>(u => u.EmployeeId)
        .IsRequired(false)
        .OnDelete(DeleteBehavior.ClientSetNull);
        
    }
}