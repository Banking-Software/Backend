using MicroFinance.Models.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.UserManagement;

public class RoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(rol=>rol.Id);
        builder.HasIndex(rol=>rol.Name).IsUnique();
        builder.Property(rol=>rol.Name).IsRequired(true);
        builder.HasIndex(rol=>rol.RoleCode).IsUnique();
        builder.Property(rol=>rol.RoleCode).IsRequired(true);
    }
}