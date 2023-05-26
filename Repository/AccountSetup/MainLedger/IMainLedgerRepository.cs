using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Repository.AccountSetup.MainLedger
{
    public interface IMainLedgerRepository
    {

        // Operations related to Account Type
        Task<int> CreateAccountType(AccountType accountType);
        Task<List<AccountType>> GetAccountTypes();
        Task<AccountType> GetAccountType(int id);

        // Operations related to Group Type
        Task<int> CreateGroupType(GroupType groupType);
        Task<List<GroupType>> GetGroupTypes();
        Task<List<GroupType>> GetGroupTypesByAccountType(int accountTypeId);
        Task<GroupType> GetGroupTypeById(int id);
        Task<GroupType> GetGroupByName(string name, string accountTypeName);

        // Operations related to Group Type Details: Bank
        Task<int> CreateGroupTypeDetails(GroupTypeDetails groupTypeDetails);
        Task<int> EditGroupTypeDetails(GroupTypeDetails groupTypeDetails);
        Task<List<GroupTypeDetails>> GetGroupTypeDetails();
        Task<GroupTypeDetails> GetGroupTypeDetailsById(int id);
        Task<List<GroupTypeDetails>> GetGroupTypeDetailsByGroupType(int groupTypeId);
        Task<List<GroupTypeDetails>> GetGroupTypeDetailsByAccountType(int accountTypeId);

        // Operations related to Ledger
        Task<int> CreateLedger(Ledger ledger, GroupType groupType);
        Task<int> EditLedger(Ledger ledger);
        Task<List<GroupTypeAndLedgerMap>> GetLedgers();
        Task<GroupTypeAndLedgerMap> GetLedgerById(int id);
        Task<Ledger> GetLedger(int id);

        Task<Ledger> GetLedgerByGroupTypeAndLedgerName(GroupType groupType, string name);

        // Operations related to subledger
        Task<int> CreateSubLedger(SubLedger subLedger);
        Task<int> EditSubledger(SubLedger subLedger);
        Task<GroupSubLedger> GetSubLedgerById(int id);
        Task<SubLedger> GetSubLedger(int id);
        Task<List<GroupSubLedger>> GetSubLedgers();
        Task<List<GroupSubLedger>> GetSubLedgersByLedger(int ledgerId);

        // Operations related AccountGroupLedgerMap
        Task<bool> CheckIfLedgerAndGroupNameExist(int grouptypeId, string ledgerName);
        Task<int> CreateGroupTypeAndLedgerMap(GroupTypeAndLedgerMap groupTypeAndLedgerMap);
        Task<List<GroupTypeAndLedgerMap>> GetGroupTypeAndLedgerMaps();
        Task<GroupTypeAndLedgerMap> GetGroupTypeAndLedgerMapById(int id);
        Task<List<GroupTypeAndLedgerMap>> GetGroupTypeAndLedgerMapByAccountType(int accountTypeId);
        Task<List<GroupTypeAndLedgerMap>> GetGroupTypeAndLedgerMapByGroupType(int groupTypeId);
    }
}