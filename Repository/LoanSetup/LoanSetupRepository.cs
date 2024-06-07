using System.Linq.Expressions;
using MicroFinance.DBContext;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Dtos.LoanSetup;
using MicroFinance.Enums;
using MicroFinance.Helpers;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.LoanSetup;
using MicroFinance.Repository.AccountSetup.MainLedger;
using MicroFinance.Services.CompanyProfile;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.LoanSetup
{
    public class LoanSetupRepository : ILoanSetupRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMainLedgerRepository _mainLedgerRepository;
        private readonly ICompanyProfileService _companyProfileService;
        private readonly IHelper _helper;
        private SemaphoreSlim loanAccountLock = new SemaphoreSlim(1, 1);

        public LoanSetupRepository
        (
            ApplicationDbContext dbContext,
            IMainLedgerRepository mainLedgerRepository,
            IHelper helper,
            ICompanyProfileService companyProfileService
        )
        {
            _dbContext = dbContext;
            _mainLedgerRepository = mainLedgerRepository;
            _companyProfileService=companyProfileService;
            _helper=helper;
        }

        private async Task<List<Ledger>> CreateLedgersAccount(string assetsAccountLedgerName, string interestAccountLedgerName, DateTime entryDate)
        {
            GroupType loanInvestment = await _mainLedgerRepository.GetGroupTypeByCharKhataNumber("110");
            GroupType interestEarning = await _mainLedgerRepository.GetGroupTypeByCharKhataNumber("160.2");
            _dbContext.GroupTypes.Attach(loanInvestment);
            _dbContext.GroupTypes.Attach(interestEarning);
            Ledger assetsAccountLedger = new Ledger()
            {
                Name = assetsAccountLedgerName,
                EntryDate = entryDate,
                IsSubLedgerActive = true,
                CurrentBalance = 0,
                GroupType = loanInvestment,
                IsBank = false
            };
            Ledger interestAccountLedger = new Ledger()
            {
                Name = assetsAccountLedgerName,
                EntryDate = entryDate,
                IsSubLedgerActive = true,
                CurrentBalance = 0,
                GroupType = interestEarning,
                IsBank = false
            };
            List<Ledger> allLedgerForLoanScheme = new List<Ledger>() { assetsAccountLedger, interestAccountLedger };
            int numberOfLedgerUpdated = await _mainLedgerRepository.CreateLedgers(allLedgerForLoanScheme);
            if (numberOfLedgerUpdated <= 0) throw new Exception("Unable to create ledger for Loan Investment and Interest Earning");
            return allLedgerForLoanScheme;
        }
        public async Task<int> CreateLoanScheme(LoanScheme loanScheme, string assetsAccountLedgerName, string interestAccountLedgerName)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                List<Ledger> allLedgersForLoanScheme = await CreateLedgersAccount(assetsAccountLedgerName, interestAccountLedgerName, loanScheme.EnglishCreationDate);
                loanScheme.AssetsAccountLedger = allLedgersForLoanScheme[0];
                loanScheme.InterestAccountLedger = allLedgersForLoanScheme[1];
                await _dbContext.LoanSchemes.AddAsync(loanScheme);
                int numberOfRowAffected = await _dbContext.SaveChangesAsync();
                if (numberOfRowAffected <= 0) throw new Exception("Unable to create a Loan Scheme");
                await transaction.CommitAsync();
                return numberOfRowAffected;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ValidateAliasCode(string aliasCode)
        {
            var loanScheme = await _dbContext.LoanSchemes.Where(ls => ls.AliasCode == aliasCode).AsNoTracking().FirstOrDefaultAsync();
            if (loanScheme == null)
                return true;
            return false;
        }

        private async Task ChangeWithDrawalStatusOnDepositAccount(List<int> depositAccountIds, bool isBlock)
        {
            var depositAccounts = await _dbContext.DepositAccounts.Where(i => depositAccountIds.Contains(i.Id)).ToListAsync();
            if (depositAccountIds.Count != depositAccounts.Count)
                throw new Exception("Please check if deposit account number passed is valid");
            foreach (var account in depositAccounts)
                _ = isBlock ? account.IsWithDrawalAllowed = false : account.IsWithDrawalAllowed = true;
        }

        public async Task<int> CreateLoanAccount(LoanAccount loanAccount)
        {
            try
            {
                if(await loanAccountLock.WaitAsync(TimeSpan.FromMinutes(1)))
                {
                    LoanScheme loanScheme = await _dbContext.LoanSchemes.FindAsync(loanAccount.LoanSchemeId);
                    Client client = await _dbContext.Clients.FindAsync(loanAccount.ClientId);
                    if (loanScheme == null || !loanScheme.IsActive || client == null || !client.IsActive)
                        throw new Exception("Either Client or LoanScheme is not valid");
                    if (loanAccount.WithDrawalBlockedDepositAccountIds != null)
                        await ChangeWithDrawalStatusOnDepositAccount(depositAccountIds: loanAccount.WithDrawalBlockedDepositAccountIds, isBlock: true);
                    bool isInterestValid =
                    loanAccount.InterestRate <= loanScheme.MaximumInterestRate && loanAccount.InterestRate >= loanScheme.MinimumInterestRate;
                    if (!isInterestValid)
                        throw new Exception("Interest Rate should be between minimum and maximum interest rate of loan scheme");
                    bool validInterestRatype = 
                    (loanScheme.IsRevolving && loanAccount.InterestType==LoanInterestTypeEnum.Diminishing)   
                    ||
                    (!loanScheme.IsRevolving && loanAccount.InterestType==LoanInterestTypeEnum.Flat);
                    
                    if(!validInterestRatype)
                        throw new Exception($"Provided Interest Rate Type is not valid when Loan Scheme has Relvoling condition {loanScheme.IsRevolving}");  
                    loanAccount.LoanScheme = loanScheme;
                    loanAccount.Client = client;
                    int highedstIndexNumber = (await _dbContext.LoanAccounts.MaxAsync(x=>(int?) x.Id)) ?? 0;
                    int newIndexNumber = highedstIndexNumber+1;
                    loanAccount.AccountNumber = $"{loanScheme.AliasCode}{newIndexNumber}{loanAccount.BranchCode}";
                    await _dbContext.LoanAccounts.AddAsync(loanAccount);
                    return await _dbContext.SaveChangesAsync();
                }
                else
                    throw new TimeoutException("Waited Too long");
                
            }
            finally
            {
                loanAccountLock.Release();
            }

        }

        public async Task<List<LoanScheme>> GetSchemes(Expression<Func<LoanScheme, bool>> expression)
        {
            return await _dbContext.LoanSchemes
            .Include(ls=>ls.AssetsAccountLedger)
            .Include(ls=>ls.InterestAccountLedger)
            .Where(expression).ToListAsync();
        }

        public async Task<List<LoanAccount>> GetLoanAccounts(int? loanAccountId)
        {
            if(loanAccountId!=null)
                return await _dbContext.LoanAccounts
                .Include(la=>la.LoanScheme)
                .Where(la=>la.Id==loanAccountId).ToListAsync();
            return await _dbContext.LoanAccounts.Include(la=>la.LoanScheme).ToListAsync();
        }
        /// <summary>
        /// If Interest Posting is  Monthly 
        ///    PA Payment: HalfYear - MaturityPeriod should greater or equal to 6 month
        ///    PA Payment: Quarterly - MaturityPeriod should greater than or equal 3 month
        ///    PA Payment: Monthly - Minimum MaturityPeriod should 1 month
        ///    PA Payment: Yealy - Minimum MaturityPeriod should be 1 year
        /// If Interest Posting is   EMI
        ///    PA Payment : EMI
        /// </summary>
        /// <param name="generateLoanSchedule"></param>
        /// <returns></returns>
        public async Task<LoanScheduleDtos> GenerateLoanSchedule(GenerateLoanScheduleDto generateLoanSchedule)
        {
            var companyActiveCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            DateTime companyActiveDateAD=await _helper.GetCompanyActiveCalendarInAD(companyActiveCalendar);
            GenerateMatureDateDto generateMatureDate = new GenerateMatureDateDto()
            {
                OpeningDate=await _helper.GetNepaliFormatDate($"{companyActiveCalendar.Year}-{companyActiveCalendar.Month}-{companyActiveCalendar.RunningDay}"),
                OpeningDateEnglish=companyActiveDateAD,
                Period=generateLoanSchedule.Period,
                PeriodType=generateLoanSchedule.PeriodType
            };
            var matureDate = await _helper.GenerateMatureDateOfAccount(generateMatureDate);
            List<LoanScheduleDto> loanSchedules = new List<LoanScheduleDto>();
            LoanScheduleDtos loanScheduleDtos = new LoanScheduleDtos();
            if(generateLoanSchedule.PrincipalPaymentSchedule == PostingSchemeEnum.Monthly)
            {
                int diffYear = matureDate.EnglishMatureDate.Year - companyActiveDateAD.Year;
                int diffMonth = matureDate.EnglishMatureDate.Month - companyActiveDateAD.Month;
                int diffDay = matureDate.EnglishMatureDate.Day - companyActiveDateAD.Day;
                int totalMonth = diffYear * 12 + diffMonth;

                int lengthOfMatureAccount = (matureDate.EnglishMatureDate - companyActiveDateAD).Days;
                if(lengthOfMatureAccount < companyActiveCalendar.NumberOfDay)
                    throw new Exception("Mature Date is too soon");

                decimal remainingPrincipalAmount = generateLoanSchedule.LoanLimit;
                decimal monthlyPAInstallment = remainingPrincipalAmount / totalMonth;

                DateTime lastPostingDate = companyActiveDateAD;
                decimal interestRate = (decimal)generateLoanSchedule.InterestRate;
                while(totalMonth>0)
                {
                    DateTime nextInterestPosting = await _helper.GenerateNextInterestPostingDate(lastPostingDate, matureDate.EnglishMatureDate, companyActiveCalendar, generateLoanSchedule.PrincipalPaymentSchedule, true);
                    string nepaliNextInterestPosting = await _helper.ConvertEnglishDateToNepali(nextInterestPosting);
                    decimal numberOfDaysValidForInterestPost = (nextInterestPosting-lastPostingDate).Days;
                    LoanScheduleDto loanSchedule = new LoanScheduleDto()
                    {
                        PrincipalAmount = monthlyPAInstallment,
                        InterestAmount = remainingPrincipalAmount*interestRate*numberOfDaysValidForInterestPost/36500,
                        PaymentDateAD = nextInterestPosting,
                        PaymentDateBS = nepaliNextInterestPosting
                    };
                    loanSchedule.TotalAmount = loanSchedule.PrincipalAmount + loanSchedule.InterestAmount;
                    loanSchedules.Add(loanSchedule);
                    lastPostingDate = nextInterestPosting;
                    remainingPrincipalAmount=remainingPrincipalAmount-monthlyPAInstallment;
                    totalMonth--;
                }
                if(diffDay>0)
                {
                    decimal remainingInstallment = monthlyPAInstallment * diffDay;
                    DateTime nextInterestPosting = await _helper.GenerateNextInterestPostingDate(lastPostingDate, matureDate.EnglishMatureDate, companyActiveCalendar, generateLoanSchedule.PrincipalPaymentSchedule, true);
                    nextInterestPosting = nextInterestPosting < matureDate.EnglishMatureDate ? nextInterestPosting : matureDate.EnglishMatureDate;
                    string nepaliNextInterestPosting = await _helper.ConvertEnglishDateToNepali(nextInterestPosting);
                    LoanScheduleDto loanSchedule = new LoanScheduleDto()
                    {
                        PrincipalAmount = remainingInstallment,
                        InterestAmount = remainingPrincipalAmount*interestRate*diffDay/36500,
                        PaymentDateAD = nextInterestPosting,
                        PaymentDateBS = nepaliNextInterestPosting
                    };
                    loanSchedule.TotalAmount = loanSchedule.PrincipalAmount + loanSchedule.InterestAmount;
                    loanSchedules.Add(loanSchedule);
                }
                loanScheduleDtos.LoanScheduleDto = loanSchedules;
                loanScheduleDtos.TotalInterestAmount = loanSchedules.Sum(ls=>ls.InterestAmount);
                loanScheduleDtos.TotalPrincipalAmount = loanSchedules.Sum(ls=>ls.PrincipalAmount);
            }
            return loanScheduleDtos;
        }


    }
}