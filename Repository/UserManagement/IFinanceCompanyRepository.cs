using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;

namespace MicroFinance.Repository.UserManagement
{
    public interface IEmployeeRepository
    {
        // START: User
        Task<User> Register(User user, string password, string role);
        Task<SignInResult> Login(User user, string password, bool stayLogin);
        Task<bool> UpdatePassword(User user, string oldPassword, string newPassword);
        Task<IdentityResult> UpdateUserProfile(User user);
        Task<User> GetUserDetailsByUsername(string userName);
        Task<User> GetUserByUsername(string userName);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserDetailsById(string id);

        Task<User> GetUserById(string id);
        Task<List<User>> GetUsers();
        Task<IdentityResult> AssignRole(User user, string role);
        Task<string> GetRole(User user);
        Task<IdentityResult> DeleteUser(User user);
        Task<IdentityResult> ActivateOrDeactivateUser(User user);
        // END 

        // START: Employee
        Task<int> CreateEmployee(Employee employee);
        Task<int> EditEmployeeProfile(Employee updateEmployee, string oldEmail);
        Task<Employee> GetEmployeeByEmail(string email);
        Task<Employee> GetEmployeeById(int id);
        Task<List<Employee>> GetEmployees();
        Task<int> DeleteEmployee(Employee employee);
    }
}