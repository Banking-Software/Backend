// using MicroFinance.Models.UserManagement;
// using MicroFinance.Role;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;

// namespace MicroFinance.DBContext.UserManagement
// {
//     public class SuperAdminDbContext : IdentityDbContext<SuperAdmin>
//     {
//         public SuperAdminDbContext(DbContextOptions<SuperAdminDbContext> options) : base (options)
//         {
            
//         }
//         protected override void OnModelCreating(ModelBuilder builder)
//         {
//             base.OnModelCreating(builder);

//             builder.Entity<IdentityRole>().HasData(
//                 new IdentityRole { Name = FintexRole.SuperAdmin.ToString(), NormalizedName = FintexRole.SuperAdmin.ToString().ToUpper()});
//         }

//     }
// }