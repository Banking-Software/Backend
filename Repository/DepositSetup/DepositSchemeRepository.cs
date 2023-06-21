using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Exceptions;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
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
            await _depositDbContext.DepositSchemes.AddAsync(depositScheme);
            return await _depositDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateDepositScheme(UpdateDepositScheme depositScheme)
        {
            var existingDepositScheme = await _depositDbContext.DepositSchemes.FindAsync(depositScheme.Id);
            if (existingDepositScheme != null)
            {
                existingDepositScheme.MinimumBalance = depositScheme.MinimumBalance;
                existingDepositScheme.InterestRate = depositScheme.InterestRate;
                existingDepositScheme.MinimumInterestRate = depositScheme.MinimumInterestRate;
                existingDepositScheme.MaximumInterestRate = depositScheme.MaximumInterestRate;
                return await _depositDbContext.SaveChangesAsync();
            }
            throw new NotImplementedException("Data doesn't exist");
        }

        public async Task<DepositScheme> GetDepositSchemeByName(string name)
        {
            var depositScheme = await _depositDbContext
            .DepositSchemes
            .Where(ds => ds.Name == name)
            .SingleOrDefaultAsync();
            return depositScheme;
        }

        public async Task<List<ResponseDepositScheme>> GetAllDepositScheme()
        {
            var depositScheme = await _depositDbContext.DepositSchemes.Include(ds => ds.PostingScheme).ToListAsync();
            var depositSchemeWithLedgerDetails = new List<ResponseDepositScheme>();
            foreach (var scheme in depositScheme)
            {
                ResponseDepositScheme temp = _mapper.Map<ResponseDepositScheme>(scheme);
                var ledgerAsInterest = await _mainLedgerRepository.GetLedger(scheme.LedgerAsInterestAccountId);
                var ledgerAsLiability = await _mainLedgerRepository.GetLedger(scheme.LedgerAsLiabilityAccountId);
                //await Task.WhenAll(ledgerAsInterest, ledgerAsLiability);
                temp.InterestAccount = ledgerAsInterest;
                temp.LiabilityAccount = ledgerAsLiability;
                depositSchemeWithLedgerDetails.Add(temp);

            }
            return depositSchemeWithLedgerDetails;
        }

        public async Task<DepositScheme> GetDepositScheme(int id)
        {
            var depositScheme = await _depositDbContext
            .DepositSchemes
            .Include(ds => ds.PostingScheme)
            .Where(ds => ds.Id == id)
            .SingleOrDefaultAsync();
            return depositScheme;

        }

        public async Task<List<ResponseDepositScheme>> GetDepositSchemeByPostingScheme(int id)
        {
            var depositSchemesByPostingScheme = await _depositDbContext
            .DepositSchemes
            .Include(ds => ds.PostingScheme)
            .Where(ds => ds.PostingSchemeId == id)
            .ToListAsync();

            var depositSchemeWithLedgerDetails = new List<ResponseDepositScheme>();
            foreach (var scheme in depositSchemesByPostingScheme)
            {
                ResponseDepositScheme temp = _mapper.Map<ResponseDepositScheme>(scheme);
                var ledgerAsInterest = await _mainLedgerRepository.GetLedger(scheme.LedgerAsInterestAccountId);
                var ledgerAsLiability = await _mainLedgerRepository.GetLedger(scheme.LedgerAsLiabilityAccountId);
                //await Task.WhenAll(ledgerAsInterest, ledgerAsLiability);
                temp.InterestAccount = ledgerAsInterest;
                temp.LiabilityAccount = ledgerAsLiability;
                depositSchemeWithLedgerDetails.Add(temp);
            }
            return depositSchemeWithLedgerDetails;
        }

        public async Task<PostingScheme> GetPositingScheme(int id)
        {
            var positngScheme = await _depositDbContext
            .PostingSchemes
            .FindAsync(id);

            return positngScheme;
        }


        // DEPOSIT ACCOUNT

        public async Task<int> CreateDepositAccount(DepositAccount depositAccount)
        {
            await _depositDbContext.DepositAccounts.AddAsync(depositAccount);
            var status = await _depositDbContext.SaveChangesAsync();
            return status;
        }

        public async Task<int> UpdateDepositAccount(UpdateDepositAccountDto updateDepositAccount, string modifiedBy)
        {
            var depositAccount = await _depositDbContext.DepositAccounts.Include(da => da.DepositScheme).Where(da => da.Id == updateDepositAccount.Id).SingleOrDefaultAsync();
            if (depositAccount == null) throw new NotFoundExceptionHandler("Given Deposit Account Not Found");
            if (depositAccount.DepositScheme.MinimumInterestRate > updateDepositAccount.InterestRate || updateDepositAccount.InterestRate > depositAccount.DepositScheme.MaximumInterestRate)
                throw new Exception("Interest Rate should be between Minimum Interest Rate and Maximum Interest Rate");

            depositAccount.Period = updateDepositAccount?.Period;
            depositAccount.PeriodType = (int)updateDepositAccount?.PeriodType;
            depositAccount.AccountType = (int)updateDepositAccount.AccountType;
            if (updateDepositAccount.JointClientId >= 1)
            {
                var jointClient = await _depositDbContext.Clients.FindAsync(updateDepositAccount.JointClientId);
                if (jointClient == null) throw new NotFoundExceptionHandler("Joint Client Not Found");
                depositAccount.JointClient = jointClient;
            }
            depositAccount.MatureDate = updateDepositAccount?.MatureDate;
            depositAccount.InterestAmount = updateDepositAccount.InterestRate;
            if (updateDepositAccount.MatureInterestPostingAccountNumber != null)
            {
                var checkMatureAccountNumber = await _depositDbContext.DepositAccounts.Where(da => da.AccountNumber == updateDepositAccount.MatureInterestPostingAccountNumber).SingleOrDefaultAsync();
                if (checkMatureAccountNumber == null) throw new NotFoundExceptionHandler("Provided Mature Interest Rate Posting Account Number Not Found");
                depositAccount.MatureInterestPostingAccountNumber = updateDepositAccount.MatureInterestPostingAccountNumber;
            }
            depositAccount.Description = updateDepositAccount.Description;
            depositAccount.Status = (int)updateDepositAccount.Status;
            depositAccount.IsSMSServiceActive = updateDepositAccount.IsSMSServiceActive;
            depositAccount.DailyDepositAmount = updateDepositAccount.DailyDepositAmount;
            depositAccount.TotalDepositDay = updateDepositAccount.TotalDepositDay;
            depositAccount.TotalDepositAmount = updateDepositAccount.TotalDepositAmount;
            depositAccount.TotalReturnAmount = updateDepositAccount.TotalReturnAmount;
            depositAccount.TotalInterestAmount = updateDepositAccount.TotalInterestAmount;
            depositAccount.ModifiedBy = modifiedBy;
            depositAccount.ModifiedOn = DateTime.Now;

            return await _depositDbContext.SaveChangesAsync();

        }

        public async Task<List<DepositAccount>> GetDepositAccountListAsync()
        {
            var depositAccounts = await _depositDbContext.DepositAccounts
            .Include(da => da.DepositScheme)
            .ThenInclude(ds => ds.PostingScheme)
            .ToListAsync();
            return depositAccounts;
        }

        public async Task<DepositAccount> GetDepositAccountByAccountNumber(string accountNumber)
        {
            var depositAccount = await _depositDbContext.DepositAccounts
            .Include(da => da.DepositScheme)
            .ThenInclude(ds => ds.PostingScheme)
            .Where(da => da.AccountNumber == accountNumber)
            .SingleOrDefaultAsync();
            return depositAccount;
        }

        public async Task<List<DepositAccount>> GetDepositAccountByDepositScheme(int depositSchemeId)
        {
            var depositAccount = await _depositDbContext.DepositAccounts
            .Include(da => da.DepositScheme)
            .ThenInclude(ds => ds.PostingScheme)
            .Where(da => da.DepositSchemeId == depositSchemeId).ToListAsync();
            return depositAccount;
        }

        public async Task<DepositAccount> GetDepositAccountById(int id)
        {
            var depositAccount = await _depositDbContext.DepositAccounts
            .Include(da => da.DepositScheme)
            .ThenInclude(ds => ds.PostingScheme)
            .Where(da => da.Id == id)
            .SingleOrDefaultAsync();
            return depositAccount;
        }

        public async Task<int> CreateFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate)
        {
            await _depositDbContext.FlexibleInterestRates.AddAsync(flexibleInterestRate);
            return await _depositDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRate flexibleInterestRate)
        {
            _logger.LogInformation($"{DateTime.Now}: Updating Interest Rate According to Flexible Interest Rate...");
            var authorizedAccounts = await _depositDbContext.DepositAccounts
            .Where(
                da => da.DepositSchemeId == flexibleInterestRate.DepositSchemeId
                &&da.PrincipalAmount >= flexibleInterestRate.FromAmount
                && da.PrincipalAmount <= flexibleInterestRate.ToAmount
                )
            .ToListAsync();
            if(authorizedAccounts!=null && authorizedAccounts.Count>=1)
            {
                authorizedAccounts.ForEach(aa=>aa.InterestRate = flexibleInterestRate.InterestRate);
                return await _depositDbContext.SaveChangesAsync();
            }
            _logger.LogError($"{DateTime.Now}: No Records found that match the condition for upating interest rate according to flexible interest rate");
            return 0;
        }

        public async Task<int> IncrementOrDecrementOfInterestRate(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto)
        {
            _logger.LogInformation($"{DateTime.Now}: incrementing or decrementing Interest Rate According to Deposit Scheme...");
            var authorizedAccounts = await _depositDbContext.DepositAccounts
            .Where(da=>da.DepositSchemeId==updateInterestRateByDepositSchemeDto.DepositSchemeId).ToListAsync();
            if(authorizedAccounts!=null && authorizedAccounts.Count>=1)
            {
                authorizedAccounts.ForEach(aa=>aa.InterestRate = aa.InterestRate+updateInterestRateByDepositSchemeDto.InterestRateChangeValue);
                return await _depositDbContext.SaveChangesAsync();
            }
            _logger.LogError($"{DateTime.Now}: No Records found that match the condition for increment or decrement of interest rate");
            return 0;

        }

        public async Task<int> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto)
        {
            _logger.LogInformation($"{DateTime.Now}: changing Interest Rate According to past interest rate...");
            var authorizedAccounts = await _depositDbContext.DepositAccounts
            .Where(da=>
            da.DepositSchemeId == changeInterestRateByDepositSchemeDto.DepositSchemeId
            && da.InterestRate == changeInterestRateByDepositSchemeDto.OldInterestRate).ToListAsync();
            if(authorizedAccounts!=null && authorizedAccounts.Count>=1)
            {
                authorizedAccounts.ForEach(aa=>aa.InterestRate = changeInterestRateByDepositSchemeDto.NewInterestRate);
                return await _depositDbContext.SaveChangesAsync();
            }
            _logger.LogError($"{DateTime.Now}: No Records found that match old interest rate");
            return 0;

        }
    }
}


