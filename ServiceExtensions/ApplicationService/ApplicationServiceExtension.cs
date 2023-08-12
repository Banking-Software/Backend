
using MicroFinance.Helper;
using MicroFinance.Repository.AccountSetup.MainLedger;
using MicroFinance.Repository.ClientSetup;
using MicroFinance.Repository.CompanyProfile;
using MicroFinance.Repository.DepositSetup;
using MicroFinance.Repository.RecordsWithCode;
using MicroFinance.Repository.Reports;
using MicroFinance.Repository.Share;
using MicroFinance.Repository.Transaction;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Services.AccountSetup.MainLedger;
using MicroFinance.Services.ClientSetup;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.DepositSetup;
using MicroFinance.Services.RecordsWithCode;
using MicroFinance.Services.Reports;
using MicroFinance.Services.Share;
using MicroFinance.Services.Transactions;
using MicroFinance.Services.UserManagement;
using MicroFinance.Token;

namespace MicroFinance.ServiceExtensions.ApplicationService
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServiceExtension(this IServiceCollection services)
        {
            services.AddTransient<ISuperAdminRepository, SuperAdminRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<ISuperAdminService, SuperAdminService>();

            services.AddTransient<IMainLedgerRepository, MainLedgerRepository>();
            services.AddTransient<IMainLedgerService, MainLedgerService>();
            services.AddTransient<IClientRepository, ClientRepository>();
            services.AddTransient<IClientService, ClientService>();

            services.AddTransient<IDepositSchemeRepository, DepositSchemeRepository>();
            services.AddTransient<IDepositSchemeService, DepositSchemeService>();

            services.AddTransient<ICompanyProfileRepository, CompanyProfileRepository>();
            services.AddTransient<ICompanyProfileService, CompanyProfileService>();

            services.AddTransient<IRecordsWithCodeRepository, RecordsWithCodeRepository>();
            services.AddTransient<IRecordsWithCodeService, RecordsWithCodeService>();

            services.AddTransient<IDepositAccountTransactionRepository, DepositAccountTransactionRepository>();
            services.AddTransient<IDepositAccountTransactionService, DepositAccountTransactionService>();

            services.AddTransient<IShareAccountTransactionRepository, ShareAccountTransactionRepository>();
            services.AddTransient<IShareAccountTransactionService, ShareAccountTransactionService>();

            services.AddTransient<IShareRepository, ShareRepository>();
            services.AddTransient<IShareService, ShareService>();

            services.AddTransient<ITransactionReportRepository, TransactionReportrepository>();
            services.AddTransient<ITransactionReportService, TransactionReportService>();

            services.AddTransient<INepaliCalendarFormat, NepaliCalendarFormat>();

            
            
            return services;
        }
    }   
}