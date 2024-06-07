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
                    new BankType {Id=1,Name="Commercial"},
                    new BankType {Id=2,Name="Development"},
                    new BankType {Id=3,Name="Finance"}
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
                var advancedLedger = await context.Ledgers.Where(l => l.LedgerCode == 4).SingleOrDefaultAsync();
                var taxPayableLedger = await context.Ledgers.Where(l => l.LedgerCode == 29).SingleOrDefaultAsync();
               

                await context.SubLedgers.AddRangeAsync(new List<SubLedger>
                {
                    new SubLedger {SubLedgerCode=1, Ledger=taxPayableLedger ,Name="Deposit Interest Tax Payable"}
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
                var interestReceivable = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="120.3").FirstOrDefaultAsync();

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
                var interestExpense = await context.GroupTypes.Where(gt => gt.CharKhataNumber == "150").FirstOrDefaultAsync();
                var goodPurchase = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.1").FirstOrDefaultAsync();
                var wages = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.2").FirstOrDefaultAsync();
                var salaryExpense = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.3").FirstOrDefaultAsync();
                var rentExpense = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.4").FirstOrDefaultAsync();
                var stationaryExpenses = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.5").FirstOrDefaultAsync();
                var repairExpenses = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.6").FirstOrDefaultAsync();
                var miscellaneousExpense = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.8").FirstOrDefaultAsync();
                var depreciationExpense = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.9").FirstOrDefaultAsync();
                var fuelExpenses = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.10").FirstOrDefaultAsync();
                var boardExpenses = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.11").FirstOrDefaultAsync();
                var penaltyExpenses = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.12").FirstOrDefaultAsync();
                var rebateExpense = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.13").FirstOrDefaultAsync();
                var mahasulExpenses = await context.GroupTypes.Where(gt=>gt.CharKhataNumber=="150.17").FirstOrDefaultAsync();

                //await context.Ledgers.AddRangeAsync(
                var temp = new Ledger { Id = 555, GroupType = assetsCash, Name = "Cash In Hand", IsSubLedgerActive = true, EntryDate = DateTime.Now, IsBank = false };
                var ledgers = new List<Ledger>
                {
                    // ASSETS//
                    new Ledger {LedgerCode=1, GroupType=assetsCash ,Name="Cash In Hand", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=2, GroupType=assetsCash ,Name="Cash In Vault", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=3, GroupType=assetsCash ,Name="Cheque In Collection", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=4, GroupType=receivableCash ,Name="Advanced 120.2", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=5, GroupType=receivableCash ,Name="Branch ABBS Receivable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=6, GroupType=fixedAssetsCash ,Name="Furniture", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=7, GroupType=fixedAssetsCash ,Name="Land Or Building", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=8, GroupType=fixedAssetsCash ,Name="Iconic Developer", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=9, GroupType=fixedAssetsCash ,Name="Motorcycle", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=10, GroupType=fixedAssetsCash ,Name="Office Equipment", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=11, GroupType=fixedAssetsCash ,Name="Vehicle", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=12, GroupType=otherAssetsCash ,Name="Last Year Loss", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=13, GroupType=otherAssetsCash ,Name="Other Assets", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=14, GroupType=otherAssetsCash ,Name="Deposit Dr Interest Receivable A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=15, GroupType=otherAssetsCash ,Name="Loan Interest Accrue Ri (Regular Interest)", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    // END
                    // LIABILITY
                    // Used
                    new Ledger {LedgerCode=16, GroupType=capitalLiability ,Name="Ordinary Share Capital", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=17, GroupType=capitalLiability ,Name="Promoter Share Capital", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    // Used
                    new Ledger {LedgerCode=18, GroupType=depositLiability ,Name="Current Saving-30.1", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=19, GroupType=depositLiability ,Name="Saving Deposit-30.2", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=20, GroupType=depositLiability ,Name="Fixed Deposit-30.3", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=21, GroupType=depositLiability ,Name="Recurring Deposit", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=22, GroupType=donationLiability ,Name="Donation From Ministry Of Sahakari", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=23, GroupType=donationLiability ,Name="Donation Received", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=24, GroupType=payableLiability ,Name="Branch ABBS Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=25, GroupType=payableLiability ,Name="Adit Tax", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=26, GroupType=payableLiability ,Name="Dividend Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=27, GroupType=payableLiability ,Name="Sundry Creditors", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=28, GroupType=payableLiability ,Name="Sundry Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    // Used
                    new Ledger {LedgerCode=29, GroupType=payableLiability ,Name="Tax Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                
                    new Ledger {LedgerCode=30, GroupType=otherLiability ,Name="Audit Fee Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=31, GroupType=otherLiability ,Name="House Rent Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=32, GroupType=otherLiability ,Name="Interest Payable A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=33, GroupType=otherLiability ,Name="Other Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=34, GroupType=otherLiability ,Name="Profit & Loss Account", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=35, GroupType=otherLiability ,Name="Rin Jokhim Kosh", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=36, GroupType=otherLiability ,Name="Staff Salary Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=37, GroupType=otherLiability ,Name="Clearing A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=38, GroupType=otherLiability ,Name="Deposit Adjustment A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=39, GroupType=otherLiability ,Name="Deposit Dr Interest Suspense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=40, GroupType=otherLiability ,Name="Loan Advance Interest", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    //END
                    // INCOME 
                    new Ledger {LedgerCode=41, GroupType=goodIncome ,Name="Goods Sales-160.01", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=42, GroupType=investmentIncome ,Name="Interest Income Bank", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=43, GroupType=otherIncome ,Name="Account Closing Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=44, GroupType=otherIncome ,Name="Cheque Book Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=45, GroupType=otherIncome ,Name="Commision From Remittance", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=46, GroupType=otherIncome ,Name="Draft Commission Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=47, GroupType=otherIncome ,Name="Entry Fee", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=48, GroupType=otherIncome ,Name="Excess Wdr Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=49, GroupType=otherIncome ,Name="Fine & Penalty", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=50, GroupType=otherIncome ,Name="Form Fee", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=51, GroupType=otherIncome ,Name="Interest Reimburse A/C", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=52, GroupType=otherIncome ,Name="Loan Service Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=53, GroupType=otherIncome ,Name="Membership Fee", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=54, GroupType=otherIncome ,Name="Miscellaneous Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=55, GroupType=otherIncome ,Name="Passbook Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=56, GroupType=otherIncome ,Name="Regd. Fee Income-160.4", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=57, GroupType=otherIncome ,Name="Service Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=58, GroupType=otherIncome ,Name="Talim Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=59, GroupType=loanProcessingDonationIncome ,Name="Loan Processing Donation Income-160.6", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=60, GroupType=loanProcessingDonationIncome ,Name="Deposit Dr Interest Income", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=61, GroupType=loanProcessingDonationIncome ,Name="Min Bal Wdr Charge", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    // End
                    // Expense
                    new Ledger {LedgerCode=62, GroupType=goodPurchase ,Name="Goods Purchase 150.01", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=63, GroupType=wages ,Name="Dhuwani Jyala Kharcha", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=64, GroupType=wages ,Name="Wages Fair Expenses 150.2", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    // Assets - Interest Recievable, added to manage the code
                    new Ledger {LedgerCode=65, GroupType=interestReceivable ,Name="Loan Interest Receivable (PI)", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {LedgerCode=66, GroupType=salaryExpense ,Name="Over Duty (Staff) 150.3", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=67, GroupType=salaryExpense ,Name="Salary (Staff) 150.3", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    // Used
                    new Ledger {LedgerCode=68, GroupType=interestExpense, Name="Interest Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    new Ledger {LedgerCode=69, GroupType=salaryExpense ,Name="Sanchay Kosh 150.03", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=70, GroupType=salaryExpense ,Name="Staff Allowance Payment 150.3", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=71, GroupType=salaryExpense ,Name="Transport & Daily Allowance 150.3", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                   
                    new Ledger {LedgerCode=72, GroupType=rentExpense ,Name="House Rent 150.4", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=73, GroupType=rentExpense ,Name="Room Rent Expense 150.4", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=74, GroupType=stationaryExpenses ,Name="Photocopy Expense 150.5", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=75, GroupType=stationaryExpenses ,Name="Printing and Stationary", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=76, GroupType=stationaryExpenses ,Name="Stationary Expense 150.5", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=77, GroupType=repairExpenses ,Name="Repair Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=78, GroupType=repairExpenses ,Name="Computer Repair 150.06", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=79, GroupType=repairExpenses ,Name="Fintex Annual Maintenance Charge 150.06", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=80, GroupType=repairExpenses ,Name="Generator Repair 150.06", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=81, GroupType=repairExpenses ,Name="Inverter Repair 150.06", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=82, GroupType=repairExpenses ,Name="Motorcycle Repair 150.06", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=83, GroupType=miscellaneousExpense ,Name="Miscellaneous Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=84, GroupType=miscellaneousExpense ,Name="Agm Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=85, GroupType=miscellaneousExpense ,Name="Audit Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=86, GroupType=miscellaneousExpense ,Name="Dhito Sulka Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=87, GroupType=miscellaneousExpense ,Name="Donation Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=88, GroupType=miscellaneousExpense ,Name="Electricity Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=89, GroupType=miscellaneousExpense ,Name="Entertainment Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=90, GroupType=miscellaneousExpense ,Name="Guest Service Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=91, GroupType=miscellaneousExpense ,Name="Loan Provision For Bad Dept Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=92, GroupType=miscellaneousExpense ,Name="Loan Risk Expenses", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=93, GroupType=miscellaneousExpense ,Name="Paint House exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=94, GroupType=miscellaneousExpense ,Name="Provident Fund Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=95, GroupType=miscellaneousExpense ,Name="Sweeper exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=96, GroupType=miscellaneousExpense ,Name="Talim Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=97, GroupType=miscellaneousExpense ,Name="Tax Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=98, GroupType=miscellaneousExpense ,Name="Tea & Water", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=99, GroupType=miscellaneousExpense ,Name="Telephone & Communication", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=100, GroupType=depreciationExpense ,Name="Depreciation Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=101, GroupType=depreciationExpense ,Name="Haras-150.9", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                
                    new Ledger {LedgerCode=102, GroupType=fuelExpenses ,Name="Fuel Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=103, GroupType=fuelExpenses ,Name="Kerosine-150.10", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=104, GroupType=fuelExpenses ,Name="Petrol Exp-150.10", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                
                    new Ledger {LedgerCode=105, GroupType=boardExpenses ,Name="Board Meeting Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=106, GroupType=penaltyExpenses ,Name="Penalty Exp", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    
                    new Ledger {LedgerCode=107, GroupType=rebateExpense ,Name="Rebate Expense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=108, GroupType=rebateExpense ,Name="Rebate On Interest", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                    //END
                    //new Assets
                    new Ledger {LedgerCode=109, GroupType=interestReceivable ,Name="Loan Interest Receivable (RI) Regular Interest", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=110, GroupType=interestReceivable ,Name="Loan Interest Receivable (ii)", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=111, GroupType=interestReceivable ,Name="Loan Interest Receivable (ARI)", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=112, GroupType=interestReceivable ,Name="Loan Interest Receivable (OI)", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=113, GroupType=receivableCash ,Name="Staff Advanced", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=114, GroupType=receivableCash ,Name="Remittance Receivable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=115, GroupType=receivableCash ,Name="sundry Debtors120.1", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=116, GroupType=receivableCash ,Name="Telephone Deposit", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=117, GroupType=receivableCash ,Name="Room Rent Advance", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},   
                    new Ledger {LedgerCode=118, GroupType=receivableCash ,Name="Advance income Tax", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=119, GroupType=receivableCash ,Name="Advance  Tax", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},   

                    // New Payable
                    new Ledger {LedgerCode=120, GroupType=payableLiability ,Name="Dividend Tax Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=121, GroupType=payableLiability ,Name="House Rent Tax Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=122, GroupType=payableLiability ,Name="Income Tax Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},
                    new Ledger {LedgerCode=123, GroupType=payableLiability ,Name="Staff salary Tax Payable", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},         
                    
                    // New Liability
                    new Ledger {LedgerCode=124, GroupType=otherLiability ,Name="Loan Interest Suspense", IsSubLedgerActive=true, EntryDate=DateTime.Now, IsBank=false},

                };
                //);
                foreach (var ledger in ledgers)
                {
                    await context.AddAsync(ledger);
                }
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
                            // ASSETS AT
                            new GroupType {Name="Cash",CharKhataNumber="80" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Bank",CharKhataNumber="90" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Investment",CharKhataNumber="100" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Loan Investment",CharKhataNumber="110" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Receivable",CharKhataNumber="120" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Interest Receivable",CharKhataNumber="120.3" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Fixed Assets",CharKhataNumber="130" ,EntryDate=DateTime.Now, AccountType=assets},
                            new GroupType {Name="Other Assets",CharKhataNumber="140" ,EntryDate=DateTime.Now, AccountType=assets},
                            
                            // EXPENSE AT

                            new GroupType {Name="INTEREST EXPENSES",CharKhataNumber="150", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="GOODS PURCHASE",CharKhataNumber="150.1", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="WAGES",CharKhataNumber="150.2", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="SALARY EXPENSE",CharKhataNumber="150.3", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="RENT EXPENSES",CharKhataNumber="150.4", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="STATIONARY EXPENSES",CharKhataNumber="150.5", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="REPAIR EXPENSES",CharKhataNumber="150.6", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="Miscellaneous Expense",CharKhataNumber="150.8", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="DEPRECIATION EXPENSES",CharKhataNumber="150.9", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="FUEL EXPENSES",CharKhataNumber="150.10", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="BOARD EXPENSES",CharKhataNumber="150.11", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="PENALTY & PRODUCTION EXPENSES",CharKhataNumber="150.12", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="REBATE EXPENSES",CharKhataNumber="150.13", EntryDate=DateTime.Now, AccountType=expense},
                            new GroupType {Name="MAHASUL EXPENSES",CharKhataNumber="150.17", EntryDate=DateTime.Now, AccountType=expense},
                            // END veriy by Kundan

                            // INCOME AT

                            new GroupType {Name="Good Sales",CharKhataNumber="160.1", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Loan Interest Earning (Interest Receive From Loan)",CharKhataNumber="160.2", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Income From Investment",CharKhataNumber="160.3", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Other Income",CharKhataNumber="160.4", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Rebate From Purchase",CharKhataNumber="160.5", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Loan Processing Donation",CharKhataNumber="160.6", EntryDate=DateTime.Now, AccountType=income},
                            new GroupType {Name="Insurance and Loan Found",CharKhataNumber="160.9", EntryDate=DateTime.Now, AccountType=income},

                            // Liability AT

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
            bool updateStatus =
            await SeedDebitOrCreditData(context) >= 1
            && await SeedAccountTypeAsync(context) >= 1
            && await SeedGroupTypeData(context) >= 1
            && await SeedLedgerData(context) >= 1
            && await SeedSubLedgerData(context) >= 1
            && await SeedBankTypeData(context) >= 1;

            // }

        }
    }

}
