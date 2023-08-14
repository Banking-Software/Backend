// using MicroFinance.DBContext.UserManagement;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.UserManagement
{
    public class SuperAdminRepository : ISuperAdminRepository
    {
        // private readonly UserManager<SuperAdmin> _userManager;
        // private readonly SignInManager<SuperAdmin> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        // private readonly UserManager<User> _userManagerFinance;
        private readonly ILogger<SuperAdminRepository> _logger;
        private readonly ApplicationDbContext _superAdminDbContex;

        // private readonly SuperAdminDbContext _superAdminDbContext;

        public SuperAdminRepository
        (
            // UserManager<SuperAdmin> userManager,
            UserManager<User> userManager,
            // SuperAdminDbContext superAdminDbContext,
            ApplicationDbContext superAdminDbContex,
            // SignInManager<SuperAdmin> signInManager,
            SignInManager<User> signInManager,
            // UserManager<User> userManagerFinance,
            ILogger<SuperAdminRepository> logger
        )
        {
            _userManager = userManager;
            _signInManager =signInManager;
            // _userManagerFinance=userManagerFinance;
            _logger = logger;
            // _superAdminDbContext=superAdminDbContext;
            _superAdminDbContex=superAdminDbContex;

        }
        // Related to SuperAdmin Profile
        // public async Task<SignInResult> Login(SuperAdmin superAdmin, string password)
        public async Task<SignInResult> Login(User superAdmin, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(superAdmin, password, false);
            return result;
        }
        // public async Task<SuperAdmin> Register(SuperAdmin superAdmin, string password, string role)
        // {
        //     using var transaction = await _superAdminDbContext.Database.BeginTransactionAsync();
        //     var result = await _userManager.CreateAsync(superAdmin, password);
        //     if(result.Succeeded)
        //     {
        //         var roleAssign = await _userManager.AddToRoleAsync(superAdmin, role);
        //         if(!roleAssign.Succeeded)
        //         {
        //             await transaction.RollbackAsync();
        //             await DeleteSuperAdmin(superAdmin);
        //             _logger.LogError($"{DateTime.Now}: Failed to Create Super Admin  due to {roleAssign.Errors}");
        //             throw new NotImplementedExceptionHandler($"{DateTime.Now}: {roleAssign.Errors}");
        //         }
        //         await transaction.CommitAsync();
        //         _logger.LogInformation($"{DateTime.Now}: {superAdmin.UserName} created as SuperAdmin");
        //         return superAdmin;
        //     }
        //     _logger.LogError($"{DateTime.Now}: Failed to Create Super Admin  due to {result.Errors}");
        //     throw new NotImplementedExceptionHandler($"{DateTime.Now}: Unable to Create SuperAdmin");
        // }

        // public async Task<IdentityResult> DeleteSuperAdmin(SuperAdmin superAdmin)
        // {
        //     return await _userManager.DeleteAsync(superAdmin);
        // }
        // public async Task<SuperAdmin> GetSuperAdminByUserName(string userName)
        public async Task<User> GetSuperAdminByUserName(string userName)
        {
            return await _userManager.Users.Include(usr=>usr.Role).Where(usr=>usr.UserName==userName).SingleOrDefaultAsync();
        }
        // public async Task<SuperAdmin> GetSuperAdminById(string id)
        public async Task<User> GetSuperAdminById(string id)
        {
            return await _userManager.Users.Include(usr=>usr.Role).Where(usr=>usr.Id==id).SingleOrDefaultAsync();
        }

        // public async Task<string> GetRole(SuperAdmin user)
        public async Task<UserRole> GetRole(int roleCode)
        {
            var role = await _superAdminDbContex.UserRoles.Where(rl=>rl.RoleCode==roleCode).SingleOrDefaultAsync();
            return role;
        }

        // public async Task<bool> UpdatePassword(SuperAdmin superAdmin, string oldPassword, string newPassword)
        public async Task<bool> UpdatePassword(User superAdmin, string oldPassword, string newPassword)
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
        // public async Task<IdentityResult> EditProfile(SuperAdmin superAdmin)
        // {
        //     return await _userManager.UpdateAsync(superAdmin);
        // }


        // Related to SuperAdmin Functionalities
        public async Task CreateAdminProfile(User user, Employee employee ,string password)
        {
            using var transaction = await _superAdminDbContex.Database.BeginTransactionAsync();
            try
            {
                await _superAdminDbContex.Employees.AddAsync(employee);
                int employeeCreateStatus = await _superAdminDbContex.SaveChangesAsync();
                if(employeeCreateStatus<1) throw new Exception("Failed to create employee profile");
                user.Employee = employee;
                user.Role = await _superAdminDbContex.UserRoles.Where(rl=>rl.RoleCode==(int) RoleEnum.Officer).SingleOrDefaultAsync();
                var userResult = await _userManager.CreateAsync(user, password);
                if(!userResult.Succeeded)
                {
                    throw new Exception(userResult.Errors?.FirstOrDefault()?.Description);
                }
                await transaction.CommitAsync();
                return;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _userManager.Users
            .Include(usr=>usr.Role)
            .Where(usr=>usr.Role.RoleCode!=(int) RoleEnum.SuperAdmin).ToListAsync();
        }       
    }
}