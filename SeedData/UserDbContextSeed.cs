using MicroFinance.DBContext;
// using MicroFinance.DBContext.UserManagement;
using MicroFinance.Models.UserManagement;
using MicroFinance.Role;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.SeedData
{
    public class UserDbContextSeed
    {

        public static async Task SeedSuperAdminRoleAsync(ApplicationDbContext superAdminDbContext, UserManager<User> userManager)
        {
            if (!superAdminDbContext.Users.Any())
            {

                using var transaction = await superAdminDbContext.Database.BeginTransactionAsync();
                try
                {
                    User superAdmin = new User()
                    {
                        IsActive = true,
                        Email = "iconichostnep@gmail.com",
                        UserName = "Fintex",
                        CreatedBy = "Ashish Adhikari",
                        CreatedOn = DateTime.Now
                    };
                    string password = "Fintex@123";
                    var result = await userManager.CreateAsync(superAdmin, password);
                    if (!result.Succeeded) throw new Exception("Not able to Create SuperAdmin");
                    var roleAssign = await userManager.AddToRoleAsync(superAdmin, UserRole.SuperAdmin.ToString());
                    if (!roleAssign.Succeeded) throw new Exception("Role failed to set");
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }

            }
        }
    }
}