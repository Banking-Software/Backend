using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Models.CompanyProfile;
// using MicroFinance.DBContext.UserManagement;
using MicroFinance.Models.UserManagement;
using MicroFinance.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.SeedData
{
    public class UserDbContextSeed
    {
        private static async Task<int> SeedRoleAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.UserRoles.Any())
            {
                List<UserRole> UserRoles = new()
                {
                    new UserRole(){Name=RoleEnum.Marketing.ToString(),RoleCode = (int) RoleEnum.Marketing},
                    new UserRole(){Name=RoleEnum.Assistant.ToString(),RoleCode = (int) RoleEnum.Assistant},
                    new UserRole(){Name=RoleEnum.SeniorAssistant.ToString(),RoleCode = (int) RoleEnum.SeniorAssistant},
                    new UserRole(){Name=RoleEnum.Officer.ToString(),RoleCode = (int) RoleEnum.Officer},
                    new UserRole(){Name=RoleEnum.SuperAdmin.ToString(),RoleCode = (int) RoleEnum.SuperAdmin},
                };
                await dbContext.UserRoles.AddRangeAsync(UserRoles);
                return await dbContext.SaveChangesAsync();
            }
            return 1;
        }

        public static async Task SeedSuperAdminAndRoleAsync(ApplicationDbContext superAdminDbContext, UserManager<User> userManager)
        {
            int UserRoleseed = await SeedRoleAsync(superAdminDbContext);
            if (UserRoleseed >= 1)
            {
                await SeedSuperAdminRoleAsync(superAdminDbContext, userManager);
            }
        }

        private static async Task SeedSuperAdminRoleAsync(ApplicationDbContext superAdminDbContext, UserManager<User> userManager)
        {

            if (!superAdminDbContext.Users.Any())
            {
                User superAdmin = new User()
                {
                    IsActive = true,
                    Email = "iconichostnep@gmail.com",
                    UserName = "Fintex",
                    CreatedBy = "AshishAdhikari",
                    CreatedOn = DateTime.Now,
                    Role = await superAdminDbContext.UserRoles.Where(rl=>rl.RoleCode==(int) RoleEnum.SuperAdmin).SingleOrDefaultAsync()
                };
                string password = "Fintex@123";
                var result = await userManager.CreateAsync(superAdmin, password);
                if (!result.Succeeded) 
                {
                    throw new Exception("Not able to Create SuperAdmin");
                }
            }
        }
    }
}