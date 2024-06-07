using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Repository.UserManagement
{
    public interface ISuperAdminRepository
    {
        // Related to SuperAdmin Profile
        // Task<SuperAdmin> Register(SuperAdmin superAdmin, string password, string role);
        // Task<User> Register(SuperAdmin superAdmin, string password, string role);
        // Task<SignInResult> Login(SuperAdmin superAdmin, string password);
        Task<SignInResult> Login(User superAdmin, string password);
        // Task<string> GetRole(SuperAdmin user);
        Task<UserRole> GetRole(int roleCode);
        // Task<IdentityResult> DeleteSuperAdmin(SuperAdmin superAdmin);
        // Task<SuperAdmin> GetSuperAdminByUserName(string userName);
        Task<User> GetSuperAdminByUserName(string userName);
        // Task<SuperAdmin> GetSuperAdminById(string id);
        Task<User> GetSuperAdminById(string id);
        // Task<IdentityResult> EditProfile(SuperAdmin superAdmin);
        // Task<IdentityResult> EditProfile(User superAdmin);
        // Task<bool> UpdatePassword(SuperAdmin superAdmin, string oldPassword, string newPassword);
        Task<bool> UpdatePassword(User superAdmin, string oldPassword, string newPassword);
        //Related to Functionality of SuperAdmin
        Task CreateAdminProfile(User user, Employee employee ,string password);
        Task<List<User>> GetAllUsers();
    }
}