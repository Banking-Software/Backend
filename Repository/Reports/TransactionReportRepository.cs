using System.Linq.Expressions;
using MicroFinance.DBContext;
using MicroFinance.Enums;
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Share;
using MicroFinance.Models.Transactions;
using MicroFinance.Models.Wrapper;
using MicroFinance.Models.Wrapper.Reports;
using MicroFinance.Models.Wrapper.Reports.TrailBalance;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.Reports
{
    public class TransactionReportrepository : ITransactionReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TransactionReportrepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // LedgerTransaction has list in decending order as per creation date
        private async Task<List<LedgerLevel>> GetLedgerTotalBalanceFromLedgerTransaction(List<LedgerTransaction> ledgerTransactions)
        {
            List<int> transactionLedgerIds = ledgerTransactions.Select(x=>x.Ledger.Id).ToList();
            List<Ledger> remainingLedger = await _dbContext.Ledgers.Where(x=>!transactionLedgerIds.Contains(x.Id)).AsNoTracking().ToListAsync();

            List<LedgerLevel> ledgerLevelSum = ledgerTransactions.GroupBy(lt=>lt.Ledger.Id)
            .Select(grp=> new LedgerLevel{
                Id = grp.Key,
                Name = grp.First().Ledger.Name,
                CurrentBalance = grp.First().BalanceAfterTransaction,
                GroupTypeId=grp.First().Ledger.GroupTypeId
            }).ToList();
            
            if(remainingLedger!=null && remainingLedger.Count>=1)
                foreach (var ledger in remainingLedger)
                    ledgerLevelSum.Add(new LedgerLevel(){Id=ledger.Id, Name=ledger.Name, GroupTypeId=ledger.GroupTypeId, CurrentBalance=0});
            return ledgerLevelSum;
        }

        private async Task<List<GroupTypeLevel>> GetGroupTypeTotalBalanceFromLedgerTransaction(List<LedgerLevel> ledgerLevels)
        {
            var groupTypes = await _dbContext.GroupTypes.ToListAsync();
            // var accountTypes = await _dbContext.AccountTypes.ToListAsync();
            List<GroupTypeLevel> groupTypeLevelSum = new();
            // List<AccountTypeLevel> accountTypeLevelSum = new();

            foreach(var grpTyp in groupTypes)
            {
                var tempGroupTypeLevel = new GroupTypeLevel(){Id = grpTyp.Id, Name=grpTyp.Name, CharkhataNumber= grpTyp.CharKhataNumber, CurrentBalance=0, AccountTypeId = grpTyp.AccountTypeId, LedgerLevels=new List<LedgerLevel>()};
                tempGroupTypeLevel.LedgerLevels=ledgerLevels.Where(x=>x.GroupTypeId==grpTyp.Id).ToList();
                if(tempGroupTypeLevel.LedgerLevels!=null && tempGroupTypeLevel.LedgerLevels.Count>=1)
                {
                    tempGroupTypeLevel.CurrentBalance = tempGroupTypeLevel.LedgerLevels.Sum(x=>x.CurrentBalance);
                }
                groupTypeLevelSum.Add(tempGroupTypeLevel);
            }
            return groupTypeLevelSum;
        }

        private async Task<List<AccountTypeLevel>> GetAccountTypeTotalBalanceFromGroupTypeTransaction(List<GroupTypeLevel> groupTypeLevel)
        {
            var accountTypes = await _dbContext.AccountTypes.ToListAsync();
            List<AccountTypeLevel> accountTypeLevelSum = new();
            foreach(var acc in accountTypes)
            {
                var tempAccountTypeLevel = new AccountTypeLevel(){Id=acc.Id, Name=acc.Name, CurrentBalance=0, GroupTypeLevels=new List<GroupTypeLevel>()};
                tempAccountTypeLevel.GroupTypeLevels = groupTypeLevel.Where(x=>x.AccountTypeId==acc.Id).ToList();
                if(tempAccountTypeLevel.GroupTypeLevels!=null && tempAccountTypeLevel.GroupTypeLevels.Count>=1)
                {
                    tempAccountTypeLevel.CurrentBalance = tempAccountTypeLevel.GroupTypeLevels.Sum(x=>x.CurrentBalance);
                }
                accountTypeLevelSum.Add(tempAccountTypeLevel);

            }
            return accountTypeLevelSum;
        }

        public async Task<TrailBalance> GenerateTrailBalanceReport(DateTime fromDate, DateTime toDate)
        {
            var ledgerTransactions = await _dbContext.LedgerTransactions
            .Include(lt=>lt.Transaction)
            .Include(lt=>lt.Ledger)
            .Where(lt=>lt.Transaction.EnglishCreationDate>=fromDate && lt.Transaction.EnglishCreationDate<=toDate)
            .OrderByDescending(lt=>lt.Transaction.RealWorldCreationDate)
            .AsNoTracking().ToListAsync();
            
            List<LedgerLevel> ledgerLevelSum = await GetLedgerTotalBalanceFromLedgerTransaction(ledgerTransactions);
            List<GroupTypeLevel> groupLevelSum = await GetGroupTypeTotalBalanceFromLedgerTransaction(ledgerLevelSum);
            List<AccountTypeLevel> accountTypeLevelSum = await GetAccountTypeTotalBalanceFromGroupTypeTransaction(groupLevelSum);
            decimal debitSum=0;
            decimal creditSum=0;
            foreach (var accountType in accountTypeLevelSum)
            {
                if(accountType.Id==1 || accountType.Id==2)
                    debitSum+=(decimal) accountType.CurrentBalance;
                else
                    creditSum+=(decimal) accountType.CurrentBalance;
            }
            decimal difference = Math.Abs(debitSum-creditSum);

            return new TrailBalance(){AccountTypeLevels = accountTypeLevelSum, CreditSum=creditSum, DebitSum=debitSum, Difference=difference};
        }

        public async Task<DepositAccountTransactionReportWrapper> GetDepositAccountTransactionReport(Expression<Func<DepositAccountTransaction, bool>> expressionOnDepositAccountTransaction)
        {
            // Get the report based on provided date
            List<DepositAccountTransactionReport> depositAccountTransactionReport =
            await _dbContext.DepositAccountTransactions
            .Include(dat => dat.Transaction)
            .ThenInclude(bt=>bt.TransactionVoucher)
            .Include(dat => dat.DepositAccount)
            .Where(expressionOnDepositAccountTransaction)
            .OrderBy(dat => dat.Transaction.RealWorldCreationDate)
            .Select
            (dat =>
                new DepositAccountTransactionReport
                {
                    BaseTransactionId = dat.Transaction.Id,
                    DepositAccountTransactionId = dat.Id,
                    VoucherNumber = dat.Transaction.TransactionVoucher.VoucherNumber,
                    TransactionRemarks = dat.Transaction.Remarks,
                    TransactionType = dat.TransactionType,
                    TransactionAmount = dat.Transaction.TransactionAmount,
                    Narration = dat.Narration,
                    Description = dat.Remarks,
                    RealWorldCreationDate = dat.Transaction.RealWorldCreationDate,
                    NepaliCreationDate = dat.Transaction.NepaliCreationDate,
                    EnglishCreationDate = dat.Transaction.EnglishCreationDate,
                    BalanceAfterTransaction = dat.BalanceAfterTransaction,
                    TransactionDoneBy = dat.Transaction.CreatedBy,
                    Status = dat.DepositAccount.Status,
                    DepositAccountId = (int)dat.DepositAccountId
                }
            ).AsNoTracking().ToListAsync();

            if (depositAccountTransactionReport != null && depositAccountTransactionReport.Any())
            {
                // Find the total amount in debit and credit side
                var transactionSum = depositAccountTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                // Get the BalanceAfterTransaction exact before our first entry of report: OpeningBalance
                var firstReport = depositAccountTransactionReport[0];
                var previousAccount = await _dbContext.DepositAccountTransactions
                .Include(dat => dat.Transaction)
                .Where(dat => dat.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate && dat.DepositAccountId == firstReport.DepositAccountId)
                .OrderByDescending(dat => dat.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                var depositAccountDetail = await _dbContext.DepositAccounts
                .Include(da => da.DepositScheme)
                .Include(da => da.Client)
                .Where(da => da.Id == firstReport.DepositAccountId).FirstOrDefaultAsync();

                DepositAccountTransactionReportWrapper depositAccountTransactionReportWrapper = new()
                {
                    DepositAccountTransactionReports = depositAccountTransactionReport,
                    PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction,
                    CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount,
                    DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount,
                    depositAccountDetails = new DepositAccountDetails()
                    {
                        ClientName = $"{depositAccountDetail.Client.ClientFirstName} {depositAccountDetail.Client.ClientLastName}",
                        ClientMemberId = depositAccountDetail.Client.ClientId,
                        AccountNumber = depositAccountDetail.AccountNumber,
                        NepaliOpeningDate = depositAccountDetail.NepaliCreationDate,
                        NepaliMatureDate = depositAccountDetail.NepaliMatureDate,
                        NepaliNextInterestPositngDate = $"{NepaliCalendar.Convert.ToNepali(depositAccountDetail.NextInterestPostingDate)} B.S",
                        PANNumber = depositAccountDetail.Client.ClientPanNumber,
                        MobileNumber = $"{depositAccountDetail.Client.ClientMobileNumber1}, {depositAccountDetail.Client.ClientMobileNumber2}",
                        DepositScheme = depositAccountDetail.DepositScheme.SchemeName
                    }
                };
                return depositAccountTransactionReportWrapper;
            }
            return new DepositAccountTransactionReportWrapper();
        }

        public async Task<LedgerTransactionReportWrapper> GetLedgerTransactionReport(Expression<Func<LedgerTransaction, bool>> expressionOnLedgerTransaction)
        {
            LedgerTransactionReportWrapper ledgerTransactionReportWrapper = new();
            var ledgerTransactionReport = await _dbContext.LedgerTransactions
            .Include(lt => lt.Transaction)
            .ThenInclude(bt=>bt.TransactionVoucher)
            .Include(lt => lt.Ledger)
            .Where(expressionOnLedgerTransaction)
            .OrderBy(lt => lt.Transaction.RealWorldCreationDate)
            .Select
            (lt =>
                new LedgerTransactionReport
                {
                    LedgerId = (int)lt.LedgerId,
                    BaseTransactionId = lt.Transaction.Id,
                    LedgerTransactionId = lt.Id,
                    VoucherNumber = lt.Transaction.TransactionVoucher.VoucherNumber,
                    TransactionRemarks = lt.Transaction.Remarks,
                    TransactionType = lt.TransactionType,
                    TransactionAmount = lt.Transaction.TransactionAmount,
                    Narration = lt.Narration,
                    Description = lt.Remarks,
                    RealWorldCreationDate = lt.Transaction.RealWorldCreationDate,
                    NepaliCreationDate = lt.Transaction.NepaliCreationDate,
                    EnglishCreationDate = lt.Transaction.EnglishCreationDate,
                    BalanceAfterTransaction = lt.BalanceAfterTransaction,
                    TransactionDoneBy = lt.Transaction.CreatedBy
                }
            ).AsNoTracking().ToListAsync();

            if (ledgerTransactionReport != null && ledgerTransactionReport.Any())
            {
                var transactionSum = ledgerTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = ledgerTransactionReport[0];

                var previousAccount = await _dbContext.LedgerTransactions
                .Include(lt => lt.Transaction)
                .Where(lt => lt.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate && lt.LedgerId==firstReport.LedgerId)
                .OrderByDescending(lt => lt.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                ledgerTransactionReportWrapper.LedgerTransactionReports = ledgerTransactionReport;
                ledgerTransactionReportWrapper.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                ledgerTransactionReportWrapper.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                ledgerTransactionReportWrapper.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;
            }
            return ledgerTransactionReportWrapper;
        }

        public async Task<ShareTransactionReportWrapper> GetShareTransactionReport(Expression<Func<ShareTransaction, bool>> expressionOnShareTransaction)
        {
            // Expression<Func<BaseTransaction, bool>> dateFilterPredicate = GetDateFilterPredicate(dateFrom, dateTo);
            ShareTransactionReportWrapper shareTransactionReportWrappr = new();
            var shareTransactionReport = await _dbContext.ShareTransactions
            .Include(st => st.Transaction)
            .ThenInclude(bt=>bt.TransactionVoucher)
            .Include(st => st.ShareAccount)
            .Where(expressionOnShareTransaction).OrderBy(st => st.Transaction.RealWorldCreationDate)
            .Select(st => new ShareTransactionReport
            {
                ShareAccountId = (int) st.ShareAccountId,
                BaseTransactionId = st.TransactionId,
                ShareTransactionId = st.Id,
                VoucherNumber = st.Transaction.TransactionVoucher.VoucherNumber,
                TransactionRemarks = st.Transaction.Remarks,
                TransactionType = st.TransactionType,
                TransactionAmount = st.Transaction.TransactionAmount,
                BalanceAfterTransaction = st.BalanceAfterTransaction,
                EnglishCreationDate = st.Transaction.EnglishCreationDate,
                NepaliCreationDate = st.Transaction.NepaliCreationDate,
                RealWorldCreationDate = st.Transaction.RealWorldCreationDate,
                Narration = st.Narration,
                Description = st.Remarks,
                TransactionDoneBy = st.Transaction.CreatedBy

            }).AsNoTracking().ToListAsync();

            if (shareTransactionReport != null && shareTransactionReport.Any())
            {
                var transactionSum = shareTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = shareTransactionReport[0];

                var previousAccount = await _dbContext.ShareTransactions
                .Include(st => st.Transaction)
                .Where(st => st.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate && st.ShareAccountId==firstReport.ShareAccountId)
                .OrderByDescending(st => st.Transaction.RealWorldCreationDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                shareTransactionReportWrappr.ShareTransactionReports = shareTransactionReport;
                shareTransactionReportWrappr.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                shareTransactionReportWrappr.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                shareTransactionReportWrappr.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;
            }
            return shareTransactionReportWrappr;
        }

        public async Task<SubLedgerTransactionReportWrapper> GetSubLedgerTransactionReport(Expression<Func<SubLedgerTransaction, bool>> expressionOnSubLedgerTransaction)
        {
            SubLedgerTransactionReportWrapper subLedgerTransactionReportWrapper = new();
            var subLedgerTransactionReport = await _dbContext.SubLedgerTransactions
            .Include(lt => lt.Transaction)
            .ThenInclude(bt=>bt.TransactionVoucher)
            .Include(lt => lt.SubLedger)
            .Where(expressionOnSubLedgerTransaction)
            .OrderBy(lt => lt.Transaction.RealWorldCreationDate)
            .Select
            (lt =>
                new SubLedgerTransactionReport
                {
                    SubLedgerId = lt.SubLedgerId,
                    BaseTransactionId = lt.Transaction.Id,
                    SubLedgerTransactionId = lt.Id,
                    VoucherNumber = lt.Transaction.TransactionVoucher.VoucherNumber,
                    TransactionRemarks = lt.Transaction.Remarks,
                    TransactionType = lt.TransactionType,
                    TransactionAmount = lt.Transaction.TransactionAmount,
                    Narration = lt.Narration,
                    Description = lt.Remarks,
                    RealWorldCreationDate = lt.Transaction.RealWorldCreationDate,
                    NepaliCreationDate = lt.Transaction.NepaliCreationDate,
                    EnglishCreationDate = lt.Transaction.EnglishCreationDate,
                    BalanceAfterTransaction = lt.BalanceAfterTransaction,
                    TransactionDoneBy = lt.Transaction.CreatedBy
                }
            ).AsNoTracking().ToListAsync();

            if (subLedgerTransactionReport != null && subLedgerTransactionReport.Any())
            {
                var transactionSum = subLedgerTransactionReport
                .GroupBy(report => report.TransactionType)
                .Select(grp => new
                {
                    TransactionType = grp.Key,
                    TransactionSumAmount = grp.Sum(report => report.TransactionAmount)
                });

                var firstReport = subLedgerTransactionReport[0];

                var previousAccount = await _dbContext.SubLedgerTransactions
                .Include(st => st.Transaction)
                .Where(st => st.Transaction.RealWorldCreationDate < firstReport.RealWorldCreationDate && st.SubLedgerId == firstReport.SubLedgerId)
                .OrderByDescending(st => st.Transaction.RealWorldCreationDate)
                .AsNoTracking()

                .FirstOrDefaultAsync();
                subLedgerTransactionReportWrapper.SubLedgerTransactionReports = subLedgerTransactionReport;
                subLedgerTransactionReportWrapper.PreviousBalanceAfterTransaction = previousAccount?.BalanceAfterTransaction;
                subLedgerTransactionReportWrapper.CreditSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Credit).SingleOrDefault()?.TransactionSumAmount;
                subLedgerTransactionReportWrapper.DebitSum = transactionSum.Where(ts => ts.TransactionType == TransactionTypeEnum.Debit).SingleOrDefault()?.TransactionSumAmount;
            }
            return subLedgerTransactionReportWrapper;
        }
    }
}