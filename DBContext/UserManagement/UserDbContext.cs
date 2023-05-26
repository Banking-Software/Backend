using MicroFinance.Models.UserManagement;
using MicroFinance.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.DBContext.UserManagement
{
    public class UserDbContext : IdentityDbContext<User>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            
        }
        // public DbSet<AuthorizedUser> AuthorizedUsers {get; set;}
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
           
            base.OnModelCreating(builder);
            builder.Entity<Employee>()
            .HasIndex(u=> new {u.Email, u.UserName})
            .IsUnique();

            builder.Entity<User>()
            .HasOne(a=>a.Employee)
            .WithOne(a=>a.User)
            .HasForeignKey<User>(u => u.EmployeeId).IsRequired(false);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = UserRole.Marketing.ToString(), NormalizedName = UserRole.Marketing.ToString().ToUpper()},
                new IdentityRole { Name = UserRole.Assistant.ToString(), NormalizedName = UserRole.Assistant.ToString().ToUpper()},
                new IdentityRole { Name = UserRole.SeniorAssistant.ToString(), NormalizedName = UserRole.SeniorAssistant.ToString().ToUpper()},
                new IdentityRole { Name = UserRole.Officer.ToString(), NormalizedName = UserRole.Officer.ToString().ToUpper()});

            // builder.Entity<FinanceRole>()
            // .Property(fr=>fr.Id).ValueGeneratedNever();

            // builder.Entity<User>()
            // .HasOne(u=>u.UserRole)
            // .WithOne(fr=>fr.User)
            // .HasForeignKey<User>(u=>u.Role).IsRequired(false)
            // .OnDelete(DeleteBehavior.ClientSetNull);

            
        }
    }
}