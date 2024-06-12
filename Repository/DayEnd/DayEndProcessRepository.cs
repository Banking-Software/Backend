using MicroFinance.DBContext;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Enums.Transaction;
using MicroFinance.Helpers;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper.DayEnd;
using MicroFinance.Repository.Transaction;
using MicroFinance.Services.CompanyProfile;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.DayEnd
{
    public class DayEndProcessRepository : IDayEndTaskRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DayEndProcessRepository> _logger;
        private readonly ICommonExpression _commonExpression;
        private readonly ICompanyProfileService _companyProfileService;
        private readonly ITransactions _transactions;
        private readonly IHelper _helper;
        private readonly SemaphoreSlim dayEndLock = new SemaphoreSlim(1, 1);
        private Dictionary<int, DepositAccount> accountsForInterestPosting = new();
        private Dictionary<int, SubLedger> subLedgerForInterestPosting = new();
        private Dictionary<int, Ledger> ledgerForInterestPosting = new();
        private Dictionary<string, TransactionVoucher> branchCodeWiseVoucherNumber = new();
        private int transactionVoucherCount = 0;



        public DayEndProcessRepository
        (
            ApplicationDbContext dbContext,
            ICommonExpression commonExpression,
            ICompanyProfileService companyProfileService,
            ITransactions transactions,
            IHelper helper,
            ILogger<DayEndProcessRepository> logger
         )
        {
            _dbContext = dbContext;
            _logger = logger;
            _commonExpression = commonExpression;
            _companyProfileService = companyProfileService;
            _transactions = transactions;
            _helper = helper;
        }

        private async Task<List<DepositAccount>> GetAccountForInterestCalculation()
        {
            var depositAccounts = await _dbContext.DepositAccounts
            .Include(da => da.DepositScheme)
            .Include(da => da.Client)
            .Where(
                da =>
                (da.Status == AccountStatusEnum.Active || da.Status == AccountStatusEnum.Suspend)
                &&
                da.DepositScheme.IsActive && da.Client.IsActive && da.PrincipalAmount > 0
            ).ToListAsync();
            return depositAccounts;
        }
        public async Task<int> CalculateDailyInterest()
        {
            if (!await dayEndLock.WaitAsync(TimeSpan.FromMinutes(0)))
                throw new Exception("Already one process is running");

            try
            {
                _logger.LogInformation($"{DateTime.Now}: Interest Calculation Started...");
                DateTime currentCompanyDate = await GetCompanyActiveCalendarInAD();
                List<DepositAccount> depositAccounts = await GetAccountForInterestCalculation();
                if (depositAccounts != null && depositAccounts.Count >= 1)
                {
                    foreach (var account in depositAccounts)
                    {
                        decimal interestRate = account.InterestRate;
                        decimal principalAmount = account.PrincipalAmount;
                        decimal perDayInterest = (interestRate * principalAmount) / 36500;
                        account.InterestAmount += perDayInterest; // (/100 and / 365)
                        TransactionVoucher transactionVoucher = null;
                        if (branchCodeWiseVoucherNumber.ContainsKey(account.BranchCode))
                            transactionVoucher = branchCodeWiseVoucherNumber[account.BranchCode];
                        else
                        {
                            transactionVoucher = await GenerateTransactionVoucher(account.BranchCode);
                            branchCodeWiseVoucherNumber.Add(account.BranchCode, transactionVoucher);
                        }
                        BaseTransaction baseTransaction = await MakeBaseTransaction(account.BranchCode, perDayInterest, transactionVoucher, TypeOfDayEndTaskEnum.InterestCalculation, currentCompanyDate);
                        InterestTransaction interestTransaction = new InterestTransaction()
                        {
                            Transaction = baseTransaction,
                            CalculatedInterestRate = account.InterestRate,
                            CalculatedInterestAmount = account.InterestAmount,
                            DepositAccount = account
                        };
                        await _dbContext.InterestTransactions.AddAsync(interestTransaction);
                        _logger.LogInformation($"Interest Transaction of {perDayInterest} added to the account {account.AccountNumber} making total interest amount of {account.InterestAmount}");
                    }
                    // await _dbContext.InterestTransactions.AddRangeAsync(allInterestTransaction);
                    await _dbContext.SaveChangesAsync();
                }
                _logger.LogInformation($"{DateTime.Now}: Interest Calculation Ended...");
                return depositAccounts.Count;

            }
            finally
            {
                dayEndLock.Release();
            }
        }

        public async Task<int> CheckInterestPositingAndUpdate()
        {
            DateTime currentCompanyActiveDate = await GetCompanyActiveCalendarInAD();
            List<DepositAccount> validAccountsForInterestPosting = await GetTodayInterestPostingAccount(currentCompanyActiveDate);
            if (validAccountsForInterestPosting != null && validAccountsForInterestPosting.Count > 0)
            {
                _logger.LogInformation($"{DateTime.Now} Interest Posting Operation started...");
                await PerformPostingTasks(validAccountsForInterestPosting, currentCompanyActiveDate, TypeOfDayEndTaskEnum.InterestPosting);
                _logger.LogInformation($"{DateTime.Now}: InterestPositing Operation Ended...");
            }
            return validAccountsForInterestPosting.Count;
        }


        public async Task<int> CheckMaturityOfAccountAndUpdate()
        {
            DateTime currentCompanyActiveDate = await GetCompanyActiveCalendarInAD();
            List<DepositAccount> validAccountsForMature = await GetTodayMaturingAccounts(currentCompanyActiveDate);
            if (validAccountsForMature != null && validAccountsForMature.Count > 0)
            {
                _logger.LogInformation($"{DateTime.Now} Mature Interest Posting Operation started...");
                await PerformPostingTasks(validAccountsForMature, currentCompanyActiveDate, TypeOfDayEndTaskEnum.MatureInterestPosting);
                _logger.LogInformation($"{DateTime.Now}: Mature Interest Positing Operation Ended...");

            }
            return validAccountsForMature.Count;
        }

        public async Task<string> UpdateCalendar()
        {
            if (!await dayEndLock.WaitAsync(TimeSpan.FromMinutes(0)))
                throw new Exception("Already a process is running...");
            try
            {
                _logger.LogInformation($"{DateTime.Now}: Calendar updating to next day...");
                var currentActiveCalendar = await _dbContext.Calendars.Where(c => c.IsActive).SingleOrDefaultAsync();
                string activeNepaliDate = await _helper.GetNepaliFormatDate(currentActiveCalendar.Year, currentActiveCalendar.Month, currentActiveCalendar.RunningDay);
                DateTime englishActiveDate = await _helper.ConvertNepaliDateToEnglish(activeNepaliDate);
                _logger.LogInformation($"Current Calendar is Found as {activeNepaliDate} B.S ({englishActiveDate} A.D)");
                DateTime nextActiveEnglishDate = englishActiveDate.AddDays(1);
                string nextNepaliActiveDate = await _helper.ConvertEnglishDateToNepali(nextActiveEnglishDate);
                List<int> nextYearMonthDay = await _helper.GetYearMonthDayFromStringDate(nextNepaliActiveDate);
                Models.CompanyProfile.Calendar nextActiveCalendar = null;
                if (currentActiveCalendar.Year == nextYearMonthDay[0] && currentActiveCalendar.Month == nextYearMonthDay[1])
                {
                    if (nextYearMonthDay[2] <= currentActiveCalendar.NumberOfDay)
                        currentActiveCalendar.RunningDay = nextYearMonthDay[2];
                    else
                    {
                        int year = currentActiveCalendar.Year;
                        int month = currentActiveCalendar.Month;
                        if (nextYearMonthDay[2] > currentActiveCalendar.NumberOfDay && currentActiveCalendar.Month == 12)
                        {
                            year += 1;
                            month = 1;
                        }
                        else
                            month += 1;
                        nextActiveCalendar = await _dbContext.Calendars.Where(
                            c => c.Year == year
                            && c.Month == month
                            && c.IsActive == false && c.IsLocked == false
                        ).SingleOrDefaultAsync();
                        if (nextActiveCalendar == null) throw new Exception("No Calendar Found");
                        currentActiveCalendar.IsActive = false;
                        nextActiveCalendar.IsActive = true;
                        nextActiveCalendar.IsLocked = true;
                    }
                }
                else
                {
                    nextActiveCalendar = await _dbContext.Calendars.Where(
                        c => c.Year == nextYearMonthDay[0]
                        && c.Month == nextYearMonthDay[1]
                        && c.IsActive == false && c.IsLocked == false
                    )
                    .SingleOrDefaultAsync();
                    if (nextActiveCalendar == null)
                    {
                        //Email ko system
                        throw new Exception($"No calendar found for: {nextNepaliActiveDate}");
                    }
                    else
                    {
                        currentActiveCalendar.IsActive = false;
                        nextActiveCalendar.IsActive = true;
                        nextActiveCalendar.IsLocked = true;
                    }
                }
                var numberOfRowsAffected = await _dbContext.SaveChangesAsync();
                if (numberOfRowsAffected < 0) throw new Exception("Unable to Update Calendar");
                string updatedDate = nextActiveCalendar != null
                ?
                $"{nextActiveCalendar.Year}-{nextActiveCalendar.Month}-{nextActiveCalendar.RunningDay}"
                :
                $"{currentActiveCalendar.Year}-{currentActiveCalendar.Month}-{currentActiveCalendar.RunningDay}";
                _logger.LogInformation($"{DateTime.Now}: Calendar Update Successful, now active calendar is {updatedDate} B.S");
                return $"Updated Date is {updatedDate}";
            }
            finally
            {
                dayEndLock.Release();
            }

        }

        private async Task PerformPostingTasks(List<DepositAccount> validAccounts, DateTime currentCompanyActiveDate, TypeOfDayEndTaskEnum typeOfDayEndTask)
        {
            if (!await dayEndLock.WaitAsync(TimeSpan.FromMinutes(0)))
                throw new Exception("Already a process is running...");
            
            try
            {
                CalendarDto currentActiveCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
                CompanyProfileDto companyProfile = await _companyProfileService.GetCompanyProfileService();
                decimal currentTax = companyProfile.CurrentTax;

                foreach (var account in validAccounts)
                    accountsForInterestPosting.Add(account.Id, account);
                

                foreach (DepositAccount account in validAccounts)
                {
                    _logger.LogInformation($"Interest Posting started for account {account.Id}->{account.AccountNumber}");
                    decimal transactionAmountForInterestPosting = account.InterestAmount;
                    if (transactionAmountForInterestPosting <= 0)
                    {
                        if (typeOfDayEndTask == TypeOfDayEndTaskEnum.MatureInterestPosting)
                            accountsForInterestPosting[account.Id].Status = AccountStatusEnum.Mature;
                        continue;
                    }
                    // VoucherNumber
                    TransactionVoucher transactionVoucher;
                    if (branchCodeWiseVoucherNumber.ContainsKey(account.BranchCode))
                        transactionVoucher = branchCodeWiseVoucherNumber[account.BranchCode];
                    else
                    {
                        transactionVoucher = await GenerateTransactionVoucher(account.BranchCode);
                        branchCodeWiseVoucherNumber.Add(account.BranchCode, transactionVoucher);
                    }
                    decimal taxDeductionTransactionAmount = transactionAmountForInterestPosting * (currentTax / 100);

                    // BaseTransaction.
                    // Interest Calculation
                    BaseTransaction baseTransactionOfInterestPosting = await MakeBaseTransaction(account.BranchCode, transactionAmountForInterestPosting, transactionVoucher, typeOfDayEndTask, currentCompanyActiveDate);
                    InterestPostWrapper interestPostWrapper = await GetPostingDetails(accountsForInterestPosting[account.Id], baseTransactionOfInterestPosting, typeOfDayEndTask);
                    // Deposit the interest amount
                    await MakeTransactionForInterestPosting(interestPostWrapper);

                    // Add Entry Of Amount after tax 
                    // Make a separate function
                    // https://docs.google.com/document/d/100563ptA_G0cvhI4Gt9jAmDHaIFjYwSli3wREeAIgLc/edit?usp=sharing
                    BaseTransaction amountAfterTaxBaseTransaction = await MakeBaseTransaction(account.BranchCode, transactionAmountForInterestPosting-taxDeductionTransactionAmount, transactionVoucher, TypeOfDayEndTaskEnum.InterestPostEntryOnDepositAccount, currentCompanyActiveDate);
                    Ledger depositSchemeTypeLedger = interestPostWrapper.isPostingToSameAccount?account.DepositScheme.SchemeType:interestPostWrapper.ToAccount.DepositScheme.SchemeType;
                    depositSchemeTypeLedger.CurrentBalance+=amountAfterTaxBaseTransaction.TransactionAmount;
                    LedgerTransaction ledgerTransaction = new()
                    {
                            Transaction = amountAfterTaxBaseTransaction,
                            TransactionType = TransactionTypeEnum.Credit,
                            Narration = $"Deposit Account '{account.AccountNumber}' Interest Post",
                            BalanceAfterTransaction = depositSchemeTypeLedger.CurrentBalance,
                            Ledger = account.DepositScheme.SchemeType
                    };
                    await _dbContext.LedgerTransactions.AddAsync(ledgerTransaction);

                    // Deduction Of Tax
                    BaseTransaction baseTransactionOfTaxDeduction = await MakeBaseTransaction(account.BranchCode, taxDeductionTransactionAmount, transactionVoucher, TypeOfDayEndTaskEnum.TaxCalculation, currentCompanyActiveDate);
                    interestPostWrapper.baseTransaction = baseTransactionOfTaxDeduction;
                    interestPostWrapper.typeOfDayEndTask = TypeOfDayEndTaskEnum.TaxCalculation;
                    await HandleTaxDeduction(interestPostWrapper);
                    await MakeTransactionForInterestPosting(interestPostWrapper);
                    
                    // 
                    // Change to Mature if the Operation is related to Mature InterestPosting
                    if (typeOfDayEndTask == TypeOfDayEndTaskEnum.MatureInterestPosting)
                        accountsForInterestPosting[account.Id].Status = AccountStatusEnum.Mature;
                    int newMonth = currentActiveCalendar.Month < 12 ? currentActiveCalendar.Month+1 : 1;
                    CalendarDto actualCalendar = await _companyProfileService.GetCalendarByYearAndMonthService(currentActiveCalendar.Year, newMonth);
                    DateTime currentPostingDate  = account.NextInterestPostingDate;
                    accountsForInterestPosting[account.Id].NextInterestPostingDate =  await _helper.GenerateNextInterestPostingDate(currentPostingDate, account.EnglishMatureDate,actualCalendar,account.DepositScheme.PostingScheme, false); 
                    accountsForInterestPosting[account.Id].PreviousInterestPostedDate = currentPostingDate;
                }
                int numberOfRowsAfftected = await _dbContext.SaveChangesAsync();
                // Clear all the updated values
                accountsForInterestPosting.Clear();
                branchCodeWiseVoucherNumber.Clear();
                ledgerForInterestPosting.Clear();
                subLedgerForInterestPosting.Clear();
                transactionVoucherCount = 0;

            }
            finally
            {
                dayEndLock.Release();
            }
        }
        private async Task<List<DepositAccount>> GetTodayMaturingAccounts(DateTime currentCompanyDate)
        {
            var depositAccounts = await _dbContext.DepositAccounts
            .Include(da => da.Client)
            .Include(da => da.DepositScheme)
            .ThenInclude(ds=>ds.SchemeType)
            // .Include(da => da.MatureInterestPostingAccountNumber)
            // .ThenInclude(mpa => mpa.DepositScheme)
            .Where(da => da.Status != AccountStatusEnum.Mature && da.Status != AccountStatusEnum.Close
                && da.DepositScheme.IsActive && da.Client.IsActive && da.EnglishMatureDate.Date == currentCompanyDate.Date)
            .ToListAsync();
            return depositAccounts;
        }
        private async Task<List<DepositAccount>> GetTodayInterestPostingAccount(DateTime currentCompanyActiveDate)
        {
            var depositAccounts = await _dbContext.DepositAccounts
            .Include(da => da.Client)
            .Include(da => da.DepositScheme)
            .ThenInclude(ds=>ds.SchemeType)
            //.Include(da => da.InterestPostingAccountNumber)
            //.ThenInclude(mpa => mpa.DepositScheme)
            .Where(da => da.Status != AccountStatusEnum.Close && da.InterestAmount > 0
                && da.DepositScheme.IsActive && da.Client.IsActive && da.NextInterestPostingDate.Date == currentCompanyActiveDate.Date
                ).ToListAsync();
            return depositAccounts;
        }

        private async Task<DateTime> GetCompanyActiveCalendarInAD()
        {
            CalendarDto calendarDto = await _companyProfileService.GetCurrentActiveCalenderService();
            return await _helper.GetCompanyActiveCalendarInAD(calendarDto);
        }

        private async Task<TransactionVoucher> GenerateTransactionVoucher(string branchCode)
        {
            if (transactionVoucherCount <= 0)
            {
                var lastRecord = await _dbContext.TransactionVouchers.OrderBy(tv => tv.Id).LastOrDefaultAsync();
                transactionVoucherCount = lastRecord==null?1:lastRecord.Id + 1;
            }
            else
                transactionVoucherCount++;
            var financialYear = await _dbContext.CompanyDetails.FirstOrDefaultAsync();
            if (financialYear == null)
                throw new NotImplementedException("No Records found for financial year");
            string voucherNumber = $"{financialYear.CurrentFiscalYear}VCH{transactionVoucherCount}{branchCode}";
            TransactionVoucher transactionVoucher = new TransactionVoucher() { VoucherNumber = voucherNumber };
            await _dbContext.AddAsync(transactionVoucher);
            return transactionVoucher;
        }

        private async Task HandleInterestPostingTask(InterestPostWrapper interestPostWrapper)
        {
            DepositAccount account = interestPostWrapper.FromAccount;
            int? postingAccountId = interestPostWrapper.typeOfDayEndTask == TypeOfDayEndTaskEnum.InterestPosting
            ?
            account.InterestPostingAccountNumberId
            :
            account.MatureInterestPostingAccountNumberId;

            if (account.Status != AccountStatusEnum.Suspend && postingAccountId != null)
            {
                DepositAccount postingAccount = accountsForInterestPosting.ContainsKey((int)postingAccountId)
                ?
                accountsForInterestPosting[(int)postingAccountId]
                :
                await _dbContext.DepositAccounts.Include(da => da.DepositScheme).ThenInclude(ds=>ds.SchemeType).Include(da => da.Client)
                .Where(await _commonExpression.GetExpressionForInterestPosting((int)postingAccountId))
                .FirstOrDefaultAsync();

                if (postingAccount != null)
                {
                    // POST TO OTHER ACCOUNT
                    if (!accountsForInterestPosting.ContainsKey(postingAccount.Id))
                        accountsForInterestPosting.Add(postingAccount.Id, postingAccount);
                    await UpdateDepositSchemeSubLedgerAndLedger((int)postingAccount.DepositScheme.InterestSubLedgerId);
                    interestPostWrapper.ToAccount = postingAccount;
                    interestPostWrapper.DepositSchemeInterestSubLedger = subLedgerForInterestPosting[(int)postingAccount.DepositScheme.InterestSubLedgerId];
                    interestPostWrapper.DepositSchemeInterestLedger = ledgerForInterestPosting[interestPostWrapper.DepositSchemeInterestSubLedger.LedgerId];
                    interestPostWrapper.isPostingToSameAccount = false;
                    return;
                }
            }
            // SELF POST
            interestPostWrapper.isPostingToSameAccount = true;
            await UpdateDepositSchemeSubLedgerAndLedger((int)account.DepositScheme.InterestSubLedgerId);
            interestPostWrapper.DepositSchemeInterestSubLedger = subLedgerForInterestPosting[(int)account.DepositScheme.InterestSubLedgerId];
            interestPostWrapper.DepositSchemeInterestLedger = ledgerForInterestPosting[interestPostWrapper.DepositSchemeInterestSubLedger.LedgerId];
            return;
        }
        private async Task<BaseTransaction> MakeBaseTransaction(string branchCode, decimal transactionAmount, TransactionVoucher transactionVoucher, TypeOfDayEndTaskEnum typeOfDayEndTaskEnum, DateTime currentActiveCompanyDate)
        {
            string nepaliTransactionDate = await _helper.ConvertEnglishDateToNepali(currentActiveCompanyDate);
            BaseTransaction baseTransaction = new BaseTransaction()
            {
                TransactionVoucher = transactionVoucher,
                BranchCode = branchCode,
                TransactionAmount = transactionAmount,
                Remarks = typeOfDayEndTaskEnum.ToString(),
                NepaliCreationDate = nepaliTransactionDate,
                EnglishCreationDate = currentActiveCompanyDate,
                RealWorldCreationDate = DateTime.Now,
                PaymentType = PaymentTypeEnum.Internal
            };
            await _transactions.GenerateBaseTransaction(baseTransaction);
            // await _dbContext.SaveChangesAsync();
            return baseTransaction;
        }


        private async Task<InterestPostWrapper> GetPostingDetails(DepositAccount account, BaseTransaction baseTransaction, TypeOfDayEndTaskEnum typeOfDayEndTask)
        {
            InterestPostWrapper interestPostWrapper = new()
            {
                FromAccount = account,
                AccountTransactionType = TransactionTypeEnum.Credit,
                SubLedgerTransactionType = TransactionTypeEnum.Credit,
                baseTransaction = baseTransaction,
                typeOfDayEndTask = typeOfDayEndTask,
                isPostingToSameAccount = true
            };
            if (typeOfDayEndTask != TypeOfDayEndTaskEnum.InterestPosting && typeOfDayEndTask != TypeOfDayEndTaskEnum.MatureInterestPosting)
                throw new Exception("Not accepted");
            await HandleInterestPostingTask(interestPostWrapper);
            return interestPostWrapper;
        }

        private async Task HandleTaxDeduction(InterestPostWrapper interestPostWrapper)
        {
            interestPostWrapper.AccountTransactionType = TransactionTypeEnum.Debit;
            interestPostWrapper.SubLedgerTransactionType = TransactionTypeEnum.Debit;
            if (interestPostWrapper.isPostingToSameAccount == false)
            {
                await UpdateDepositSchemeSubLedgerAndLedger((int)interestPostWrapper.ToAccount.DepositScheme.TaxSubledgerId);
                interestPostWrapper.DepositSchemeTaxSubLedger = subLedgerForInterestPosting[(int)interestPostWrapper.ToAccount.DepositScheme.TaxSubledgerId];
                interestPostWrapper.DepositSchemeTaxLedger = ledgerForInterestPosting[interestPostWrapper.DepositSchemeTaxSubLedger.LedgerId];
                return;
            }
            // SELF POST
            await UpdateDepositSchemeSubLedgerAndLedger((int)interestPostWrapper.FromAccount.DepositScheme.TaxSubledgerId);
            interestPostWrapper.DepositSchemeTaxSubLedger = subLedgerForInterestPosting[(int)interestPostWrapper.FromAccount.DepositScheme.TaxSubledgerId];
            interestPostWrapper.DepositSchemeTaxLedger = ledgerForInterestPosting[interestPostWrapper.DepositSchemeTaxSubLedger.LedgerId];
            return;
        }

        private async Task MakeTransactionForInterestPosting(InterestPostWrapper interestPostWrapper)
        {
            _logger.LogInformation("Updating the deposit account, subledger, and ledger");
            await UpdateDepositAccount(interestPostWrapper);
            await UpdateSubLedger(interestPostWrapper);
        }
        private async Task UpdateDepositSchemeSubLedgerAndLedger(int subledgerId)
        {
            SubLedger subLedger = subLedgerForInterestPosting.ContainsKey(subledgerId) ? subLedgerForInterestPosting[subledgerId] : await _dbContext.SubLedgers.FindAsync(subledgerId);
            Ledger ledger = ledgerForInterestPosting.ContainsKey(subLedger.LedgerId) ? ledgerForInterestPosting[subLedger.LedgerId] : await _dbContext.Ledgers.FindAsync(subLedger.LedgerId);
            if (subLedger == null || ledger == null)
                throw new Exception("No SubLedger and Ledger found for the deposit scheme");
            if (!subLedgerForInterestPosting.ContainsKey(subledgerId))
                subLedgerForInterestPosting.Add(subledgerId, subLedger);
            if (!ledgerForInterestPosting.ContainsKey(subLedger.LedgerId))
                ledgerForInterestPosting.Add(subLedger.LedgerId, ledger);
        }

        private async Task UpdateDepositAccount(InterestPostWrapper interestPostWrapper)
        {
            DepositAccount accountToUpdate;
            if (interestPostWrapper.isPostingToSameAccount)
            {
                _logger.LogInformation($"Interest Posting to same account");
                accountToUpdate = interestPostWrapper.FromAccount;
            }
            else
            {
                _logger.LogInformation($"Interest Posting From {interestPostWrapper.FromAccount.Id}->{interestPostWrapper.FromAccount.AccountNumber} to {interestPostWrapper?.ToAccount?.Id}->{interestPostWrapper?.ToAccount?.AccountNumber}");
                accountToUpdate = interestPostWrapper.ToAccount;
            }
            decimal currentBalance = accountToUpdate.PrincipalAmount;
            decimal transactionAmount = interestPostWrapper.baseTransaction.TransactionAmount;
            decimal balanceAfterTransaction = interestPostWrapper.typeOfDayEndTask == TypeOfDayEndTaskEnum.TaxCalculation
            ?
            currentBalance - transactionAmount
            :
            currentBalance + transactionAmount;
            if (balanceAfterTransaction < 0)
                return; // log it
            accountToUpdate.PrincipalAmount = balanceAfterTransaction;
            interestPostWrapper.FromAccount.InterestAmount = 0;
            await AddDepositAccountTransactionRecord(interestPostWrapper, balanceAfterTransaction);
        }

        private async Task AddDepositAccountTransactionRecord(InterestPostWrapper interestPostWrapper, decimal balanceAfterTransaction)
        {
            string sameAccountOrDifferent = interestPostWrapper.isPostingToSameAccount ?
            "same account" : $"{interestPostWrapper.ToAccount.AccountNumber}";

            DepositAccountTransaction depositAccountTransaction = new()
            {
                Transaction = interestPostWrapper.baseTransaction,
                BalanceAfterTransaction = balanceAfterTransaction,
                DepositAccount = interestPostWrapper.isPostingToSameAccount ? interestPostWrapper.FromAccount : interestPostWrapper.ToAccount,
                TransactionType = interestPostWrapper.AccountTransactionType,
                WithDrawalType = interestPostWrapper.typeOfDayEndTask == TypeOfDayEndTaskEnum.TaxCalculation ? WithDrawalTypeEnum.ByTax : null,
                Source = interestPostWrapper.typeOfDayEndTask.ToString(),
                Narration = interestPostWrapper.typeOfDayEndTask == TypeOfDayEndTaskEnum.TaxCalculation ?
                $"Tax deduction while interest posting from {interestPostWrapper.FromAccount.AccountNumber} to {sameAccountOrDifferent}"
                :
                $"Interest Posted from {interestPostWrapper.FromAccount.AccountNumber} to {sameAccountOrDifferent}"
            };
            await _dbContext.DepositAccountTransactions.AddAsync(depositAccountTransaction);
        }

        private async Task UpdateSubLedger(InterestPostWrapper interestPostWrapper)
        {
            decimal balanceAfterTransactionOfSubledger;
            decimal balanceAfterTransactionOfLedger;
            if (interestPostWrapper.typeOfDayEndTask != TypeOfDayEndTaskEnum.TaxCalculation)
            {
                interestPostWrapper.DepositSchemeInterestSubLedger.CurrentBalance += interestPostWrapper.baseTransaction.TransactionAmount;
                interestPostWrapper.DepositSchemeInterestLedger.CurrentBalance += interestPostWrapper.baseTransaction.TransactionAmount;
                balanceAfterTransactionOfSubledger = interestPostWrapper.DepositSchemeInterestSubLedger.CurrentBalance;
                balanceAfterTransactionOfLedger = interestPostWrapper.DepositSchemeInterestLedger.CurrentBalance;
            }
            else
            {
                interestPostWrapper.DepositSchemeTaxSubLedger.CurrentBalance += interestPostWrapper.baseTransaction.TransactionAmount;
                interestPostWrapper.DepositSchemeTaxLedger.CurrentBalance += interestPostWrapper.baseTransaction.TransactionAmount;
                balanceAfterTransactionOfSubledger = interestPostWrapper.DepositSchemeTaxSubLedger.CurrentBalance;
                balanceAfterTransactionOfLedger = interestPostWrapper.DepositSchemeTaxLedger.CurrentBalance;

            }
            await AddSubLedgerTransactionRecord(interestPostWrapper, balanceAfterTransactionOfSubledger);
            await AddLedgerTransactionRecord(interestPostWrapper, balanceAfterTransactionOfLedger);

        }

        private async Task AddSubLedgerTransactionRecord(InterestPostWrapper interestPostWrapper, decimal balanceAfterTransaction)
        {
            SubLedgerTransaction subLedgerTransaction = new()
            {
                Transaction = interestPostWrapper.baseTransaction,
                TransactionType = interestPostWrapper.SubLedgerTransactionType,
                Narration = interestPostWrapper.typeOfDayEndTask.ToString(),
                BalanceAfterTransaction = balanceAfterTransaction,
                SubLedger = interestPostWrapper.typeOfDayEndTask == TypeOfDayEndTaskEnum.TaxCalculation ? interestPostWrapper.DepositSchemeTaxSubLedger : interestPostWrapper.DepositSchemeInterestSubLedger
            };
            await _dbContext.SubLedgerTransactions.AddAsync(subLedgerTransaction);
        }

        private async Task AddLedgerTransactionRecord(InterestPostWrapper interestPostWrapper, decimal balanceAfterTransaction)
        {
            LedgerTransaction ledgerTransaction = new()
            {
                Transaction = interestPostWrapper.baseTransaction,
                TransactionType = interestPostWrapper.SubLedgerTransactionType,
                Narration = interestPostWrapper.typeOfDayEndTask.ToString(),
                BalanceAfterTransaction = balanceAfterTransaction,
                Ledger = interestPostWrapper.typeOfDayEndTask == TypeOfDayEndTaskEnum.TaxCalculation ? interestPostWrapper.DepositSchemeTaxLedger : interestPostWrapper.DepositSchemeInterestLedger
            };
            await _dbContext.LedgerTransactions.AddAsync(ledgerTransaction);
        }
    }
}