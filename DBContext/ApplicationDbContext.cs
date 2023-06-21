using System.Reflection;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.CompanyProfile;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            //Start: Client Setup
            modelBuilder.Entity<ClientAccountTypeInfo>()
            .HasIndex(at => at.Type).IsUnique();

            modelBuilder.Entity<ClientTypeInfo>()
            .HasIndex(ct => ct.Type).IsUnique();

            modelBuilder.Entity<ClientShareTypeInfo>()
            .HasIndex(st => st.Type).IsUnique();

            modelBuilder.Entity<ClientKYMTypeInfo>()
            .HasIndex(kt => kt.Type).IsUnique();

            // Flexible Interest Rate
            modelBuilder.Entity<FlexibleInterestRate>()
            .HasOne(fir=>fir.DepositScheme)
            .WithMany(ds=>ds.FlexibleInterestRates)
            .HasForeignKey(fir=>fir.DepositSchemeId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }

        // Start: MainLedger Setup
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<BankSetup> BankSetups { get; set; }
        public DbSet<SubLedger> SubLedgers { get; set; }
        public DbSet<DebitOrCredit> DebitOrCredits { get; set; }
        public DbSet<BankType> BankTypes {get; set;}

        //START: Company Details
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        // START: ClientRegistration
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientKYMTypeInfo> ClientKYMTypeInfos { get; set; }
        public DbSet<ClientInfo> ClientInfos { get; set; }
        public DbSet<ClientAccountTypeInfo> ClientAccountTypeInfos { get; set; }
        public DbSet<ClientTypeInfo> ClientTypeInfos { get; set; }
        public DbSet<ClientContactInfo> ClientContactInfos { get; set; }
        public DbSet<ClientAddressInfo> ClientAddressInfos { get; set; }
        public DbSet<ClientFamilyInfo> ClientFamilyInfos { get; set; }
        public DbSet<ClientNomineeInfo> ClientNomineeInfos { get; set; }
        public DbSet<ClientShareTypeInfo> ClientShareTypeInfos { get; set; }


        // START: Deposit

        public DbSet<DepositScheme> DepositSchemes { get; set; }
        public DbSet<PostingScheme> PostingSchemes { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<FlexibleInterestRate> FlexibleInterestRates { get; set; }

        // // START: TRANSACTION
        // public DbSet<Transaction> Transactions { get; set; }
        // public DbSet<DepositTransaction> DepositTransactions { get; set; }
    
    }
}