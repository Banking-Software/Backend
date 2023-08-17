using MicroFinance.DBContext;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Helper;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Repository.CompanyProfile;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.DayEndTaskRepository;

public class DayEndTaskRepository : IDayEndTaskRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DayEndTaskRepository> _logger;
    private readonly ICompanyProfileRepository _companyProfileRepository;
    private readonly INepaliCalendarFormat _nepaliCalendarFormat;

    public DayEndTaskRepository
    (
        ApplicationDbContext dbContext,
        ICompanyProfileRepository companyProfileRepository,
        ILogger<DayEndTaskRepository> logger,
        INepaliCalendarFormat nepaliCalendarFormat
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _companyProfileRepository = companyProfileRepository;
        _nepaliCalendarFormat = nepaliCalendarFormat;
    }
    public async Task<int> CalculateDailyInterest()
    {
        _logger.LogInformation($"{DateTime.Now}: Interest Calculation Started");
        _dbContext.ChangeTracker.Clear();
        var depositAccounts = await _dbContext.DepositAccounts
        .Include(da => da.DepositScheme)
        .Include(da => da.Client)
        .Where(
            da =>
            (da.Status == AccountStatusEnum.Active || da.Status == AccountStatusEnum.Suspend)
            &&
            da.DepositScheme.IsActive && da.Client.IsActive
        ).ToListAsync();

        foreach (var account in depositAccounts)
        {
            decimal interestRate = account.InterestRate;
            decimal principalAmount = account.PrincipalAmount;
            account.InterestAmount += interestRate / 100 * principalAmount;
        }
        var numberAccountUpdated = await _dbContext.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now}: Interest Calculation Ended. Updated Entries are {numberAccountUpdated}");
        return numberAccountUpdated;
    }

    private async Task<List<DepositAccount>> GetTodayMaturingAccount(DateTime currentCompanyDate)
    {
        var matureAccounts = await _dbContext.DepositAccounts
        .Include(da => da.DepositScheme)
        .Include(da => da.Client)
        .Where
        (
            da => da.Status != AccountStatusEnum.Mature
            && da.Status != AccountStatusEnum.Close
            && da.DepositScheme.IsActive
            && da.Client.IsActive
            && da.EnglishMatureDate <= currentCompanyDate
        ).AsNoTracking().ToListAsync();
        return matureAccounts;
    }

    private async Task<Dictionary<int, DepositAccount>> PostInterestForMaturingAccount(List<DepositAccount> matureAccounts)
    {
        Dictionary<int, DepositAccount> updatedAccounts = new();

        foreach (var account in matureAccounts)
        {
            var accountToCheck = updatedAccounts.ContainsKey(account.Id) ? updatedAccounts[account.Id] : account;
            decimal postingInterestAmount = accountToCheck.InterestAmount;
            bool hasMatureInterestPostingAccountNumber = accountToCheck.MatureInterestPostingAccountNumberId != null
                                                         && accountToCheck.MatureInterestPostingAccountNumberId >= 1;
            if (hasMatureInterestPostingAccountNumber)
            {
                int matPostId = (int) accountToCheck.MatureInterestPostingAccountNumberId;
                DepositAccount matureInterestPostingAccount = updatedAccounts.ContainsKey(matPostId) ?
                updatedAccounts[matPostId]
                :
                await _dbContext.DepositAccounts.Include(ma => ma.DepositScheme).Include(ma => ma.Client)
                .Where(ma => ma.Id == matPostId).AsNoTracking().SingleOrDefaultAsync();

                bool matureAccountPosConditionMatch = matureInterestPostingAccount != null
                && (matureInterestPostingAccount.Status == AccountStatusEnum.Active || matureInterestPostingAccount.Status == AccountStatusEnum.Mature)
                && matureInterestPostingAccount.DepositScheme.IsActive && matureInterestPostingAccount.Client.IsActive;

                if (matureAccountPosConditionMatch)
                {
                    matureInterestPostingAccount.PrincipalAmount += postingInterestAmount;
                    if (!updatedAccounts.ContainsKey(matureInterestPostingAccount.Id))
                        updatedAccounts.Add(matureInterestPostingAccount.Id, matureInterestPostingAccount);
                }
                else
                {
                    accountToCheck.PrincipalAmount += postingInterestAmount;
                }
            }
            else
            {
                accountToCheck.PrincipalAmount += postingInterestAmount;
            }
            accountToCheck.Status = AccountStatusEnum.Mature;
            accountToCheck.InterestAmount = 0;
            if (!updatedAccounts.ContainsKey(accountToCheck.Id))
                updatedAccounts.Add(accountToCheck.Id, accountToCheck);
        }
        return updatedAccounts;
    }

    public async Task<int> CheckMaturityOfAccountAndUpdate()
    {
        DateTime currentCompanyDate = await _nepaliCalendarFormat.GetCurrentCompanyDate();
        List<DepositAccount> matureAccounts = await GetTodayMaturingAccount(currentCompanyDate);
        Dictionary<int, DepositAccount> updatedMatureAccounts = await PostInterestForMaturingAccount(matureAccounts); 
        foreach (var kvp in updatedMatureAccounts)
        {
            var account = kvp.Value;
            _dbContext.Entry(account).State = EntityState.Modified;
        }
        int numberOfAffectedRows =  await _dbContext.SaveChangesAsync();
        return numberOfAffectedRows;
    }

    public Task<int> InterestPositing()
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateCalendar()
    {
        throw new NotImplementedException();
    }
}