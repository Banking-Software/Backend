using System.Text.RegularExpressions;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Models.AccountSetup;

namespace MicroFinance.Repository.AccountSetup.MainLedger
{
    public interface IMainLedgerRepository
    {

        // Operations related to Account Type
        Task<List<AccountType>> GetAccountTypes();
        Task<AccountType> GetAccountType(int id);

        // Operations related to Group Type
        Task<bool> CheckIfGroupNameExist(int accountTypeId, string groupName);
        Task<int> CreateGroupType(GroupType groupType);
        Task<int> UpdateGroupType(UpdateGroupTypeDto groupTypeDto);
        Task<List<GroupType>> GetGroupTypes();
        Task<List<GroupType>> GetGroupTypesByAccountType(int accountTypeId);
        Task<GroupType> GetGroupTypeById(int id);
        Task<GroupType> GetGroupTypeByCharKhataNumber(string charKhataNumber);
        Task<GroupType> GetGroupByName(string name, string accountTypeName);

        // Operations related to Group Type Details: Bank
        Task<int> CreateBankSetup(BankSetup bankSetup);
        Task<int> EditBankSetup(UpdateBankSetup bankSetup);
        Task<List<BankSetup>> GetBankSetup();
        Task<BankSetup> GetBankSetupById(int id);
        Task<List<BankSetup>> GetBankSetupByLedger(int ledgerId);
        Task<List<BankType>> GetAllBankType();
        Task<BankType> GetBankTypeById(int id);

        // Operations related to Ledger
        Task<bool> CheckIfLedgerNameExist(int groupTypeId, string ledgerName);
        Task<int> CreateLedger(Ledger ledger);
        Task<int> CreateLedgers(List<Ledger> ledgers);
        Task<int> EditLedger(UpdateLedgerDto ledger);
        Task<List<Ledger>> GetLedgersByAccountType(int accountTypeId);
        Task<List<Ledger>> GetLedgerByGroupType(int groupTypeId);
        Task<List<Ledger>> GetLedgers();
        Task<Ledger> GetLedger(int id);
        Task<Ledger> GetLedgerByGroupTypeAndLedgerName(GroupType groupType, string ledgerName);


        // Operations related to subledger
        Task<int> CreateSubLedger(SubLedger subLedger);
        Task<int> UpdateSubLedgerCode(SubLedger subLedger);
        Task<int> EditSubledger(SubLedger subLedger);
        Task<SubLedger> GetSubLedgerById(int id);
        Task<SubLedger> GetSubLedger(int id);
        Task<SubLedger> GetSubLedgerByNameAndLedgerId(string subLedgerName, int ledgerId);
        Task<List<SubLedger>> GetSubLedgers();
        Task<List<SubLedger>> GetSubLedgersByLedger(int ledgerId);
        Task<List<SubLedger>> GetSubLedgersByAccountType(int accountTypeId);
        Task<List<SubLedger>> GetSubLedgersByGroupType(int groupTypeId);

        // Debit Or Credit
        Task<DebitOrCredit> GetDebitOrCreditById(int id);
        Task<List<DebitOrCredit>> GetDebitOrCredits();
    }
}