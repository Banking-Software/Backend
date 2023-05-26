using MicroFinance.DBContext;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.SeedData
{
    public class LedgerDbContextSeed
    {
        private readonly ILogger<LedgerDbContextSeed> _logger;

        public LedgerDbContextSeed(ILogger<LedgerDbContextSeed> logger)
        {
            _logger = logger;
        }
        private static async Task<int> SeedAccountTypeAsync(ApplicationDbContext context)
        {
            if (!context.AccountTypes.Any())
            {
                await context.AccountTypes.AddRangeAsync(new List<AccountType>
                {
                        new AccountType {Id=1, Name="Assets"},
                        new AccountType {Id=2, Name="Expense"},
                        new AccountType {Id=3, Name="Income"},
                        new AccountType {Id=4, Name="Liability"}
                });
                var result = await context.SaveChangesAsync();
                return result;

            }
            return 0;
        }

        public static async Task SeedGroupTypeAsync(ApplicationDbContext context)
        {

            if (!context.GroupTypes.Any() && !context.AccountTypes.Any())
            {
                int accountStatus = await SeedAccountTypeAsync(context);
                if (accountStatus >= 1)
                {
                    var assets = await context.AccountTypes.FindAsync(1);
                    var expense = await context.AccountTypes.FindAsync(2);
                    var income = await context.AccountTypes.FindAsync(3);
                    var liability = await context.AccountTypes.FindAsync(4);
                    if (assets != null && expense != null && income != null && liability != null)
                    {
                        await context.GroupTypes.AddRangeAsync(new List<GroupType>{
                            new GroupType {Name="Cash", EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Bank", EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Advance Amount (Receivable)", EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Fixed Assets", EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Loan Investment", EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Remittance", EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Siling Fine", EntryDate=DateTime.Now, AccountType=assets},

                            new GroupType {Name="Interest Expenses", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="Other Expenses", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="Adminstrative Expenses", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="Miscellaneous Expense", EntryDate=DateTime.Now, AccountType=expense},

                            new GroupType {Name="Other Income", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Loan Interest Earning", EntryDate=DateTime.Now, AccountType=income},

                            new GroupType {Name="Other Liabilities", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Funds", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Saving and Deposit", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Share Capital", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Loan Security Fund", EntryDate=DateTime.Now, AccountType=liability},
                        });
                        await context.SaveChangesAsync();
                    }
                }

            }

        }
    }

}
