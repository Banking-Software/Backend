using System.Transactions;
using MicroFinance.DBContext;
// using MicroFinance.DBContext.UserManagement;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.UserManagement
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        // private readonly UserDbContext _userDbContext;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository
        (
            ILogger<EmployeeRepository> logger,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            // UserDbContext userDbContext,
            ApplicationDbContext dbContext
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            // _userDbContext = userDbContext;
            _logger = logger;
            _dbContext=dbContext;

        }
        // START: User
        public async Task<SignInResult> Login(User user, string password, bool stayLogin)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, stayLogin);
        }

        public async Task<User> Register(User user, string password, string role)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var userCreate = await _userManager.CreateAsync(user, password);
            string errorDescription = "Not able to Create User";
            string errorCode = "Invalid";
            if (userCreate.Succeeded)
            {
                var roleAssign = await _userManager.AddToRoleAsync(user, role);
                if (!roleAssign.Succeeded)
                {
                    await transaction.RollbackAsync();
                    await DeleteUser(user);
                    errorDescription = roleAssign.Errors.FirstOrDefault()?.Description;
                    errorCode = roleAssign.Errors.FirstOrDefault()?.Code;
                    _logger.LogError($"{DateTime.Now}: {errorCode}. {errorDescription}");
                    throw new NotImplementedExceptionHandler($"{errorCode}. {errorDescription}");
                }
                _logger.LogInformation($"{DateTime.Now}: {user.UserName} is created");
                await transaction.CommitAsync();
                return user;
            }
            await transaction.RollbackAsync();
            errorDescription = userCreate.Errors.FirstOrDefault()?.Description;
            errorCode = userCreate.Errors.FirstOrDefault()?.Code;
            _logger.LogError($"{DateTime.Now}: {errorCode}. {errorDescription}");
            throw new NotImplementedExceptionHandler($"{errorCode}. {errorDescription}");
        }

        public async Task<bool> UpdatePassword(User user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation($"{DateTime.Now}: {user.UserName} 's Password Updated ");
                return true;
            }
            _logger.LogError($"{DateTime.Now}: Password Update Failed for {user.UserName} due to {result.Errors}. {result.Errors.FirstOrDefault()?.Description}");

            return false;
        }

        public async Task<IdentityResult> UpdateUserProfile(User user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            existingUser.DepositLimit = user.DepositLimit;
            existingUser.LoanLimit = user.LoanLimit;
            return await _userManager.UpdateAsync(existingUser);
        }

        public async Task<IdentityResult> ActivateOrDeactivateUser(User user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            existingUser.IsActive = user.IsActive;
            return await _userManager.UpdateAsync(existingUser);
        }
        public async Task<IdentityResult> DeleteUser(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _userManager.Users.Include(u => u.Employee).ToListAsync();
        }

        public async Task<User> GetUserDetailsById(string id)
        {
            return await _userManager.Users
                    .Include(u => u.Employee)
                    .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetUserDetailsByUsername(string userName)
        {
            return await _userManager.Users
                 .Include(u => u.Employee)
                 .FirstOrDefaultAsync(u => u.UserName == userName);

        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<User> GetUserByUsername(string userName)
        {
            //return await _userManager.FindByNameAsync(userName);
            return await _userManager.Users
            .Include(usr => usr.Employee)
            .Where(usr => usr.UserName == userName)
            .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AssignRole(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<string> GetRole(User user)
        {
            return (await _userManager.GetRolesAsync(user))[0];
        }

        // END
        // START: Employee
        public async Task<int> CreateEmployee(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            return await _dbContext.SaveChangesAsync();
        }
        // Admin Task or User Task
        public async Task<int> EditEmployeeProfile(Employee updateEmployee, string oldEmail)
        {
            string newEmail = updateEmployee.Email;
            var existingEmployee = await _dbContext.Employees.FindAsync(updateEmployee.Id);
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _dbContext.Entry(existingEmployee).State = EntityState.Detached;
                    _dbContext.Employees.Attach(updateEmployee);
                    _dbContext.Entry(updateEmployee).State = EntityState.Modified;
                    if (oldEmail != newEmail)
                    {
                        var existingUserWithOldEmail = await _userManager.FindByEmailAsync(oldEmail);
                        if (existingUserWithOldEmail != null)
                        {
                            existingUserWithOldEmail.Email = newEmail;
                            existingUserWithOldEmail.ModifiedBy = updateEmployee.ModifiedBy;
                            existingUserWithOldEmail.ModifiedOn = DateTime.Now;
                            await _userManager.UpdateAsync(existingUserWithOldEmail);
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    transactionScope.Complete();
                    return 1;
                }
                catch (Exception)
                {
                    transactionScope.Dispose();
                    return 0;
                }
            }
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _dbContext.Employees.ToListAsync();
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            var employee = await _dbContext.Employees
                    .FirstOrDefaultAsync(u => u.Email == email);

            return employee;
        }

        public async Task<int> DeleteEmployee(Employee employee)
        {
            _dbContext.Employees.Remove(employee);
            return await _dbContext.SaveChangesAsync();
        }
        //END: Employee
    }
}