using MicroFinance.DBContext;
// using MicroFinance.DBContext.CompanyOperations;
using MicroFinance.DBContext.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.ServiceExtensions.DbService
{
    public static class DbServiceExtension
    {
        public static IServiceCollection AddDbServiceExtension(this IServiceCollection services, IConfiguration config)
        {
            // For Super Admin User Management
            services.AddDbContext<SuperAdminDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("SuperAdmin")));

            // For Micro Finance User management 
            services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Users")));
            
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("CompanyOperations")));
            // For Main Ledger Setup
            // services.AddDbContext<LedgerDbContext>(options =>
            // options.UseSqlServer(config.GetConnectionString("CompanyOperations")));
            
            // // For Client Setup
            // services.AddDbContext<ClientDbContext>(options =>
            // options.UseSqlServer(config.GetConnectionString("CompanyOperations")));

            //  // For Deposit Setup
            // services.AddDbContext<DepositDbContext>(options =>
            // options.UseSqlServer(config.GetConnectionString("CompanyOperations")));

            return services;
        }
    }
}