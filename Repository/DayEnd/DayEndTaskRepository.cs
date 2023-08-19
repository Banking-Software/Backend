using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Enums.Transaction;
using MicroFinance.Helper;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using MicroFinance.Repository.CompanyProfile;
using MicroFinance.Repository.Reports;
using Humanizer;
using MicroFinance.Repository.Transaction;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.DayEnd;

public class DayEndTaskRepository : IDayEndTaskRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DayEndTaskRepository> _logger;
    private readonly INepaliCalendarFormat _nepaliCalendarFormat;
    private readonly IDepositAccountTransactionRepository _depositAccountTransactionRepository;
    private DateTime todayCompanyDate;
    private bool isMatureTransaction;
    private bool isTaxDeduction;
    public DayEndTaskRepository
    (
        ApplicationDbContext dbContext,
        IDepositAccountTransactionRepository depositAccountTransactionRepository,
        ILogger<DayEndTaskRepository> logger,
        INepaliCalendarFormat nepaliCalendarFormat
    )
    {
        _dbContext = dbContext;
        _logger = logger;
        _nepaliCalendarFormat = nepaliCalendarFormat;
        _depositAccountTransactionRepository = depositAccountTransactionRepository;
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

    private async Task<List<DepositAccount>> GetTodayMaturingAccounts(DateTime currentCompanyDate)
    {
        var depositAccounts = await _dbContext.DepositAccounts
        .Include(da => da.Client)
        .Include(da => da.DepositScheme)
        .Include(da => da.MatureInterestPostingAccountNumber)
        .ThenInclude(mpa=>mpa.DepositScheme)
        .Where(da => da.Status != AccountStatusEnum.Mature && da.Status != AccountStatusEnum.Close
            && da.DepositScheme.IsActive && da.Client.IsActive && da.EnglishMatureDate <= currentCompanyDate)
        .AsNoTracking().ToListAsync();
        return depositAccounts;
    }
    private async Task<BaseTransaction> MakeBaseTransaction(decimal transactionAmount)
    {
        string nepaliTransactionDate = await _nepaliCalendarFormat.ConvertEnglishDateToNepali(todayCompanyDate);
        BaseTransaction baseTransaction = new BaseTransaction()
        {
            TransactionAmount = transactionAmount,
            Remarks = isMatureTransaction?TransactionRemarks.MatureInterestPosting.ToString():TransactionRemarks.InterestPosting.ToString(),
            NepaliCreationDate = nepaliTransactionDate,
            EnglishCreationDate = todayCompanyDate,
            RealWorldCreationDate = DateTime.Now,
            PaymentType = PaymentTypeEnum.Internal
        };
        return await _depositAccountTransactionRepository.BaseTransaction(baseTransaction, null, null, null);
    }
    private async Task<DepositAccount> MakeDepositAccountTransaction(BaseTransaction baseTransaction, InterestPostingWrapper interestPostingWrapper)
    {
        int firstHalfInterestAmonut = (int)interestPostingWrapper.TransactionAmount;
        int secondHalfInterestAmount = (int)((interestPostingWrapper.TransactionAmount - firstHalfInterestAmonut) * 10000);
        DepositAccountTransactionWrapper depositAccountTransactionWrapper = new()
        {
            TransactionAmount = interestPostingWrapper.TransactionAmount,
            AmountInWords = $"{firstHalfInterestAmonut.ToWords()} point {secondHalfInterestAmount.ToWords()}",
            DepositAccountId = interestPostingWrapper.ToAccountId,
            DepositSchemeId = interestPostingWrapper.SchemeId,
            DepositSchemeSubLedgerId = isTaxDeduction?(int)interestPostingWrapper.TaxSubLedgerId: (int)interestPostingWrapper.InterestSubLedgerId,
            TransactionType = interestPostingWrapper.TransactionType,
            WithDrawalType = isTaxDeduction ? WithDrawalTypeEnum.ByTax : null,
            Source = isTaxDeduction ? "TAX DEDUCTION" : "INTEREST POSTING",
            Narration = interestPostingWrapper.TransactionType == TransactionTypeEnum.Debit 
            ? 
            $"Tax deduction: Int Post {interestPostingWrapper.FromAccountNumber} to {interestPostingWrapper.ToAccountNumber}" 
            : $"Int Post {interestPostingWrapper.FromAccountNumber} to {interestPostingWrapper.ToAccountNumber}",
        };
        TransactionTypeEnum subLedgerTransactionType = isTaxDeduction?TransactionTypeEnum.Credit:interestPostingWrapper.TransactionType;
        return await _depositAccountTransactionRepository.BaseTransactionOnDepositAccount(depositAccountTransactionWrapper, baseTransaction, subLedgerTransactionType);
        
    }

    private async Task TransactionOnInterestPosting(BaseTransaction baseTransaction, DepositAccount depositAccount)
    {
        
        InterestPostingWrapper interestPostingWrapper = new()
        {
            FromAccountId = depositAccount.Id,
            FromAccountNumber = depositAccount.AccountNumber,
            TransactionDate = todayCompanyDate,
            TransactionAmount = baseTransaction.TransactionAmount,
            TransactionType = !isTaxDeduction?TransactionTypeEnum.Credit:TransactionTypeEnum.Debit
        };

        if(isMatureTransaction && depositAccount.MatureInterestPostingAccountNumber != null &&
            (depositAccount.MatureInterestPostingAccountNumber.Status == AccountStatusEnum.Active 
            || depositAccount.MatureInterestPostingAccountNumber.Status == AccountStatusEnum.Mature))
        {
            interestPostingWrapper.ToAccountId = depositAccount.MatureInterestPostingAccountNumber.Id;
            interestPostingWrapper.ToAccountNumber = depositAccount.MatureInterestPostingAccountNumber.AccountNumber;
            if(isTaxDeduction)
            {
                interestPostingWrapper.TaxSubLedgerId = depositAccount.MatureInterestPostingAccountNumber.DepositScheme.TaxSubledgerId;
            }
            else
                interestPostingWrapper.InterestSubLedgerId = depositAccount.MatureInterestPostingAccountNumber.DepositScheme.InterestSubLedgerId;
            interestPostingWrapper.SchemeId = depositAccount.MatureInterestPostingAccountNumber.DepositSchemeId;
        }
        else if(!isMatureTransaction && depositAccount.InterestPostingAccountNumber!=null &&
            (depositAccount.InterestPostingAccountNumber.Status== AccountStatusEnum.Active 
            || depositAccount.InterestPostingAccountNumber.Status== AccountStatusEnum.Mature))
        {
            interestPostingWrapper.ToAccountId = depositAccount.InterestPostingAccountNumber.Id;
            interestPostingWrapper.ToAccountNumber = depositAccount.InterestPostingAccountNumber.AccountNumber;
            if(isTaxDeduction)
            {
                interestPostingWrapper.TaxSubLedgerId = depositAccount.InterestPostingAccountNumber.DepositScheme.TaxSubledgerId;
            }
            else
                interestPostingWrapper.InterestSubLedgerId = depositAccount.InterestPostingAccountNumber.DepositScheme.InterestSubLedgerId;
            interestPostingWrapper.SchemeId = depositAccount.InterestPostingAccountNumber.DepositSchemeId;
        }
        else
        {
            interestPostingWrapper.ToAccountId = depositAccount.Id;
            interestPostingWrapper.ToAccountNumber = depositAccount.AccountNumber;
            if(isTaxDeduction)
            {
                interestPostingWrapper.TaxSubLedgerId = depositAccount.DepositScheme.TaxSubledgerId;
            }
            else
                interestPostingWrapper.InterestSubLedgerId = depositAccount.DepositScheme.InterestSubLedgerId;
            interestPostingWrapper.SchemeId = depositAccount.DepositSchemeId;
        }
        DepositAccount returnDepositAccount = await  MakeDepositAccountTransaction(baseTransaction, interestPostingWrapper);
        if(!isTaxDeduction && returnDepositAccount.Id==depositAccount.Id)
            returnDepositAccount.InterestAmount = 0;
        else if(!isTaxDeduction && returnDepositAccount.Id!=depositAccount.Id)
        {
            _dbContext.DepositAccounts.Attach(depositAccount);
            depositAccount.InterestAmount = 0;
            _dbContext.Entry(depositAccount).State = EntityState.Modified;
        }
          
    }

    private async Task BaseTransactionOnInterestPosting(DepositAccount depositAccount)
    {
        isTaxDeduction=false;
        BaseTransaction baseTransaction = await MakeBaseTransaction(depositAccount.InterestAmount);
        await TransactionOnInterestPosting(baseTransaction, depositAccount);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
    }

    private async Task BaseTransactionOnTaxDeduction(DepositAccount depositAccount)
    {
        isTaxDeduction = true;
        decimal taxPercentage = (await _dbContext.CompanyDetails.FirstOrDefaultAsync()).CurrentTax;
        decimal deductedTaxAmount = taxPercentage / 100 * depositAccount.InterestAmount;
        BaseTransaction baseTransaction = await MakeBaseTransaction(deductedTaxAmount);
        await TransactionOnInterestPosting(baseTransaction, depositAccount);
        await _dbContext.SaveChangesAsync();
        _dbContext.ChangeTracker.Clear();
    }

    
    private async Task TransactionsOnMatureAccount(List<DepositAccount> matureAccounts)
    {
        foreach (var account in matureAccounts)
        {
            isMatureTransaction = true;
            await BaseTransactionOnInterestPosting(account);
            await BaseTransactionOnTaxDeduction(account);
        }
    }

    public async Task<int> CheckMaturityOfAccountAndUpdate()
    {

        DateTime currentCompanyDate = await _nepaliCalendarFormat.GetCurrentCompanyDate();
        todayCompanyDate=currentCompanyDate;
        List<DepositAccount> matureAccounts = await GetTodayMaturingAccounts(currentCompanyDate);
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await TransactionsOnMatureAccount(matureAccounts);
            await transaction.CommitAsync();
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
        
        // Dictionary<int, DepositAccount> updatedMatureAccounts = await PostInterestForMaturingAccount(matureAccounts);
        // foreach (var kvp in updatedMatureAccounts)
        // {
        //     var account = kvp.Value;
        //     _dbContext.Entry(account).State = EntityState.Modified;
        // }
        // int numberOfAffectedRows = await _dbContext.SaveChangesAsync();
        //return numberOfAffectedRows;
        return 1;
    }

    public Task<int> InterestPositing()
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateCalendar()
    {
        throw new NotImplementedException();
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
                int matPostId = (int)accountToCheck.MatureInterestPostingAccountNumberId;
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




}