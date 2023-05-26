using MicroFinance.DBContext.UserManagement;
using MicroFinance.Models.UserManagement;
using MicroFinance.Role;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.SeedData
{
    public class UserDbContextSeed
    {
       
        public static async Task SeedSuperAdminRoleAsync(SuperAdminDbContext superAdminDbContext, UserManager<SuperAdmin> userManager)
        {
            if(!superAdminDbContext.Users.Any()){

            using var transaction = await superAdminDbContext.Database.BeginTransactionAsync();
            SuperAdmin superAdmin = new SuperAdmin()
            {
                IsActive=true,
                Email="iconichostnep@gmail.com",
                UserName = "Fintex",
                Name = "Fintex"
            };
            string password = "Fintex@123";
            var result = await userManager.CreateAsync(superAdmin, password);
            if(result.Succeeded)
            {
                var roleAssign = await userManager.AddToRoleAsync(superAdmin, FintexRole.SuperAdmin.ToString());
                if(!roleAssign.Succeeded)
                {
                    await transaction.RollbackAsync();
                    await userManager.DeleteAsync(superAdmin);
                }
                await transaction.CommitAsync();
            }
            }
        }
    }
}