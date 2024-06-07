using System.Linq.Expressions;
using System.Runtime;
using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Models.AccountSetup;
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

        private async Task<List<SubLedger>> CreateSubLedgerForDepositScheme(List<string> subLedgerNamesForDepositScheme, Ledger depositLedger, string schemeName)
        {
            var depositSubledgerName = subLedgerNamesForDepositScheme[0];
            var interestSubledgerName = subLedgerNamesForDepositScheme[1];
            var taxSublegderName = subLedgerNamesForDepositScheme[2];
            // Ledger depsoitLedgerSchemeType = await _mainLedgerRepository.GetLedger(depositLedgerId);
            SubLedger depositSubledger = new SubLedger() { Name = depositSubledgerName, Ledger = depositLedger, Description = $"Subledger created while creating Deposit Scheme {schemeName}" };
            // Interest SubLedger
            Ledger interestLedger = await _depositDbContext.Ledgers.Where(l => l.LedgerCode == 68).SingleOrDefaultAsync(); //Interest Expenses
            SubLedger interestSubledger = new SubLedger() { Name = interestSubledgerName, Ledger = interestLedger, Description = $"Subledger created while creating Deposit Scheme {schemeName}" };
            // Tax SubLedger
            Ledger taxLedger = await _depositDbContext.Ledgers.Where(l => l.LedgerCode == 29).SingleOrDefaultAsync(); // Tax Payable
            SubLedger taxSubledger = new SubLedger() { Name = taxSublegderName, Ledger = taxLedger, Description = $"Subledger created while creating Deposit Scheme {schemeName}" };
            List<SubLedger> allSublegders = new List<SubLedger>() { depositSubledger, interestSubledger, taxSubledger };
            await _depositDbContext.SubLedgers.AddRangeAsync(allSublegders);
            var subLedgerStatus = await _depositDbContext.SaveChangesAsync();
            if (subLedgerStatus <= 0) throw new Exception("Unable to create subledgers for Deposit Scheme");
            foreach (var subLedger in allSublegders)
            {
                var updateStatus = await _mainLedgerRepository.UpdateSubLedgerCode(subLedger);
                if (updateStatus <= 0) throw new Exception($"Unable to Create '{subLedger.Name}' for deposit scheme");
            }
            return allSublegders;

        }
        public async Task<int> CreateDepositScheme(DepositScheme depositScheme, List<string> subLedgerNamesForDepositScheme)
        {
            using (var transaction = await _depositDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    depositScheme.SchemeType = await _depositDbContext.Ledgers.Where(l => l.LedgerCode == depositScheme.SchemeTypeId).SingleOrDefaultAsync();
                    List<SubLedger> allSubLedger = await CreateSubLedgerForDepositScheme(subLedgerNamesForDepositScheme, depositScheme.SchemeType, depositScheme.SchemeName);
                    depositScheme.DepositSubLedger = allSubLedger[0];
                    depositScheme.InterestSubledger = allSubLedger[1];
                    depositScheme.TaxSubledger = allSubLedger[2];
                    await _depositDbContext.DepositSchemes.AddAsync(depositScheme);
                    var createStatus = await _depositDbContext.SaveChangesAsync();
                    if (createStatus < 1) throw new Exception("Failed to create Scheme");
                    _logger.LogInformation($"{DateTime.Now}: Deposit Scheme {depositScheme.SchemeName} created by employee {depositScheme.CreatedBy}");
                    await transaction.CommitAsync();
                    _depositDbContext.ChangeTracker.Clear();
                    return depositScheme.Id;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{DateTime.Now}: Deposit Scheme {depositScheme.SchemeName} failed created by employee {depositScheme.CreatedBy}");
                    _logger.LogInformation($"{DateTime.Now}: Removing created subledger from the given scheme...");
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
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
            return await _depositDbContext.DepositSchemes.Where(ds => ds.Symbol == symbol).SingleOrDefaultAsync();
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
            .AsNoTracking()
            .SingleOrDefaultAsync();
            return depositScheme;

        }


        // DEPOSIT ACCOUNT
        private async Task<DepositAccount> CreateDepositAccountNumber(DepositAccount depositAccount)
        {
            var existingDepositAccount = await _depositDbContext.DepositAccounts.FindAsync(depositAccount.Id);
            _depositDbContext.Entry(existingDepositAccount).State = EntityState.Detached;

            _depositDbContext.DepositAccounts.Attach(depositAccount);
            depositAccount.AccountNumber =
            depositAccount.DepositScheme.Symbol +
            depositAccount.BranchCode +
            depositAccount.Id.ToString().PadLeft(5, '0');
            _depositDbContext.Entry(depositAccount).State = EntityState.Modified;
            var updateStatus = await _depositDbContext.SaveChangesAsync();
            if (updateStatus < 1) throw new Exception("Unable to Create Deposit Account");
            return depositAccount;
        }

        private async Task InsertPostingAccounts(DepositAccount depositAccount)
        {
            if
            (
                depositAccount.MatureInterestPostingAccountNumberId != null
                && depositAccount.InterestPostingAccountNumberId != null
                && depositAccount.MatureInterestPostingAccountNumberId == depositAccount.InterestPostingAccountNumberId
            )
            {
                var postingAccount = await _depositDbContext.DepositAccounts.FindAsync(depositAccount.MatureInterestPostingAccountNumberId);
                depositAccount.MatureInterestPostingAccountNumber = postingAccount;
                depositAccount.InterestPostingAccountNumber = postingAccount;
            }
            else if (depositAccount.MatureInterestPostingAccountNumberId != null)
                depositAccount.MatureInterestPostingAccountNumber = await _depositDbContext.DepositAccounts.FindAsync(depositAccount.MatureInterestPostingAccountNumberId);
            else if (depositAccount.InterestPostingAccountNumberId != null)
                depositAccount.InterestPostingAccountNumber = await _depositDbContext.DepositAccounts.FindAsync(depositAccount.InterestPostingAccountNumber);
        }
        public async Task<int> CreateDepositAccount(DepositAccount depositAccount, CreateDepositAccountDto createDepositAccountDto)
        {
            using var transaction = await _depositDbContext.Database.BeginTransactionAsync();
            try
            {
                _depositDbContext.ChangeTracker.Clear();
                depositAccount.Client = await _depositDbContext.Clients.FindAsync(depositAccount.ClientId);
                depositAccount.DepositScheme = await _depositDbContext.DepositSchemes.FindAsync(depositAccount.DepositSchemeId);
                await InsertPostingAccounts(depositAccount);
                await _depositDbContext.DepositAccounts.AddAsync(depositAccount);
                var status = await _depositDbContext.SaveChangesAsync();
                if (status < 1) throw new Exception("Unable to Create Deposit Account");
                depositAccount = await CreateDepositAccountNumber(depositAccount);
                if(createDepositAccountDto.AccountType==AccountTypeEnum.Joint && createDepositAccountDto.JointClientIds.Count>0)
                    await CreateJointClientForAnAccount(createDepositAccountDto.JointClientIds, depositAccount);
                await transaction.CommitAsync();
                return status;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }

        }

        private async Task CreateJointClientForAnAccount(List<int> jointClientIds, DepositAccount depositAccount)
        {
            var jointClients = await _depositDbContext.Clients.Where(client => jointClientIds.Contains(client.Id)).ToListAsync();
            var jointAccounts = await CreateJointAccount(jointClients, depositAccount);
            await _depositDbContext.JointAccounts.AddRangeAsync(jointAccounts);
            var addStatus = await _depositDbContext.SaveChangesAsync();
            if (addStatus < 1) throw new Exception("Unable to Create Joint Account");
            return;
        }
        private async Task<List<JointAccount>> CreateJointAccount(List<Client> jointClients, DepositAccount depositAccount)
        {
            List<JointAccount> jointAccounts = new List<JointAccount>();
            DateTime RealWorldStartDate = DateTime.Now;
            string accountCreationDateInNepali = depositAccount.NepaliCreationDate;
            foreach (var jointClient in jointClients)
            {
                var jointAccount = new JointAccount()
                {
                    JointClient = jointClient,
                    DepositAccount = depositAccount,
                    RealWorldStartDate = RealWorldStartDate,
                    CompanyCalendarStartDate = accountCreationDateInNepali
                };
                jointAccounts.Add(jointAccount);
            }
            return jointAccounts;
        }


        public async Task<int> UpdateDepositAccount(DepositAccount updateDepositAccount)
        {
            _depositDbContext.ChangeTracker.Clear();
            _depositDbContext.DepositAccounts.Attach(updateDepositAccount);
            await InsertPostingAccounts(updateDepositAccount);
            _depositDbContext.Entry(updateDepositAccount).State = EntityState.Modified;
            return await _depositDbContext.SaveChangesAsync();
        }
        public async Task<List<DepositAccountWrapper>> GetAllDepositAccountsWrapper(Expression<Func<DepositAccount, bool>> expression)
        {
            var depositAccountWrappers = await _depositDbContext.DepositAccounts
             .Include(da => da.Client)
             .Include(da => da.DepositScheme)
             .Include(da => da.InterestPostingAccountNumber)
             .Include(da => da.InterestPostingAccountNumber)
             .Where(expression)
             .Select(da => new DepositAccountWrapper
             {
                 DepositAccount = da,
                 JointAccount = _depositDbContext.JointAccounts
                     .Include(ja => ja.JointClient)
                     .Where(ja => ja.DepositAccountId == da.Id && ja.RealWorldEndDate == null && ja.CompanyCalendarEndDate == null)
                     .ToList()
             })
             .AsNoTracking()
             .ToListAsync();
            return depositAccountWrappers;
        }
        public async Task<DepositAccountWrapper> GetDepositAccountWrapper(Expression<Func<DepositAccount, bool>> expression)
        {
            var depositAccountWrappers = await _depositDbContext.DepositAccounts
             .Include(da => da.Client)
             .Include(da => da.DepositScheme)
             .Include(da => da.InterestPostingAccountNumber)
             .Include(da => da.MatureInterestPostingAccountNumber)
             .Where(expression)
             .Select(da => new DepositAccountWrapper
             {
                 DepositAccount = da,
                 JointAccount = _depositDbContext.JointAccounts
                     .Include(ja => ja.JointClient)
                     .Where(ja => ja.DepositAccountId == da.Id && ja.RealWorldEndDate == null && ja.CompanyCalendarEndDate == null)
                     .ToList()
             })
             .AsNoTracking()
             .SingleOrDefaultAsync();
            return depositAccountWrappers;

        }

        public async Task<DepositAccount> GetDepositAccount(Expression<Func<DepositAccount, bool>> expression)
        {
            var depositAccount = await _depositDbContext.DepositAccounts
            .Include(da => da.Client)
            .Include(da => da.DepositScheme)
            .Include(da => da.InterestPostingAccountNumber)
            .Include(da => da.InterestPostingAccountNumber)
            .Where(expression)
            .AsNoTracking()
            .SingleOrDefaultAsync();
            return depositAccount;
        }
        // public async Task<DepositAccount> GetDepositAccountByDepositSchemeIdAndClientId(int depositSchemeId, int clientId)
        // {
        //     return await _depositDbContext.DepositAccounts
        //     .Include(da => da.DepositScheme)
        //     .Include(da => da.Client)
        //     .Where
        //     (
        //         da => da.DepositSchemeId == depositSchemeId
        //         && da.ClientId == clientId
        //         && da.Status != AccountStatusEnum.Close
        //     )
        //     .AsNoTracking()
        //     .SingleOrDefaultAsync();
        // }

        // public async Task<DepositAccountWrapper> GetNonClosedDepositAccountByAccountNumber(string accountNumber)
        // {
        //     var depositAccountWrappers = await _depositDbContext.DepositAccounts
        //     .Include(da => da.Client)
        //     .Include(da => da.DepositScheme)
        //     .Include(da => da.InterestPostingAccountNumber)
        //     .Include(da => da.InterestPostingAccountNumber)
        //     .Where(da => da.Status != AccountStatusEnum.Close && da.AccountNumber == accountNumber)
        //     .Select(da => new DepositAccountWrapper
        //     {
        //         DepositAccount = da,
        //         JointAccount = _depositDbContext.JointAccounts
        //             .Include(ja => ja.JointClient)
        //             .Where(ja => ja.DepositAccountId == da.Id && ja.RealWorldEndDate == null && ja.CompanyCalendarEndDate == null)
        //             .ToList()
        //     })
        //     .AsNoTracking()
        //     .SingleOrDefaultAsync();
        //     return depositAccountWrappers;
        // }

        // public async Task<List<DepositAccountWrapper>> GetNonClosedDepositAccountByDepositScheme(int depositSchemeId)
        // {
        //     var depositAccountWrappers = await _depositDbContext.DepositAccounts
        //     .Include(da => da.Client)
        //     .Include(da => da.DepositScheme)
        //     .Include(da => da.InterestPostingAccountNumber)
        //     .Include(da => da.InterestPostingAccountNumber)
        //     .Where(da => da.Status != AccountStatusEnum.Close && da.DepositSchemeId == depositSchemeId)
        //     .Select(da => new DepositAccountWrapper
        //     {
        //         DepositAccount = da,
        //         JointAccount = _depositDbContext.JointAccounts
        //             .Include(ja => ja.JointClient)
        //             .Where(ja => ja.DepositAccountId == da.Id && ja.RealWorldEndDate == null && ja.CompanyCalendarEndDate == null)
        //             .ToList()
        //     })
        //     .AsNoTracking()
        //     .ToListAsync();
        //     return depositAccountWrappers;
        // }
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


