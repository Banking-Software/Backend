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
            System.Console.WriteLine("Connection Strings are: ");
            System.Console.WriteLine("SuperAdmin: "+config.GetConnectionString("SuperAdmin"));
            System.Console.WriteLine("Users: "+config.GetConnectionString("Users"));
            System.Console.WriteLine("SuperAdmin: "+config.GetConnectionString("CompanyOperations"));

            services.AddDbContext<SuperAdminDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("SuperAdmin")));
            

            // For Micro Finance User management 
            services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Users")));
            
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("CompanyOperations")));

            return services;
        }
    }
}