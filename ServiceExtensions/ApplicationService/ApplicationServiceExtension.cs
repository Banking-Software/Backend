
using MicroFinance.Repository.AccountSetup.MainLedger;
using MicroFinance.Repository.ClientSetup;
using MicroFinance.Repository.CompanyProfile;
using MicroFinance.Repository.DepositSetup;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Services.AccountSetup.MainLedger;
using MicroFinance.Services.ClientSetup;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.DepositSetup;
using MicroFinance.Services.UserManagement;
using MicroFinance.Token;

namespace MicroFinance.ServiceExtensions.ApplicationService
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServiceExtension(this IServiceCollection services)
        {
            services.AddScoped<ISuperAdminRepository, SuperAdminRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ISuperAdminService, SuperAdminService>();
            services.AddScoped<IMainLedgerRepository, MainLedgerRepository>();
            services.AddScoped<IMainLedgerService, MainLedgerService>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, ClientService>();

            services.AddScoped<IDepositSchemeRepository, DepositSchemeRepository>();
            services.AddScoped<IDepositSchemeService, DepositSchemeService>();

            services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
            services.AddScoped<ICompanyProfileService, CompanyProfileService>();
            
            return services;
        }
    }   
}