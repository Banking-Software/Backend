using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Wrapper;
using MicroFinance.Repository.AccountSetup.MainLedger;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.DepositSetup
{
    public class DepositSchemeRepository : IDepositSchemeRepository
    {
        private readonly ApplicationDbContext _depositDbContext;
        private readonly ILogger<DepositSchemeRepository> _logger;
        private readonly IMainLedgerRepository _mainLedgerRepository;
        private readonly IMapper _mapper;

        public DepositSchemeRepository
        (
        ApplicationDbContext depositDbContext,
        ILogger<DepositSchemeRepository> logger,
        IMainLedgerRepository mainLedgerRepository,
        IMapper mapper

        )
        {
            _depositDbContext = depositDbContext;
            _logger = logger;
            _mainLedgerRepository = mainLedgerRepository;
            _mapper = mapper;
        }
        public async Task<int> CreateDepositScheme(DepositScheme depositScheme)
        {
            try
            {
                await _depositDbContext.DepositSchemes.AddAsync(depositScheme);
                var createStatus = await _depositDbContext.SaveChangesAsync();
                if (createStatus < 1) throw new Exception("Failed to create Scheme");
                _logger.LogInformation($"{DateTime.Now}: Deposit Scheme {depositScheme.SchemeName} created by employee {depositScheme.CreatedBy}");
                return depositScheme.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: Deposit Scheme {depositScheme.SchemeName} failed created by employee {depositScheme.CreatedBy}");
                _logger.LogInformation($"{DateTime.Now}: Removing created subledger from the given scheme...");
                _depositDbContext.SubLedgers.Remove(depositScheme.DepositSubLedger);
                _depositDbContext.SubLedgers.Remove(depositScheme.InterestSubledger);
                _depositDbContext.SubLedgers.Remove(depositScheme.TaxSubledger);
                await _depositDbContext.SaveChangesAsync();
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> UpdateDepositScheme(DepositScheme updateDepositScheme)
        {
            var existingDepositScheme = await _depositDbContext.DepositSchemes.FindAsync(updateDepositScheme.Id);
            _depositDbContext.Entry(existingDepositScheme).State = EntityState.Detached;
            _depositDbContext.DepositSchemes.Attach(updateDepositScheme);
            _depositDbContext.Entry(updateDepositScheme).State = EntityState.Modified;
            return await _depositDbContext.SaveChangesAsync();
        }

        public async Task<DepositScheme> GetDepositSchemeByName(string name)
        {
            var depositScheme = await _depositDbContext.DepositSchemes
            .Include(ds => ds.DepositSubLedger)
            .Include(ds => ds.InterestSubledger)
            .Include(ds => ds.TaxSubledger)
            .Include(ds => ds.SchemeType)
            .Where(ds => ds.SchemeName == name)
            .SingleOrDefaultAsync();
            return depositScheme;
        }
        public async Task<DepositScheme> GetDepositSchemeBySymbol(string symbol)
        {
            return await _depositDbContext.DepositSchemes.Where(ds=>ds.Symbol==symbol).SingleOrDefaultAsync();
        }

        public async Task<List<DepositScheme>> GetAllDepositScheme()
        {
            return await _depositDbContext.DepositSchemes
            .Include(ds => ds.DepositSubLedger)
             .Include(ds => ds.InterestSubledger)
             .Include(ds => ds.TaxSubledger)
             .Include(ds => ds.SchemeType)
            .ToListAsync();
        }

        public async Task<DepositScheme> GetDepositSchemeById(int id)
        {
            var depositScheme = await _depositDbContext.DepositSchemes
            .Include(ds => ds.DepositSubLedger)
            .Include(ds => ds.InterestSubledger)
            .Include(ds => ds.TaxSubledger)
            .Include(ds => ds.SchemeType)
            .Where(ds => ds.Id == id)
            .SingleOrDefaultAsync();
            return depositScheme;

        }

        // public async Task<List<ResponseDepositScheme>> GetDepositSchemeByPostingScheme(int id)
        // {
        //     var depositSchemesByPostingScheme = await _depositDbContext
        //     .DepositSchemes
        //     .Include(ds => ds.PostingScheme)
        //     .Where(ds => ds.PostingSchemeId == id)
        //     .ToListAsync();

        //     var depositSchemeWithLedgerDetails = new List<ResponseDepositScheme>();
        //     foreach (var scheme in depositSchemesByPostingScheme)
        //     {
        //         ResponseDepositScheme temp = _mapper.Map<ResponseDepositScheme>(scheme);
        //         var ledgerAsInterest = await _mainLedgerRepository.GetLedger(scheme.LedgerAsInterestAccountId);
        //         var ledgerAsLiability = await _mainLedgerRepository.GetLedger(scheme.LedgerAsLiabilityAccountId);
        //         //await Task.WhenAll(ledgerAsInterest, ledgerAsLiability);
        //         temp.InterestAccount = ledgerAsInterest;
        //         temp.LiabilityAccount = ledgerAsLiability;
        //         depositSchemeWithLedgerDetails.Add(temp);
        //     }
        //     return depositSchemeWithLedgerDetails;
        // }

        // public async Task<PostingScheme> GetPositingScheme(int id)
        // {
        //     var positngScheme = await _depositDbContext
        //     .PostingSchemes
        //     .FindAsync(id);

        //     return positngScheme;
        // }


        // DEPOSIT ACCOUNT
        private async Task<int> CreateDepositAccountNumber(DepositAccount depositAccount)
        {
            try
            {
                var existingDepositAccount = await _depositDbContext.DepositAccounts.FindAsync(depositAccount.Id);
                _depositDbContext.Entry(existingDepositAccount).State = EntityState.Detached;
                _depositDbContext.DepositAccounts.Attach(depositAccount);
                depositAccount.AccountNumber = 
                depositAccount.DepositScheme.Symbol+
                depositAccount.BranchCode+
                depositAccount.Id.ToString().PadLeft(5,'0');
                _depositDbContext.Entry(depositAccount).State = EntityState.Modified;
                var updateStatus = await _depositDbContext.SaveChangesAsync();
                if(updateStatus<1) throw new Exception("Unable to Create Deposit Account");
                return depositAccount.Id;
            }
            catch(Exception ex)
            {
                _depositDbContext.DepositAccounts.Remove(depositAccount);
                await _depositDbContext.SaveChangesAsync();
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> CreateDepositAccount(DepositAccount depositAccount)
        {
            await _depositDbContext.DepositAccounts.AddAsync(depositAccount);
            var status = await _depositDbContext.SaveChangesAsync();
            if(status<1) throw new Exception("Unable to Create Deposit Account");
            return await CreateDepositAccountNumber(depositAccount);
        }

        public async Task<int> CreateJointAccount(List<JointAccount> jointAccounts, DepositAccount depositAccount)
        {
            try
            {
                await _depositDbContext.JointAccounts.AddRangeAsync(jointAccounts);
                var addStatus = await _depositDbContext.SaveChangesAsync();
                if(addStatus<1) throw new Exception("Unable to Create Joint Account");
                return addStatus;
            }
            catch (Exception ex)
            {
                _depositDbContext.DepositAccounts.Remove(depositAccount);
                await _depositDbContext.SaveChangesAsync();
                throw new Exception(ex.Message);
            }
        }
         public async Task<List<DepositAccountWrapper>> GetAllNonClosedDepositAccounts()
         {
            var depositAccounts = await _depositDbContext.DepositAccounts
            .Include(da=>da.Client)
            .Include(da=>da.DepositScheme)
            .Include(da=>da.InterestPostingAccountNumber)
            .Include(da=>da.InterestPostingAccountNumber)
            .Where(da=>da.Status!=AccountStatusEnum.Close)
            .ToListAsync();
            List<DepositAccountWrapper> depositAccountWrappers = new List<DepositAccountWrapper>();
            foreach (var depositAccount in depositAccounts)
            {
                DepositAccountWrapper depositWrapper = new DepositAccountWrapper();
                depositWrapper.DepositAccount = depositAccount;
                if(depositAccount.AccountType==AccountTypeEnum.Joint)
                {
                    var jointAccounts = await _depositDbContext.JointAccounts
                    .Include(ja=>ja.JointClient)
                    .Where(ja=>ja.DepositAccountId==depositAccount.Id && ja.RealWorldEndDate==null && ja.CompanyCalendarEndDate==null)
                    .ToListAsync();
                    depositWrapper.JointAccount = jointAccounts;
                }
                depositAccountWrappers.Add(depositWrapper);
            }
            return depositAccountWrappers;
         }
        public async Task<DepositAccountWrapper> GetNonCloseDepositAccountById(int id)
        {
            DepositAccountWrapper nonCloseDepositAccountWrapper = new DepositAccountWrapper();
            var depositAccount = await _depositDbContext.DepositAccounts
            .Include(da=>da.Client)
            .Include(da=>da.DepositScheme)
            .Include(da=>da.InterestPostingAccountNumber)
            .Include(da=>da.InterestPostingAccountNumber)
            .Where(da=>da.Status!=AccountStatusEnum.Close && da.Id==id)
            .SingleOrDefaultAsync();
            if(depositAccount==null)
                return new DepositAccountWrapper();
            if(depositAccount.AccountType == AccountTypeEnum.Joint)
            {
                var jointAccounts = await _depositDbContext.JointAccounts
                .Include(ja=>ja.JointClient)
                .Where(ja=>ja.DepositAccountId==depositAccount.Id && ja.RealWorldEndDate==null && ja.CompanyCalendarEndDate==null)
                .ToListAsync();
                nonCloseDepositAccountWrapper.JointAccount = jointAccounts;
            }
            nonCloseDepositAccountWrapper.DepositAccount = depositAccount;
            return nonCloseDepositAccountWrapper;
        }

        // public async Task<int> UpdateDepositAccount(UpdateDepositAccountDto updateDepositAccount, string modifiedBy)
        // {
        //     var depositAccount = await _depositDbContext.DepositAccounts.Include(da => da.DepositScheme).Where(da => da.Id == updateDepositAccount.Id).SingleOrDefaultAsync();
        //     if (depositAccount == null) throw new NotFoundExceptionHandler("Given Deposit Account Not Found");
        //     if (depositAccount.DepositScheme.MinimumInterestRate > updateDepositAccount.InterestRate || updateDepositAccount.InterestRate > depositAccount.DepositScheme.MaximumInterestRate)
        //         throw new Exception("Interest Rate should be between Minimum Interest Rate and Maximum Interest Rate");

        //     depositAccount.Period = updateDepositAccount?.Period;
        //     depositAccount.PeriodType = (int)updateDepositAccount?.PeriodType;
        //     depositAccount.AccountType = (int)updateDepositAccount.AccountType;
        //     if (updateDepositAccount.JointClientId >= 1)
        //     {
        //         var jointClient = await _depositDbContext.Clients.FindAsync(updateDepositAccount.JointClientId);
        //         if (jointClient == null) throw new NotFoundExceptionHandler("Joint Client Not Found");
        //         depositAccount.JointClient = jointClient;
        //     }
        //     depositAccount.MatureDate = updateDepositAccount?.MatureDate;
        //     depositAccount.InterestAmount = updateDepositAccount.InterestRate;
        //     if (updateDepositAccount.MatureInterestPostingAccountNumber != null)
        //     {
        //         var checkMatureAccountNumber = await _depositDbContext.DepositAccounts.Where(da => da.AccountNumber == updateDepositAccount.MatureInterestPostingAccountNumber).SingleOrDefaultAsync();
        //         if (checkMatureAccountNumber == null) throw new NotFoundExceptionHandler("Provided Mature Interest Rate Posting Account Number Not Found");
        //         depositAccount.MatureInterestPostingAccountNumber = updateDepositAccount.MatureInterestPostingAccountNumber;
        //     }
        //     depositAccount.Description = updateDepositAccount.Description;
        //     depositAccount.Status = (int)updateDepositAccount.Status;
        //     depositAccount.IsSMSServiceActive = updateDepositAccount.IsSMSServiceActive;
        //     depositAccount.DailyDepositAmount = updateDepositAccount.DailyDepositAmount;
        //     depositAccount.TotalDepositDay = updateDepositAccount.TotalDepositDay;
        //     depositAccount.TotalDepositAmount = updateDepositAccount.TotalDepositAmount;
        //     depositAccount.TotalReturnAmount = updateDepositAccount.TotalReturnAmount;
        //     depositAccount.TotalInterestAmount = updateDepositAccount.TotalInterestAmount;
        //     depositAccount.ModifiedBy = modifiedBy;
        //     depositAccount.ModifiedOn = DateTime.Now;

        //     return await _depositDbContext.SaveChangesAsync();

        // }

        // public async Task<List<DepositAccount>> GetDepositAccountListAsync()
        // {
        //     var depositAccounts = await _depositDbContext.DepositAccounts
        //     .Include(da => da.DepositScheme)
        //     .ThenInclude(ds => ds.PostingScheme)
        //     .ToListAsync();
        //     return depositAccounts;
        // }

        // public async Task<DepositAccount> GetDepositAccountByAccountNumber(string accountNumber)
        // {
        //     var depositAccount = await _depositDbContext.DepositAccounts
        //     .Include(da => da.DepositScheme)
        //     .ThenInclude(ds => ds.PostingScheme)
        //     .Where(da => da.AccountNumber == accountNumber)
        //     .SingleOrDefaultAsync();
        //     return depositAccount;
        // }

        // public async Task<List<DepositAccount>> GetDepositAccountByDepositScheme(int depositSchemeId)
        // {
        //     var depositAccount = await _depositDbContext.DepositAccounts
        //     .Include(da => da.DepositScheme)
        //     .ThenInclude(ds => ds.PostingScheme)
        //     .Where(da => da.DepositSchemeId == depositSchemeId).ToListAsync();
        //     return depositAccount;
        // }

        public async Task<DepositAccount> GetDepositAccountById(int id)
        {
            var depositAccount = await _depositDbContext.DepositAccounts
            .Include(da => da.DepositScheme)
            .Where(da => da.Id == id)
            .SingleOrDefaultAsync();
            return depositAccount;
        }
        public async Task<DepositAccount> GetDepositAccountByDepositSchemeIdAndClientId(int depositSchemeId, int clientId)
        {
            return await _depositDbContext.DepositAccounts
            .Include(da=>da.DepositScheme)
            .Include(da=>da.Client)
            .Where
            (
                da=>da.DepositSchemeId==depositSchemeId 
                && da.ClientId==clientId 
                && da.Status!=AccountStatusEnum.Close
            )
            .SingleOrDefaultAsync();
        }

        // public async Task<int> CreateFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate)
        // {
        //     await _depositDbContext.FlexibleInterestRates.AddAsync(flexibleInterestRate);
        //     return await _depositDbContext.SaveChangesAsync();
        // }

        // public async Task<int> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate)
        // {
        //     _logger.LogInformation($"{DateTime.Now}: Updating Interest Rate According to Flexible Interest Rate...");
        //     var authorizedAccounts = await _depositDbContext.DepositAccounts
        //     .Where(
        //         da => da.DepositSchemeId == flexibleInterestRate.DepositSchemeId
        //         &&da.PrincipalAmount >= flexibleInterestRate.FromAmount
        //         && da.PrincipalAmount <= flexibleInterestRate.ToAmount
        //         )
        //     .ToListAsync();
        //     if(authorizedAccounts!=null && authorizedAccounts.Count>=1)
        //     {
        //         authorizedAccounts.ForEach(aa=>aa.InterestRate = flexibleInterestRate.InterestRate);
        //         return await _depositDbContext.SaveChangesAsync();
        //     }
        //     _logger.LogError($"{DateTime.Now}: No Records found that match the condition for upating interest rate according to flexible interest rate");
        //     return 0;
        // }

        // public async Task<int> IncrementOrDecrementOfInterestRate(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto)
        // {
        //     _logger.LogInformation($"{DateTime.Now}: incrementing or decrementing Interest Rate According to Deposit Scheme...");
        //     var authorizedAccounts = await _depositDbContext.DepositAccounts
        //     .Where(da=>da.DepositSchemeId==updateInterestRateByDepositSchemeDto.DepositSchemeId).ToListAsync();
        //     if(authorizedAccounts!=null && authorizedAccounts.Count>=1)
        //     {
        //         authorizedAccounts.ForEach(aa=>aa.InterestRate = aa.InterestRate+updateInterestRateByDepositSchemeDto.InterestRateChangeValue);
        //         return await _depositDbContext.SaveChangesAsync();
        //     }
        //     _logger.LogError($"{DateTime.Now}: No Records found that match the condition for increment or decrement of interest rate");
        //     return 0;

        // }

        // public async Task<int> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto)
        // {
        //     _logger.LogInformation($"{DateTime.Now}: changing Interest Rate According to past interest rate...");
        //     var authorizedAccounts = await _depositDbContext.DepositAccounts
        //     .Where(da=>
        //     da.DepositSchemeId == changeInterestRateByDepositSchemeDto.DepositSchemeId
        //     && da.InterestRate == changeInterestRateByDepositSchemeDto.OldInterestRate).ToListAsync();
        //     if(authorizedAccounts!=null && authorizedAccounts.Count>=1)
        //     {
        //         authorizedAccounts.ForEach(aa=>aa.InterestRate = changeInterestRateByDepositSchemeDto.NewInterestRate);
        //         return await _depositDbContext.SaveChangesAsync();
        //     }
        //     _logger.LogError($"{DateTime.Now}: No Records found that match old interest rate");
        //     return 0;

        // }
    }
}


