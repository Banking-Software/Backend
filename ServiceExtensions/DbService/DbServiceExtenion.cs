using MicroFinance.DBContext;
// using MicroFinance.DBContext.CompanyOperations;
// using MicroFinance.DBContext.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.ServiceExtensions.DbService
{
    public static class DbServiceExtension
    {
        public static IServiceCollection AddDbServiceExtension(this IServiceCollection services, IConfiguration config)
        {
            System.Console.WriteLine("DbConnection: "+config.GetConnectionString("DbConnection"));            
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DbConnection")));
            return services;
        }
    }
}