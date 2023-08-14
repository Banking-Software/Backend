using System.Reflection;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.CompanyProfile;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.DepositSetup.HelperTable;
using MicroFinance.Models.RecordsWithCode;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.UserManagement;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.DBContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private readonly ILoggerFactory _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // modelBuilder.Entity<IdentityRole>().HasData(
            //     new IdentityRole { Name = UserRole.Marketing.ToString(), NormalizedName = UserRole.Marketing.ToString().ToUpper()},
            //     new IdentityRole { Name = UserRole.Assistant.ToString(), NormalizedName = UserRole.Assistant.ToString().ToUpper()},
            //     new IdentityRole { Name = UserRole.SeniorAssistant.ToString(), NormalizedName = UserRole.SeniorAssistant.ToString().ToUpper()},
            //     new IdentityRole { Name = UserRole.Officer.ToString(), NormalizedName = UserRole.Officer.ToString().ToUpper()},
            //     new IdentityRole {Name = UserRole.SuperAdmin.ToString(), NormalizedName = UserRole.SuperAdmin.ToString().ToUpper()});
            //Start: Client Setup

            modelBuilder.Entity<ClientGroup>()
            .HasIndex(g => g.Code).IsUnique();

            modelBuilder.Entity<ClientUnit>()
            .HasIndex(u => u.Code).IsUnique();

            // Flexible Interest Rate
            modelBuilder.Entity<FlexibleInterestRate>()
            .HasOne(fir => fir.DepositScheme)
            .WithMany(ds => ds.FlexibleInterestRates)
            .HasForeignKey(fir => fir.DepositSchemeId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // Start: MainLedger Setup
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<BankSetup> BankSetups { get; set; }
        public DbSet<SubLedger> SubLedgers { get; set; }
        public DbSet<DebitOrCredit> DebitOrCredits { get; set; }
        public DbSet<BankType> BankTypes { get; set; }

        //START: Company Details
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CompanyDetail> CompanyDetails { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public int MyProperty { get; set; }

        // START: RecordsWithCode
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<State> States { get; set; }
        // START: ClientRegistration
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientKYMType> ClientKYMTypes { get; set; }
        public DbSet<ClientType> ClientTypes { get; set; }
        public DbSet<ClientGroup> ClientGroups { get; set; }
        public DbSet<ClientUnit> ClientUnits { get; set; }


        // START: SHARE
        public DbSet<ShareAccount> ShareAccounts { get; set; }
        public DbSet<ShareKitta> ShareKittas { get; set; }
        // START: Deposit

        public DbSet<DepositScheme> DepositSchemes { get; set; }
        public DbSet<DepositPostingScheme> DepositPostingSchemes { get; set; }
        public DbSet<DepositAccountStatus> DepositAccountStatuses { get; set; }
        public DbSet<DepositAccountType> DepositAccountTypes { get; set; }
        public DbSet<DepositSchemeCalculationType> DepositSchemeCalculationTypes { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<JointAccount> JointAccounts { get; set; }
        public DbSet<FlexibleInterestRate> FlexibleInterestRates { get; set; }

        // // START: TRANSACTION
        public DbSet<BaseTransaction> Transactions { get; set; }
        public DbSet<DepositAccountTransaction> DepositAccountTransactions { get; set; }
        public DbSet<LedgerTransaction> LedgerTransactions { get; set; }
        public DbSet<SubLedgerTransaction> SubLedgerTransactions { get; set; }
        public DbSet<ShareTransaction> ShareTransactions { get; set; }

    }
}