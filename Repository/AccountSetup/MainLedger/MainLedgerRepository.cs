using MicroFinance.DBContext;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Exceptions;
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
        public async Task<AccountType> GetAccountType(int id)
        {
            _logger.LogInformation($"{DateTime.Now} (GetAccountType) Returing...");
            return await _dbContext.AccountTypes.Where(at=>at.Id==id).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<List<AccountType>> GetAccountTypes()
        {
            var accountTypes = await _dbContext.AccountTypes
            .AsNoTracking()
            .ToListAsync();
            return accountTypes;
        }

        // END

        // Operations Related to Group
        public async Task<bool> CheckIfGroupNameExist(int accountTypeId, string groupName)
        {
            var group = await _dbContext.GroupTypes
            .Where(gt => gt.AccountTypeId == accountTypeId && gt.Name == groupName)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            if (group != null) return true;
            return false;
        }
        public async Task<int> CreateGroupType(GroupType groupType)
        {
            _logger.LogInformation($"{DateTime.Now} (CreateGroupType) Creating...");
            groupType.AccountType = await _dbContext.AccountTypes.FindAsync(groupType.Id);
            await _dbContext.GroupTypes.AddAsync(groupType);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(groupType).State = EntityState.Detached;
            return groupType.Id;
        }

        public async Task<int> UpdateGroupType(UpdateGroupTypeDto groupTypeDto)
        {
            var existingGroup = await _dbContext.GroupTypes.FindAsync(groupTypeDto.Id);
            if(existingGroup == null) throw new NotFoundExceptionHandler("No Data Found for requested Group Type");
            existingGroup.NepaliName = groupTypeDto.NepaliName;
            existingGroup.Name = groupTypeDto.Name;
            existingGroup.Schedule = groupTypeDto.Schedule;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<GroupType> GetGroupTypeById(int id)
        {
            return await _dbContext.GroupTypes
            .Include(gt => gt.AccountType)
            .AsNoTracking()
            .SingleOrDefaultAsync(gt => gt.Id == id);
        }

        public async Task<GroupType> GetGroupByName(string name, string accountTypeName)
        {
            var groupTypeForDepositScheme = await _dbContext.GroupTypes
            .Where(gt => gt.Name == name.ToUpper() && gt.AccountType.Name == accountTypeName.ToUpper())
            .AsNoTracking()
            .FirstOrDefaultAsync();
            return groupTypeForDepositScheme;
        }

        public async Task<List<GroupType>> GetGroupTypes()
        {
            return await _dbContext.GroupTypes.Include(gt => gt.AccountType)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<GroupType>> GetGroupTypesByAccountType(int accountTypeId)
        {
            var groupTypes =
            await _dbContext.GroupTypes
            .Include(gt => gt.AccountType)
            .Where(gt => gt.AccountTypeId == accountTypeId)
            .AsNoTracking()
            .ToListAsync();
            return groupTypes;

        }


        // Operations Related to Ledger

        public async Task<bool> CheckIfLedgerNameExist(int groupTypeId, string ledgerName)
        {
            var ledger = await _dbContext.Ledgers
            .Where(l => l.GroupTypeId == groupTypeId && l.Name == ledgerName)
            .AsNoTracking()
            .FirstOrDefaultAsync();
            if (ledger != null) return true;
            return false;
        }
        public async Task<int> CreateLedger(Ledger ledger)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                _logger.LogInformation($"{DateTime.Now} Creating Ledger...");
                try
                {
                    ledger.GroupType = await _dbContext.GroupTypes.FindAsync(ledger.GroupTypeId);
                    await _dbContext.Ledgers.AddAsync(ledger);
                    var status = await _dbContext.SaveChangesAsync();
                    if (status <= 0) throw new Exception("Unable to Create Ledger");
                    var codeUpdate = await UpdateLedgerCode(ledger);
                    if (codeUpdate <= 0) throw new Exception("Unable to assign the code");
                    await transaction.CommitAsync();
                    return ledger.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

        }
        private async Task<int> UpdateLedgerCode(Ledger ledger)
        {
            var existingledger = await _dbContext.Ledgers.FindAsync(ledger.Id);
            _dbContext.Entry(existingledger).State = EntityState.Detached;
            _dbContext.Ledgers.Attach(ledger);
            existingledger.LedgerCode = ledger.Id;
            _dbContext.Entry(ledger).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> EditLedger(UpdateLedgerDto ledger)
        {
            var existingLedger = await _dbContext.Ledgers.FindAsync(ledger.Id);
            if (existingLedger == null) throw new NotImplementedException("No Ledger Found");
            existingLedger.NepaliName = ledger.NepaliName;
            existingLedger.IsSubLedgerActive = ledger.IsSubLedgerActive;
            existingLedger.DepreciationRate = ledger.DepreciationRate;
            existingLedger.HisabNumber = ledger.HisabNumber;
            existingLedger.Name = ledger.Name;
            if(existingLedger.IsBank==true)
            {
                var exisitngBank =  await _dbContext.BankSetups.Where(bs=>bs.LedgerId==existingLedger.Id).FirstOrDefaultAsync();
                exisitngBank.NepaliName = existingLedger.NepaliName;
                exisitngBank.Name = existingLedger.Name; 
            }
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<Ledger> GetLedger(int id)
        {
            return await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(l => l.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }
        public async Task<List<Ledger>> GetLedgers()
        {
            return await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .AsNoTracking()
            .ToListAsync();
            //return await _dbContext.Ledgers.ToListAsync();
        }

        public async Task<List<Ledger>> GetLedgersByAccountType(int accountTypeId)
        {
            var ledgers = await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(l => l.GroupType.AccountTypeId == accountTypeId)
            .AsNoTracking()
            .ToListAsync();
            return ledgers;
        }

        public async Task<List<Ledger>> GetLedgerByGroupType(int groupTypeId)
        {
            var ledgers = await _dbContext.Ledgers
            .Include(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(l => l.GroupTypeId == groupTypeId)
            .AsNoTracking()
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
            return await _dbContext.Ledgers
            .Where(l => l.GroupTypeId == groupType.Id && l.Name == ledgerName.ToUpper())
            .AsNoTracking()
            .FirstOrDefaultAsync();
        }

        private async Task CreateLedgerForBank(Ledger ledger)
        {
            await _dbContext.Ledgers.AddAsync(ledger);
            var ledgerUpdate = await _dbContext.SaveChangesAsync();
            if(ledgerUpdate<=0) throw new Exception("Not able to create ledger for bank");
            await UpdateLedgerCode(ledger);
        }

        // Operations Related to Bank Setup
        public async Task<int> CreateBankSetup(BankSetup bankSetup)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    bankSetup.BankType = await _dbContext.BankTypes.FindAsync(bankSetup.BankTypeId);
                    var groupType = await _dbContext.GroupTypes.Where(gt => gt.CharKhataNumber == "90").FirstOrDefaultAsync();
                    Ledger ledger = await _dbContext.Ledgers.Where(l => l.GroupTypeId == groupType.Id && l.Name == bankSetup.Name.ToUpper()).FirstOrDefaultAsync();
                    if (ledger != null) throw new Exception("Ledger for Bank already exist");
                    ledger = new Ledger()
                    {
                        GroupType = groupType,
                        Name = bankSetup.Name,
                        NepaliName = bankSetup.NepaliName,
                        EntryDate = DateTime.Now,
                        IsSubLedgerActive = false,
                        IsBank = true
                    };
                    await CreateLedgerForBank(ledger);
                    bankSetup.Ledger = ledger;
                    await _dbContext.BankSetups.AddAsync(bankSetup);
                    var addStatus = await _dbContext.SaveChangesAsync();
                    if(addStatus<=0) throw new Exception("Unable to create bank");
                    await transaction.CommitAsync();
                    return bankSetup.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

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
            existingLedger.NepaliName = bankSetup.NepaliName;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BankSetup>> GetBankSetup()
        {
            return await _dbContext.BankSetups
            .Include(bs => bs.Ledger)
            .Include(bs => bs.BankType)
            .AsNoTracking()
            .ToListAsync();
        }
        public async Task<BankSetup> GetBankSetupById(int id)
        {
            return await _dbContext.BankSetups
            .Include(bs => bs.Ledger)
            .Include(bs => bs.BankType)
            .AsNoTracking()
            .SingleOrDefaultAsync(gtd => gtd.Id == id);
        }
        public async Task<List<BankSetup>> GetBankSetupByLedger(int ledgerId)
        {
            return await _dbContext.BankSetups
            .Where(bs => bs.LedgerId == ledgerId)
            .Include(bs => bs.Ledger)
            .Include(bs => bs.BankType)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<BankType>> GetAllBankType()
        {
            return await _dbContext.BankTypes.AsNoTracking().ToListAsync();
        }
        public async Task<BankType> GetBankTypeById(int id)
        {
            return await _dbContext.BankTypes.Where(bs=>bs.Id==id).AsNoTracking().SingleOrDefaultAsync();
        }
        // Operation related to Subledger
        public async Task<int> CreateSubLedger(SubLedger subLedger)
        {
            using (var processTransaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    subLedger.Ledger = await _dbContext.Ledgers.FindAsync(subLedger.LedgerId);
                    await _dbContext.SubLedgers.AddAsync(subLedger);
                    var subLedgerCreateStatus = await _dbContext.SaveChangesAsync();
                    if (subLedgerCreateStatus <= 0) throw new Exception("Unable to create subledger");
                    var updateCodeStatus = await UpdateSubLedgerCode(subLedger);
                    if (updateCodeStatus <= 0) throw new Exception("Unable To access SubLedger Code");
                    await processTransaction.CommitAsync();
                    return subLedger.Id;
                }
                catch (Exception ex)
                {
                    await processTransaction.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<int> UpdateSubLedgerCode(SubLedger subLedger)
        {
            var existingSubLedger = await _dbContext.SubLedgers.FindAsync(subLedger.Id);
            _dbContext.Entry(existingSubLedger).State = EntityState.Detached;
            subLedger.SubLedgerCode = subLedger.Id;
            _dbContext.SubLedgers.Attach(subLedger);
            _dbContext.Entry(subLedger).State = EntityState.Modified;
            var status = await _dbContext.SaveChangesAsync();
            return status;
        }

        public async Task<int> EditSubledger(SubLedger subLedger)
        {
            var existingSubLedger = await _dbContext.SubLedgers.FindAsync(subLedger.Id);
            existingSubLedger.Name = subLedger.Name;
            existingSubLedger.Description = subLedger.Description;
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<SubLedger> GetSubLedger(int id)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .AsNoTracking()
            .Where(sl => sl.Id == id).FirstOrDefaultAsync();
        }
        public async Task<SubLedger> GetSubLedgerById(int id)
        {
            return await _dbContext.SubLedgers.Include(sl => sl.Ledger).Where(sl => sl.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<SubLedger> GetSubLedgerByNameAndLedgerId(string subLedgerName, int ledgerId)
        {
            return await _dbContext.SubLedgers.Where(sl => sl.Name == subLedgerName && sl.LedgerId == ledgerId).AsNoTracking().SingleOrDefaultAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgers()
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgersByLedger(int ledgerId)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.LedgerId == ledgerId)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgersByAccountType(int accountTypeId)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.Ledger.GroupType.AccountTypeId == accountTypeId)
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<List<SubLedger>> GetSubLedgersByGroupType(int groupTypeId)
        {
            return await _dbContext.SubLedgers
            .Include(sl => sl.Ledger)
            .ThenInclude(l => l.GroupType)
            .ThenInclude(gt => gt.AccountType)
            .Where(sl => sl.Ledger.GroupTypeId == groupTypeId)
            .AsNoTracking()
            .ToListAsync();
        }
        // DEBIT CREDIT
        public async Task<DebitOrCredit> GetDebitOrCreditById(int id)
        {
            return await _dbContext.DebitOrCredits.Where(dc=>dc.Id==id).AsNoTracking().SingleOrDefaultAsync();
        }
        public async Task<List<DebitOrCredit>> GetDebitOrCredits()
        {
            return await _dbContext.DebitOrCredits.AsNoTracking().ToListAsync();
        }
    }
}