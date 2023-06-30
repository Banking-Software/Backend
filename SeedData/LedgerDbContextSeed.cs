using MicroFinance.DBContext;
using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;

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
            return 1;
        }

        private static async Task<int> SeedDebitOrCreditData(ApplicationDbContext context)
        {
            if (!context.DebitOrCredits.Any())
            {
                await context.DebitOrCredits.AddRangeAsync(new List<DebitOrCredit>
                {
                    new DebitOrCredit {Id=1,Name="Debit"},
                    new DebitOrCredit {Id=2,Name="Credit"}
                });
                var result = await context.SaveChangesAsync();
                return result;
            }
            return 1;
        }

        private static async Task<int> SeedBankTypeData(ApplicationDbContext context)
        {
            if (!context.BankTypes.Any())
            {
                await context.BankTypes.AddRangeAsync(new List<BankType>
                {
                    new BankType {Name="Commercial"},
                    new BankType {Name="Development"},
                    new BankType {Name="Finance"}
                });
                var result = await context.SaveChangesAsync();
                return result;
            }
            return 1;
        }

        private static async Task<int> SeedSubLedgerData(ApplicationDbContext context)
        {
            if (!context.SubLedgers.Any())
            {
                var advancedLedger = await context.Ledgers.FindAsync(4);
                var taxPayableLedger = await context.Ledgers.FindAsync(29);
                var goodPurchaseLedger = await context.Ledgers.FindAsync(62);
                var dhuwaniJyalaLedger = await context.Ledgers.FindAsync(63);
                var salaryStaffLedger = await context.Ledgers.FindAsync(64);
                var rentExpLedger = await context.Ledgers.FindAsync(65);
                var statExpenseLedger = await context.Ledgers.FindAsync(66);
                var repairExpenseLedger = await context.Ledgers.FindAsync(67);
                var miscellaneousExpenseLedger = await context.Ledgers.FindAsync(69);
                var depreciationExpenseLedger = await context.Ledgers.FindAsync(70);
                var fuelExpenseLedger = await context.Ledgers.FindAsync(71);
                var rebateExpenseLedger = await context.Ledgers.FindAsync(74);

                await context.SubLedgers.AddRangeAsync(new List<SubLedger>
                {
                    // Advanced 120.2
                    new SubLedger {Id=1, Ledger=advancedLedger ,Name="Advanced Income Tax"},
                    new SubLedger {Id=2, Ledger=advancedLedger ,Name="Advanced Rent"},
                    new SubLedger {Id=3, Ledger=advancedLedger ,Name="Advanced Tax"},
                    new SubLedger {Id=4, Ledger=advancedLedger ,Name="Other Receivable"},
                    new SubLedger {Id=5, Ledger=advancedLedger ,Name="Staff Advanced"},
                    new SubLedger {Id=6, Ledger=advancedLedger ,Name="Sundry ters"},
                    new SubLedger {Id=7, Ledger=advancedLedger ,Name="Telephone Deposit"},
                    new SubLedger {Id=8, Ledger=advancedLedger ,Name="Room Rent Advanced"},
                    //END

                    // TAX PAYABLE
                    new SubLedger {Id=9, Ledger=taxPayableLedger ,Name="Deposit Interest Tax Payable"},
                    new SubLedger {Id=10, Ledger=taxPayableLedger ,Name="Dividend Tax Payable"},
                    new SubLedger {Id=11, Ledger=taxPayableLedger ,Name="House Rent Tax Payable"},
                    new SubLedger {Id=12, Ledger=taxPayableLedger ,Name="Income Tax Payable"},
                    new SubLedger {Id=13, Ledger=taxPayableLedger ,Name="Staff Salary Tax Payable"},
                    //END

                    // Good Purchase
                    new SubLedger {Id=14, Ledger=goodPurchaseLedger ,Name="Saman Kharid 150.1"},
                    //END
                    // Dhuwani//
                    new SubLedger {Id=15, Ledger=dhuwaniJyalaLedger ,Name="Wages Fair Expenses 150.2"},
                    //END
                    // Salary staff
                    new SubLedger {Id=16, Ledger=salaryStaffLedger ,Name="Over Duty (Staff) 150.3"},
                    new SubLedger {Id=17, Ledger=salaryStaffLedger ,Name="Salary (Staff) 150.3"},
                    new SubLedger {Id=18, Ledger=salaryStaffLedger ,Name="Sanchay Kosh 150.03"},
                    new SubLedger {Id=19, Ledger=salaryStaffLedger ,Name="Staff Allowance Payment 150.3"},
                    new SubLedger {Id=20, Ledger=salaryStaffLedger ,Name="Transport & Daily Allowance 150.3"},
                    //END
                    // RENT EXP
                    new SubLedger {Id=21, Ledger=rentExpLedger ,Name="Bonus Expense 150.4"},
                    new SubLedger {Id=22, Ledger=rentExpLedger ,Name="Meeting Expense 150.4"},
                    new SubLedger {Id=23, Ledger=rentExpLedger ,Name="Room Rent Expense 150.4"},
                    //END
                    // Stationary Exp
                    new SubLedger {Id=24, Ledger=statExpenseLedger ,Name="Photocopy Expense 150.5"},
                    new SubLedger {Id=25, Ledger=statExpenseLedger ,Name="Printing and Stationary"},
                    new SubLedger {Id=26, Ledger=statExpenseLedger ,Name="Stationary Expense 150.5"},
                    //END
                    //Repari Exp
                    new SubLedger {Id=27, Ledger=repairExpenseLedger ,Name="Computer Repair 150.06"},
                    new SubLedger {Id=28, Ledger=repairExpenseLedger ,Name="Cycle Repair 150.06"},
                    new SubLedger {Id=29, Ledger=repairExpenseLedger ,Name="Fintex Annual Maintenance Charge 150.06"},
                    new SubLedger {Id=30, Ledger=repairExpenseLedger ,Name="Generator Repair 150.06"},
                    new SubLedger {Id=34, Ledger=repairExpenseLedger ,Name="Inverter Repair 150.06"},
                    new SubLedger {Id=35, Ledger=repairExpenseLedger ,Name="Motorcycle Repair 150.06"},
                    //END
                    //Miscellaneous Exp
                    new SubLedger {Id=36, Ledger=miscellaneousExpenseLedger ,Name="Miscellaneous Expense"},
                    new SubLedger {Id=37, Ledger=miscellaneousExpenseLedger ,Name="Agm Exp"},
                    new SubLedger {Id=38, Ledger=miscellaneousExpenseLedger ,Name="Audit Exp"},
                    new SubLedger {Id=39, Ledger=miscellaneousExpenseLedger ,Name="Dhito Sulka Exp"},
                    new SubLedger {Id=40, Ledger=miscellaneousExpenseLedger ,Name="Donation Exp"},
                    new SubLedger {Id=41, Ledger=miscellaneousExpenseLedger ,Name="Electricity Exp"},
                    new SubLedger {Id=42, Ledger=miscellaneousExpenseLedger ,Name="Entertainment Exp"},
                    new SubLedger {Id=43, Ledger=miscellaneousExpenseLedger ,Name="Fuel and Transportation Exp"},
                    new SubLedger {Id=44, Ledger=miscellaneousExpenseLedger ,Name="Guest Service Exp"},
                    new SubLedger {Id=45, Ledger=miscellaneousExpenseLedger ,Name="Loan Provision For Bad Dept Exp"},
                    new SubLedger {Id=46, Ledger=miscellaneousExpenseLedger ,Name="Loan Risk Expenses"},
                    new SubLedger {Id=47, Ledger=miscellaneousExpenseLedger ,Name="Paint House exp"},
                    new SubLedger {Id=48, Ledger=miscellaneousExpenseLedger ,Name="Provident Fund Expense"},
                    new SubLedger {Id=49, Ledger=miscellaneousExpenseLedger ,Name="Sweeper exp"},
                    new SubLedger {Id=50, Ledger=miscellaneousExpenseLedger ,Name="Talim Exp"},
                    new SubLedger {Id=51, Ledger=miscellaneousExpenseLedger ,Name="Tax Expense"},
                    new SubLedger {Id=52, Ledger=miscellaneousExpenseLedger ,Name="Tea & Water"},
                    new SubLedger {Id=53, Ledger=miscellaneousExpenseLedger ,Name="Telephone & Communication"},
                    //END
                    // Depreciation Exp
                    new SubLedger {Id=54, Ledger=depreciationExpenseLedger ,Name="Haras-150.9"},
                    //END
                    //Fuel Exp
                    new SubLedger {Id=55, Ledger=fuelExpenseLedger ,Name="Kerosine-150.10"},
                    new SubLedger {Id=56, Ledger=fuelExpenseLedger ,Name="Petrol Exp-150.10"},
                    //END
                    // Rebate Exp
                    new SubLedger {Id=57, Ledger=fuelExpenseLedger ,Name="Rebate On Interest"}

                });
                return await context.SaveChangesAsync();
            }
            return 1;
        }

        private static async Task<int> SeedLedgerData(ApplicationDbContext context)
        {
            if (!context.Ledgers.Any())
            {
                // Assets  //
                var assetsCash = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "80").FirstOrDefaultAsync();
                var receivableCash = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "120").FirstOrDefaultAsync();
                var fixedAssetsCash = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "130").FirstOrDefaultAsync();
                var otherAssetsCash = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "140").FirstOrDefaultAsync();

                // Liability //
                var capitalLiability = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "10").FirstOrDefaultAsync();
                var depositLiability = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "30").FirstOrDefaultAsync();
                var donationLiability = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "50").FirstOrDefaultAsync();
                var payableLiability = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "60").FirstOrDefaultAsync();
                var otherLiability = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "70").FirstOrDefaultAsync();

                // Income //
                var goodIncome = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "160.1").FirstOrDefaultAsync();
                var investmentIncome = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "160.3").FirstOrDefaultAsync();
                var otherIncome = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "160.4").FirstOrDefaultAsync();
                var loanProcessingDonationIncome = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "160.6").FirstOrDefaultAsync();

                // Expense //
                var expenseGT = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "150").FirstOrDefaultAsync();

                await context.Ledgers.AddRangeAsync(new List<Ledger>
                {
                    // ASSETS//
                    new Ledger {Id=1, GroupType=assetsCash ,Name="Cash In Hand", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=2, GroupType=assetsCash ,Name="Cash In Vault", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=3, GroupType=assetsCash ,Name="Cheque In Collection", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=4, GroupType=receivableCash ,Name="Advanced 120.2", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=5, GroupType=receivableCash ,Name="Branch ABBS Receivable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=6, GroupType=fixedAssetsCash ,Name="Furniture", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=7, GroupType=fixedAssetsCash ,Name="Land Or Building", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=8, GroupType=fixedAssetsCash ,Name="Iconic Developer", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=9, GroupType=fixedAssetsCash ,Name="Motorcycle", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=10, GroupType=fixedAssetsCash ,Name="Office Equipment", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=11, GroupType=fixedAssetsCash ,Name="Vehicle", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=12, GroupType=otherAssetsCash ,Name="Last Year Loss", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=13, GroupType=otherAssetsCash ,Name="Other Assets", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=14, GroupType=otherAssetsCash ,Name="Deposit Dr Interest Receivable A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=15, GroupType=otherAssetsCash ,Name="Loan Interest Accrue Ri (Regular Interest)", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    // END 

                    // LIABILITY //
                    new Ledger {Id=16, GroupType=capitalLiability ,Name="Ordinary Share Capital", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=17, GroupType=capitalLiability ,Name="Promoter Share Capital", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=18, GroupType=depositLiability ,Name="Current Saving-30.1", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=19, GroupType=depositLiability ,Name="Saving Deposit-30.2", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=20, GroupType=depositLiability ,Name="Fixed Deposit-30.3", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=21, GroupType=depositLiability ,Name="Recurring Deposit", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=22, GroupType=donationLiability ,Name="Donation From Ministry Of Sahakari", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=23, GroupType=donationLiability ,Name="Donation Received", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=24, GroupType=payableLiability ,Name="Branch ABBS Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=25, GroupType=payableLiability ,Name="Adit Tax", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=26, GroupType=payableLiability ,Name="Dividend Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=27, GroupType=payableLiability ,Name="Sundry Creditors", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=28, GroupType=payableLiability ,Name="Sundry Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=29, GroupType=payableLiability ,Name="Tax Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=30, GroupType=otherLiability ,Name="Audit Fee Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=31, GroupType=otherLiability ,Name="House Rent Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=32, GroupType=otherLiability ,Name="Interest Payable A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=33, GroupType=otherLiability ,Name="Other Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=34, GroupType=otherLiability ,Name="Profit & Loss Account", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=35, GroupType=otherLiability ,Name="Rin Jokhim Kosh", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=36, GroupType=otherLiability ,Name="Staff Salary Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=37, GroupType=otherLiability ,Name="Clearing A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=38, GroupType=otherLiability ,Name="Deposit Adjustment A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=39, GroupType=otherLiability ,Name="Deposit Dr Interest Suspense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=40, GroupType=otherLiability ,Name="Loan Advance Interest", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    //END//
                    // INCOME //
                    new Ledger {Id=41, GroupType=goodIncome ,Name="Goods Sales-160.01", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=42, GroupType=investmentIncome ,Name="Interest Income Bank", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=43, GroupType=otherIncome ,Name="Account Closing Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=44, GroupType=otherIncome ,Name="Cheque Book Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=45, GroupType=otherIncome ,Name="Commision From Remittance", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=46, GroupType=otherIncome ,Name="Draft Commission Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=47, GroupType=otherIncome ,Name="Entry Fee", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=48, GroupType=otherIncome ,Name="Excess Wdr Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=49, GroupType=otherIncome ,Name="Fine & Penalty", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=50, GroupType=otherIncome ,Name="Form Fee", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=51, GroupType=otherIncome ,Name="Interest Reimburse A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=52, GroupType=otherIncome ,Name="Loan Service Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=53, GroupType=otherIncome ,Name="Membership Fee", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=54, GroupType=otherIncome ,Name="Miscellaneous Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=55, GroupType=otherIncome ,Name="Passbook Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=56, GroupType=otherIncome ,Name="Regd. Fee Income-160.4", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=57, GroupType=otherIncome ,Name="Service Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=58, GroupType=otherIncome ,Name="Talim Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {Id=59, GroupType=loanProcessingDonationIncome ,Name="Loan Processing Donation Income-160.6", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=60, GroupType=loanProcessingDonationIncome ,Name="Deposit Dr Interest Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=61, GroupType=loanProcessingDonationIncome ,Name="Min Bal Wdr Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    // END

                    // Expense //
                    new Ledger {Id=62, GroupType=expenseGT ,Name="Goods Purchase 150.01", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=63, GroupType=expenseGT ,Name="Dhuwani Jyala Kharcha", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=64, GroupType=expenseGT ,Name="Salary Staff-150.1", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=65, GroupType=expenseGT ,Name="Rent Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=66, GroupType=expenseGT ,Name="Stationary Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=67, GroupType=expenseGT ,Name="Repair Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=68, GroupType=expenseGT ,Name="Interest Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=69, GroupType=expenseGT ,Name="Miscellaneous Expense-150.1", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=70, GroupType=expenseGT ,Name="Depreciation Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=71, GroupType=expenseGT ,Name="Fuel Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=72, GroupType=expenseGT ,Name="Board Meeting Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=73, GroupType=expenseGT ,Name="Penalty Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {Id=74, GroupType=expenseGT ,Name="Rebate Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false}
                    //END
                });

                var result = await context.SaveChangesAsync();
                return result;
            }
            return 1;
        }

        private static async Task<int> SeedGroupTypeData(ApplicationDbContext context)
        {
            if (!context.GroupTypes.Any())
            {
                var assets = await context.AccountTypes.FindAsync(1);
                var expense = await context.AccountTypes.FindAsync(2);
                var income = await context.AccountTypes.FindAsync(3);
                var liability = await context.AccountTypes.FindAsync(4);
                await context.GroupTypes.AddRangeAsync(new List<GroupType>{
                            new GroupType {Name="Cash",CharKhataNumber="80" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Bank",CharKhataNumber="90" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Investment",CharKhataNumber="100" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Loan Investment",CharKhataNumber="110" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Receivable",CharKhataNumber="120" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Interest Receivable",CharKhataNumber="120.3" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Fixed Assets",CharKhataNumber="130" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Other Assets",CharKhataNumber="140" ,EntryDate=DateTime.Now, AccountType=assets},

                            new GroupType {Name="Expenses",CharKhataNumber="150", EntryDate=DateTime.Now, AccountType=expense},

                            new GroupType {Name="Good Sales",CharKhataNumber="160.1", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Loan Interest Earning (Interest Receive From Loan)",CharKhataNumber="160.2", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Income From Investment",CharKhataNumber="160.3", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Other Income",CharKhataNumber="160.4", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Rebate From Purchase",CharKhataNumber="160.5", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Loan Processing Donation",CharKhataNumber="160.6", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Insurance and Loan Found",CharKhataNumber="160.9", EntryDate=DateTime.Now, AccountType=income},

                            new GroupType {Name="Capital", CharKhataNumber="10",EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Reserver Funds",CharKhataNumber="20", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Deposit",CharKhataNumber="30", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Loan Taken",CharKhataNumber="40", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Donation",CharKhataNumber="50", EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Payable", CharKhataNumber="60",EntryDate=DateTime.Now, AccountType=liability},
                            new GroupType {Name="Other Liabilities",CharKhataNumber="70", EntryDate=DateTime.Now, AccountType=liability},
                        });
                return await context.SaveChangesAsync();
            }
            return 1;
        }

        public static async Task SeedMainLedgerAsync(ApplicationDbContext context)
        {
                bool updateStatus=
                await SeedDebitOrCreditData(context)>=1
                &&await SeedAccountTypeAsync(context)>=1 
                && await SeedGroupTypeData(context)>=1 
                && await SeedLedgerData(context)>=1
                &&await SeedSubLedgerData(context)>=1
                && await SeedBankTypeData(context)>=1;
                
           // }

        }
    }

}
