using MicroFinance.DBContext;
using MicroFinance.Dtos.AccountSetup.MainLedger;
// using MicroFinance.DBContext.CompanyOperations;
using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;

namespace MicroFinance.Repository.AccountSetup.MainLedger
{
    public class MainLedgerRepository : IMainLedgerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<MainLedgerRepository> _logger;

        public MainLedgerRepository(ApplicationDbContext dbContext,ILogger<MainLedgerRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        // Operations Related to Account
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
        public async Task<bool> CheckIfGroupNameExist(int accountTypeId, string groupName)
        {
            var group = await _dbContext.GroupTypes.Where(gt => gt.AccountTypeId == accountTypeId && gt.Name == groupName).FirstOrDefaultAsync();
            if (group != null) return true;
            return false;
        }
        public async Task<int> CreateGroupType(GroupType groupType)
        {
            _logger.LogInformation($"{DateTime.Now} (CreateGroupType) Creating...");
            await _dbContext.GroupTypes.AddAsync(groupType);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateGroupType(UpdateGroupTypeDto groupTypeDto)
        {
            var existingGroup = await _dbContext.GroupTypes.FindAsync(groupTypeDto.Id);
            existingGroup.NepaliName = groupTypeDto.NepaliName;
            existingGroup.Name=groupTypeDto.Name;
            existingGroup.Schedule=groupTypeDto.Schedule;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<GroupType> GetGroupTypeById(int id)
        {
            return await _dbContext.GroupTypes
            .Include(gt => gt.AccountType)
            // .Include(gt => gt.DebitOrCredit)
            .SingleOrDefaultAsync(gt => gt.Id == id);
        }

        public async Task<GroupType> GetGroupByName(string name, string accountTypeName)
        {
            var groupTypeForDepositScheme = await _dbContext.GroupTypes
            .Where(gt => gt.Name == name.ToUpper() && gt.AccountType.Name == accountTypeName.ToUpper())
            .FirstOrDefaultAsync();
            return groupTypeForDepositScheme;
        }

        public async Task<List<GroupType>> GetGroupTypes()
        {
            return await _dbContext.GroupTypes.Include(gt => gt.AccountType)
            // .Include(gt => gt.DebitOrCredit)
            .ToListAsync();
        }

        public async Task<List<GroupType>> GetGroupTypesByAccountType(int accountTypeId)
        {
            var groupTypes =
            await _dbContext.GroupTypes
            .Include(gt => gt.AccountType)
            // .Include(gt => gt.DebitOrCredit)
            .Where(gt => gt.AccountTypeId == accountTypeId)
            .ToListAsync();
            return groupTypes;

        }


        // Operations Related to Ledger

        public async Task<bool> CheckIfLedgerNameExist(int groupTypeId, string ledgerName)
        {
            var ledger = await _dbContext.Ledgers.Where(l => l.GroupTypeId == groupTypeId && l.Name == ledgerName).FirstOrDefaultAsync();
            if (ledger != null) return true;
            return false;
        }
        public async Task<int> CreateLedger(Ledger ledger)
        {
            _logger.LogInformation($"{DateTime.Now} Creating Ledger...");
            await _dbContext.Ledgers.AddAsync(ledger);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetUniqueIdForLedger()
        {
            int numberOfLedgerEntry= await _dbContext.Ledgers.CountAsync();
            var checkIfEntryExist = await _dbContext.Ledgers.FindAsync(numberOfLedgerEntry+1);
            if(checkIfEntryExist==null)
                return numberOfLedgerEntry+1;
            
            List<int> ids = await _dbContext.Ledgers.Select(l=>l.Id).ToListAsync();
            int expectedId =0;
            for (int i = 1; i < numberOfLedgerEntry+1; i++)
            {
                if(!ids.Contains(i))
                {
                    expectedId=i;
                    break;
                }
            }
            return expectedId;
        }

        public async Task<int> EditLedger(UpdateLedgerDto ledger)
        {
            var existingLedger = await _dbContext.Ledgers.FindAsync(ledger.Id);
            if (existingLedger == null) throw new NotImplementedException("No Ledger Found");
            existingLedger.NepaliName = ledger.NepaliName;
            existingLedger.IsSubLedgerActive = ledger.IsSubLedgerActive;
            existingLedger.DepreciationRate = ledger.DepreciationRate;
            existingLedger.HisabNumber = ledger.HisabNumber;
            existingLedger.Name=ledger.Name;
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<Ledger> GetLedger(int id)
        {
            return await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt=>gt.AccountType)
            .Where(l => l.Id == id)
            .FirstOrDefaultAsync();
        }
        public async Task<List<Ledger>> GetLedgers()
        {
            return await _dbContext.Ledgers.Include(l => l.GroupType).ThenInclude(gt=>gt.AccountType)
            .ToListAsync();
            //return await _dbContext.Ledgers.ToListAsync();
        }

        public async Task<List<Ledger>> GetLedgersByAccountType(int accountTypeId)
        {
            var ledgers = await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt=>gt.AccountType)
            .Where(l => l.GroupType.AccountTypeId == accountTypeId)
            .ToListAsync();
            return ledgers;
        }

        public async Task<List<Ledger>> GetLedgerByGroupType(int groupTypeId)
        {
            var ledgers = await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt=>gt.AccountType)
            .Where(l => l.GroupTypeId == groupTypeId)
            .ToListAsync();
            return ledgers;
        }

        /// <summary>
        /// Being Used by Deposit Scheme to create a Ledger
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="ledgerName"></param>
        /// <returns></returns>
        public async Task<Ledger> GetLedgerByGroupTypeAndLedgerName(GroupType groupType, string ledgerName)
        {
            return await _dbContext.Ledgers.Where(l=>l.GroupTypeId==groupType.Id && l.Name==ledgerName.ToUpper()).FirstOrDefaultAsync();
        }



        // Operations Related to Bank Setup
        public async Task<int> CreateBankSetup(BankSetup bankSetup)
        {
            var groupType = await _dbContext.GroupTypes.Where(gt => gt.CharKhataNumber == "90").FirstOrDefaultAsync();
            Ledger ledger = await _dbContext.Ledgers.Where(l => l.GroupTypeId == groupType.Id && l.Name == bankSetup.Name.ToUpper()).FirstOrDefaultAsync();
            if (ledger == null)
            {
                int ledgerId = await GetUniqueIdForLedger();
                if(ledgerId<=0) throw new Exception("Unable to Create Ledger. No Unique Id found");
                ledger = new Ledger(){
                    Id = ledgerId,
                    GroupType = groupType,
                    Name = bankSetup.Name,
                    NepaliName = bankSetup.NepaliName,
                    EntryDate = DateTime.Now,
                    IsSubLedgerActive = false,
                    IsBank = true};
                await _dbContext.Ledgers.AddAsync(ledger);
                await _dbContext.SaveChangesAsync();
            }

                bankSetup.Ledger = ledger;
                await _dbContext.BankSetups.AddAsync(bankSetup);
                var bankId = await _dbContext.SaveChangesAsync();
                if (bankId < 1)
                {
                    _dbContext.Ledgers.Remove(ledger);
                    await _dbContext.SaveChangesAsync();
                }
            return bankId;


        }

        public async Task<int> EditBankSetup(UpdateBankSetup bankSetup)
        {
            var existingBankSetup = await _dbContext.BankSetups.FindAsync(bankSetup.Id);
            existingBankSetup.NepaliName = bankSetup.NepaliName;
            existingBankSetup.BankBranch = bankSetup.BankBranch;
            if (existingBankSetup.BankTypeId != bankSetup.BankTypeId)
                existingBankSetup.BankType = await _dbContext.BankTypes.FindAsync(bankSetup.BankTypeId);
            existingBankSetup.InterestRate = bankSetup.InterestRate;

            var existingLedger = await _dbContext.Ledgers.FindAsync(existingBankSetup.LedgerId);
            existingLedger.NepaliName=bankSetup.NepaliName;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BankSetup>> GetBankSetup()
        {
            return await _dbContext.BankSetups
            .Include(bs => bs.Ledger)
            .Include(bs => bs.BankType)
            .ToListAsync();
        }
        public async Task<BankSetup> GetBankSetupById(int id)
        {
            return await _dbContext.BankSetups
            .Include(bs => bs.Ledger)
            .Include(bs => bs.BankType)
            .SingleOrDefaultAsync(gtd => gtd.Id == id);
        }
        public async Task<List<BankSetup>> GetBankSetupByLedger(int ledgerId)
        {
            return await _dbContext.BankSetups
            .Where(bs => bs.LedgerId == ledgerId)
            .Include(bs => bs.Ledger)
            .Include(bs => bs.BankType)
            .ToListAsync();
        }

        public async Task<List<BankType>> GetAllBankType()
        {
            return await _dbContext.BankTypes.ToListAsync();
        }
        public async Task<BankType> GetBankTypeById(int id)
        {
            return await _dbContext.BankTypes.FindAsync(id);
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
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.Id == id).FirstOrDefaultAsync();
        }
        public async Task<SubLedger> GetSubLedgerById(int id)
        {
            return await _dbContext.SubLedgers.Include(sl => sl.Ledger).Where(sl => sl.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgers()
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .ToListAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgersByLedger(int ledgerId)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.LedgerId == ledgerId)
            .ToListAsync();
        }

         public async Task<List<SubLedger>> GetSubLedgersByAccountType(int accountTypeId)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.Ledger.GroupType.AccountTypeId==accountTypeId)
            .ToListAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgersByGroupType(int groupTypeId)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.Ledger.GroupTypeId==groupTypeId)
            .ToListAsync();
        }
        // DEBIT CREDIT
        public async Task<DebitOrCredit> GetDebitOrCreditById(int id)
        {
            return await _dbContext.DebitOrCredits.FindAsync(id);
        }
        public async Task<List<DebitOrCredit>> GetDebitOrCredits()
        {
            return await _dbContext.DebitOrCredits.ToListAsync();
        }

        public async Task<int> GetUniqueIdForSubLedger()
        {
            int numberOfSubLedgerEntry = await _dbContext.SubLedgers.CountAsync();
            var subLedgerExistForNewId = await _dbContext.SubLedgers.FindAsync(numberOfSubLedgerEntry+1);
            if(subLedgerExistForNewId==null) return numberOfSubLedgerEntry+1;

            List<int> ids = await _dbContext.SubLedgers.Select(sl=>sl.Id).ToListAsync();
            int expectedId =0;
            for (int i = 1; i < numberOfSubLedgerEntry+1; i++)
            {
                if(!ids.Contains(i))
                {
                    expectedId=i;
                    break;
                }
            }
            return expectedId;
        }
    }
}