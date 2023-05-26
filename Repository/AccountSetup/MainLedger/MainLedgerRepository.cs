using MicroFinance.DBContext;
// using MicroFinance.DBContext.CompanyOperations;
using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.AccountSetup.MainLedger
{
    public class MainLedgerRepository : IMainLedgerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<MainLedgerRepository> _logger;

        public MainLedgerRepository(ApplicationDbContext dbContext, ILogger<MainLedgerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        // Operations Related to Account
        public async Task<int> CreateAccountType(AccountType accountType)
        {
            _logger.LogInformation($"{DateTime.Now} (CreateAccountType) Creating...");
            await _dbContext.AccountTypes.AddAsync(accountType);
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<AccountType> GetAccountType(int id)
        {
            _logger.LogInformation($"{DateTime.Now} (GetAccountType) Returing...");
            return await _dbContext.AccountTypes.FindAsync(id);
        }

        public async Task<List<AccountType>> GetAccountTypes()
        {
            var accountTypes = await _dbContext.AccountTypes.ToListAsync();
            return accountTypes;
        }

        // END

        // Operations Related to Group
        public async Task<int> CreateGroupType(GroupType groupType)
        {
            _logger.LogInformation($"{DateTime.Now} (CreateGroupType) Creating...");
            await _dbContext.GroupTypes.AddAsync(groupType);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<GroupType> GetGroupTypeById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} (GetGroupTypeById) Returning...");
            return await _dbContext.GroupTypes
            .Include(gt => gt.AccountType)
            .SingleOrDefaultAsync(gt => gt.Id == id);
        }

        public async Task<GroupType> GetGroupByName(string name, string accountTypeName)
        {
            var groupTypeForDepositScheme = await _dbContext.GroupTypes
            .Where(gt => gt.Name == name && gt.AccountType.Name == accountTypeName)
            .FirstOrDefaultAsync();
            return groupTypeForDepositScheme;
        }

        public async Task<List<GroupType>> GetGroupTypes()
        {
            _logger.LogInformation($"{DateTime.Now} (GetGroupTypes) Returing...");
            return await _dbContext.GroupTypes.Include(gt => gt.AccountType).ToListAsync();
        }

        public async Task<List<GroupType>> GetGroupTypesByAccountType(int accountTypeId)
        {
            _logger.LogInformation($"{DateTime.Now} (GetGroupTypesByAccountType) Returing...");
            var groupTypes =
            await _dbContext.GroupTypes
            .Include(gt => gt.AccountType)
            .Where(gt => gt.AccountTypeId == accountTypeId)
            .ToListAsync();
            return groupTypes;

        }

        // Operations Related to Group Details
        public async Task<int> CreateGroupTypeDetails(GroupTypeDetails groupTypeDetails)
        {
            await _dbContext.GroupTypeDetails.AddAsync(groupTypeDetails);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> EditGroupTypeDetails(GroupTypeDetails groupTypeDetails)
        {
            var existingGroupTypeDetails = await _dbContext.GroupTypeDetails.FindAsync(groupTypeDetails.Id);
            var propertyBag = _dbContext.Entry(existingGroupTypeDetails).CurrentValues;
            foreach (var property in propertyBag.Properties)
            {
                var newValue = groupTypeDetails.GetType().GetProperty(property.Name)?.GetValue(groupTypeDetails);
                if (newValue != null && !Equals(propertyBag[property.Name], newValue))
                {
                    propertyBag[property.Name] = newValue;
                }
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<GroupTypeDetails>> GetGroupTypeDetails()
        {
            return await _dbContext.GroupTypeDetails
            .Include(gtd => gtd.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .ToListAsync();
        }
        public async Task<GroupTypeDetails> GetGroupTypeDetailsById(int id)
        {
            return await _dbContext.GroupTypeDetails
            .Include(gtd => gtd.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .SingleOrDefaultAsync(gtd => gtd.Id == id);
        }
        public async Task<List<GroupTypeDetails>> GetGroupTypeDetailsByGroupType(int groupTypeId)
        {
            return await _dbContext.GroupTypeDetails
            .Where(gtd => gtd.GroupTypeId == groupTypeId)
            .Include(gtd => gtd.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .ToListAsync();
        }

        public async Task<List<GroupTypeDetails>> GetGroupTypeDetailsByAccountType(int accountTypeId)
        {
            return await _dbContext.GroupTypeDetails
            .Where(gtd => gtd.GroupType.AccountTypeId == accountTypeId)
            .Include(gtd => gtd.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .ToListAsync();
        }

        // Operations Related to Ledger
        public async Task<int> CreateLedger(Ledger ledger, GroupType groupType)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                _logger.LogInformation($"{DateTime.Now} Creating Ledger...");
                await _dbContext.Ledgers.AddAsync(ledger);
                var groupTypeAndLedgerMap = new GroupTypeAndLedgerMap();
                groupTypeAndLedgerMap.Ledger = ledger;
                groupTypeAndLedgerMap.GroupType = groupType;
                await _dbContext.AddAsync(groupTypeAndLedgerMap);
                var mappingStatus = await _dbContext.SaveChangesAsync();
                if (mappingStatus < 1)
                {
                    _logger.LogError($"{DateTime.Now} Unable to create ledger");
                    await transaction.RollbackAsync();
                    return 0;
                }
                await transaction.CommitAsync();

                return 1;
            }

        }

        public async Task<int> EditLedger(Ledger ledger)
        {
            var existingLedger = await _dbContext.Ledgers.FindAsync(ledger.Id);
            ledger.Id = existingLedger.Id;
            var propertyBag = _dbContext.Entry(existingLedger).CurrentValues;
            foreach (var property in propertyBag.Properties)
            {
                var newValue = ledger.GetType().GetProperty(property.Name)?.GetValue(ledger);
                if (newValue != null && !Equals(propertyBag[property.Name], newValue))
                {
                    propertyBag[property.Name] = newValue;
                }
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<GroupTypeAndLedgerMap> GetLedgerById(int id)
        {
            return await _dbContext.GroupTypeAndLedgerMaps
            .Where(gtl => gtl.LedgerId == id)
            .Include(gtl => gtl.Ledger)
            .Include(gtl => gtl.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .SingleOrDefaultAsync();
            //return await _dbContext.Ledgers.FindAsync(id);
        }
        public async Task<Ledger> GetLedgerByGroupTypeAndLedgerName(GroupType groupType, string name)
        {
            var ledgerDetails = await _dbContext.GroupTypeAndLedgerMaps
            .Include(gtlm => gtlm.Ledger)
            .Where(gtlm => gtlm.GroupTypeId == groupType.Id && gtlm.Ledger.Name == name)
            .SingleOrDefaultAsync();
            if (ledgerDetails != null)
                return ledgerDetails.Ledger;
            return new Ledger();
        }

        public async Task<Ledger> GetLedger(int id)
        {
            return await _dbContext.Ledgers.FindAsync(id);
        }
        public async Task<List<GroupTypeAndLedgerMap>> GetLedgers()
        {
            return await _dbContext.GroupTypeAndLedgerMaps
            .Include(gtl => gtl.Ledger)
            .Include(gtl => gtl.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .ToListAsync();
            //return await _dbContext.Ledgers.ToListAsync();
        }

        // Operation related to Subledger

        public async Task<int> CreateSubLedger(SubLedger subLedger)
        {
            await _dbContext.SubLedgers.AddAsync(subLedger);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> EditSubledger(SubLedger subLedger)
        {
            var existingSubLedger = await _dbContext.SubLedgers.FindAsync(subLedger.Id);
            var propertyBag = _dbContext.Entry(existingSubLedger).CurrentValues;
            foreach (var property in propertyBag.Properties)
            {
                var newValue = subLedger.GetType().GetProperty(property.Name)?.GetValue(subLedger);
                if (newValue != null && !Equals(propertyBag[property.Name], newValue))
                {
                    propertyBag[property.Name] = newValue;
                }
            }
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<SubLedger> GetSubLedger(int id)
        {
            return await _dbContext.SubLedgers.FindAsync(id);
        }
        public async Task<GroupSubLedger> GetSubLedgerById(int id)
        {

            var results = await (from subledger in _dbContext.SubLedgers
                                 where subledger.Id == id
                                 join groupTypeAndLedgerMap in _dbContext.GroupTypeAndLedgerMaps on subledger.LedgerId equals groupTypeAndLedgerMap.LedgerId
                                 join groupType in _dbContext.GroupTypes on groupTypeAndLedgerMap.GroupTypeId equals groupType.Id
                                 select new GroupSubLedger
                                 {
                                     SubLedger = new SubLedger
                                     {
                                         Id = subledger.Id,
                                         Name = subledger.Name,
                                         Description = subledger.Description,
                                         Ledger = subledger.Ledger,
                                         LedgerId = subledger.LedgerId
                                     },
                                     GroupType = new GroupType
                                     {
                                         Id = groupType.Id,
                                         Name = groupType.Name,
                                         NepaliName = groupType.NepaliName,
                                         EntryDate = groupType.EntryDate,
                                         Schedule = groupType.Schedule,
                                         AccountType = groupType.AccountType,
                                         AccountTypeId = groupType.AccountTypeId
                                     }

                                 }).SingleOrDefaultAsync();
            return results;

        }

        public async Task<List<GroupSubLedger>> GetSubLedgers()
        {
            var results = await (from subledger in _dbContext.SubLedgers
                                 join groupTypeAndLedgerMap in _dbContext.GroupTypeAndLedgerMaps on subledger.LedgerId equals groupTypeAndLedgerMap.LedgerId
                                 join groupType in _dbContext.GroupTypes on groupTypeAndLedgerMap.GroupTypeId equals groupType.Id

                                 select new GroupSubLedger
                                 {
                                     SubLedger = new SubLedger
                                     {
                                         Id = subledger.Id,
                                         Name = subledger.Name,
                                         Description = subledger.Description,
                                         Ledger = subledger.Ledger,
                                         LedgerId = subledger.LedgerId
                                     },
                                     GroupType = new GroupType
                                     {
                                         Id = groupType.Id,
                                         Name = groupType.Name,
                                         NepaliName = groupType.NepaliName,
                                         EntryDate = groupType.EntryDate,
                                         Schedule = groupType.Schedule,
                                         AccountType = groupType.AccountType,
                                         AccountTypeId = groupType.AccountTypeId
                                     }
                                 })
                     .ToListAsync();
            return results;
        }

        public async Task<List<GroupSubLedger>> GetSubLedgersByLedger(int ledgerId)
        {

            var results = await (from subledger in _dbContext.SubLedgers
                                 where subledger.LedgerId == ledgerId
                                 join groupTypeAndLedgerMap in _dbContext.GroupTypeAndLedgerMaps on subledger.LedgerId equals groupTypeAndLedgerMap.LedgerId
                                 join groupType in _dbContext.GroupTypes on groupTypeAndLedgerMap.GroupTypeId equals groupType.Id

                                 select new GroupSubLedger
                                 {
                                     SubLedger = new SubLedger
                                     {
                                         Id = subledger.Id,
                                         Name = subledger.Name,
                                         Description = subledger.Description,
                                         Ledger = subledger.Ledger,
                                         LedgerId = subledger.LedgerId
                                     },
                                     GroupType = new GroupType
                                     {
                                         Id = groupType.Id,
                                         Name = groupType.Name,
                                         NepaliName = groupType.NepaliName,
                                         EntryDate = groupType.EntryDate,
                                         Schedule = groupType.Schedule,
                                         AccountType = groupType.AccountType,
                                         AccountTypeId = groupType.AccountTypeId
                                     }
                                 })
                     .ToListAsync();
            return results;
        }

        // Related to GroupTypeLedgerMap
        public async Task<bool> CheckIfLedgerAndGroupNameExist(int grouptypeId, string ledgerName)
        {
            var entry = await _dbContext.GroupTypeAndLedgerMaps
            .Where(gtl => gtl.GroupTypeId == grouptypeId && gtl.Ledger.Name == ledgerName)
            .ToListAsync();
            if (entry != null && entry.Count >= 1) return true;
            return false;
        }

        public async Task<int> CreateGroupTypeAndLedgerMap(GroupTypeAndLedgerMap groupTypeAndLedgerMap)
        {
            await _dbContext.GroupTypeAndLedgerMaps.AddAsync(groupTypeAndLedgerMap);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<List<GroupTypeAndLedgerMap>> GetGroupTypeAndLedgerMaps()
        {
            return await _dbContext.GroupTypeAndLedgerMaps
            .Include(gtl => gtl.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Include(gtl => gtl.Ledger)
            .ToListAsync();
        }

        public async Task<GroupTypeAndLedgerMap> GetGroupTypeAndLedgerMapById(int id)
        {
            return await _dbContext.GroupTypeAndLedgerMaps
            .Include(gtl => gtl.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Include(gtl => gtl.Ledger)
            .SingleOrDefaultAsync(gtl => gtl.Id == id);
        }

        public async Task<List<GroupTypeAndLedgerMap>> GetGroupTypeAndLedgerMapByAccountType(int accountTypeId)
        {
            return await _dbContext.GroupTypeAndLedgerMaps

            .Where(gtl => gtl.GroupType.AccountTypeId == accountTypeId)
            .Include(gtl => gtl.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Include(gtl => gtl.Ledger)
            .ToListAsync();
        }

        public async Task<List<GroupTypeAndLedgerMap>> GetGroupTypeAndLedgerMapByGroupType(int groupTypeId)
        {
            return await _dbContext.GroupTypeAndLedgerMaps
            .Where(gtl => gtl.GroupTypeId == groupTypeId)
            .Include(gtl => gtl.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Include(gtl => gtl.Ledger)
            .ToListAsync();
        }
    }
}