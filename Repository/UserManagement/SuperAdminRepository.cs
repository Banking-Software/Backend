using MicroFinance.DBContext.UserManagement;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.UserManagement
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        private readonly UserManager<SuperAdmin> _userManager;
        private readonly SignInManager<SuperAdmin> _signInManager;
        private readonly UserManager<User> _userManagerFinance;
        private readonly ILogger<SuperAdminRepository> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SuperAdminDbContext _superAdminDbContext;

        public SuperAdminRepository
        (
            UserManager<SuperAdmin> userManager,
            SuperAdminDbContext superAdminDbContext,
            SignInManager<SuperAdmin> signInManager,
            UserManager<User> userManagerFinance,
            RoleManager<IdentityRole> roleManager,
            ILogger<SuperAdminRepository> logger
        )
        {
            _userManager = userManager;
            _signInManager =signInManager;
            _userManagerFinance=userManagerFinance;
            _logger = logger;
            _roleManager=roleManager;
            _superAdminDbContext=superAdminDbContext;

        }
        // Related to SuperAdmin Profile
        public async Task<SignInResult> Login(SuperAdmin superAdmin, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(superAdmin, password, false);
            return result;
        }
        public async Task<SuperAdmin> Register(SuperAdmin superAdmin, string password, string role)
        {
            using var transaction = await _superAdminDbContext.Database.BeginTransactionAsync();
            var result = await _userManager.CreateAsync(superAdmin, password);
            if(result.Succeeded)
            {
                var roleAssign = await _userManager.AddToRoleAsync(superAdmin, role);
                if(!roleAssign.Succeeded)
                {
                    await transaction.RollbackAsync();
                    await DeleteSuperAdmin(superAdmin);
                    _logger.LogError($"{DateTime.Now}: Failed to Create Super Admin  due to {roleAssign.Errors}");
                    throw new NotImplementedExceptionHandler($"{DateTime.Now}: {roleAssign.Errors}");
                }
                await transaction.CommitAsync();
                _logger.LogInformation($"{DateTime.Now}: {superAdmin.UserName} created as SuperAdmin");
                return superAdmin;
            }
            _logger.LogError($"{DateTime.Now}: Failed to Create Super Admin  due to {result.Errors}");
            throw new NotImplementedExceptionHandler($"{DateTime.Now}: Unable to Create SuperAdmin");
        }

        public async Task<IdentityResult> DeleteSuperAdmin(SuperAdmin superAdmin)
        {
            return await _userManager.DeleteAsync(superAdmin);
        }
        public async Task<SuperAdmin> GetSuperAdminByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
         public async Task<SuperAdmin> GetSuperAdminById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<string> GetRole(SuperAdmin user)
        {
            return (await _userManager.GetRolesAsync(user))[0];
        }

        public async Task<bool> UpdatePassword(SuperAdmin superAdmin, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(superAdmin, oldPassword,newPassword);
            if (result.Succeeded)
            { 
                _logger.LogInformation($"{DateTime.Now} (SuperAdmin: UpdatePassword) Password Updated: {superAdmin.UserName}");
                return true;
            }
            _logger.LogError($"{DateTime.Now} (SuperAdmin: UpdatePassword) Password Updated failed: {superAdmin.UserName}, Error: {result.Errors}");
            return false;
        }
        public async Task<IdentityResult> EditProfile(SuperAdmin superAdmin)
        {
            return await _userManager.UpdateAsync(superAdmin);
        }


        // Related to SuperAdmin Functionalities
        public async Task<IdentityResult> CreateAdminProfile(User user, string password)
        {
            var result = await _userManagerFinance.CreateAsync(user, password);
            return result;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _userManagerFinance.Users.ToListAsync();
        }

       
    }
}