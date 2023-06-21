using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;

namespace MicroFinance.Services.AccountSetup.MainLedger
{
    public interface IMainLedgerService
    {

        // STRAT: ACCOUNT TYPE
        // Task<ResponseDto> CreateAccountTypeService(CreateAccountTypeDto createAccountTypeDto);
        Task<AccountTypeDto> GetAccountTypeByIdService(int id);
        Task<List<AccountTypeDto>> GetAccountTypesService();

        // END

        //START: GROUP TYPE

        Task<ResponseDto> CreateGroupTypeService(CreateGroupTypeDto createGroupTypeDto);
        Task<ResponseDto> UpdateGroupTypeService(UpdateGroupTypeDto updateGroupTypeDto);
        Task<List<GroupTypeDto>> GetGroupTypesByAccountService(int accountTypeId);
        Task<GroupTypeDto> GetGroupTypeByIdService(int id);
        Task<List<GroupTypeDto>> GetGroupTypesService();

        // END

        // START: LEDGER
        Task<ResponseDto> CreateLedgerService(CreateLedgerDto createLedgerDto);
        Task<int> GetUniqueIdForLedgerService();
        Task<ResponseDto> EditLedgerService(UpdateLedgerDto ledgerDto);
        Task<LedgerDto> GetLedgerByIdService(int id);
        Task<List<LedgerDto>> GetLedgers();
        Task<List<LedgerDto>> GetLedgerByAccountService(int accountTypeId);
        Task<List<LedgerDto>> GetLedgerByGroupService(int groupTypeId);

        // END

        // START: BANK SETUP DETAILS
        Task<ResponseDto> CreateBankSetupService(CreateBankSetupDto createBankSetupDto, string branchCode);
        Task<ResponseDto> EditBankSetupService(UpdateBankSetup bankSetupDto);
        Task<List<BankSetupDto>> GetBankSetupService();
        Task<BankSetupDto> GetBankSetupByIdService(int id);
        Task<List<BankSetupDto>> GetBankSetupByLedgerService(int ledgerId);

        Task<List<BankTypeDto>> GetAllBankTypeService();
        Task<BankTypeDto> GetBankTypeByIdService(int id);

        // START: SUB LEDGER

        // First check if entry exist and only then create ledger and map it
        Task<ResponseDto> CreateSubLedgerService(CreateSubLedgerDto createSubLedgerDto);
        Task<int> GetUniqueIdForSubLedgerService();
        Task<ResponseDto> EditSubLedgerService(UpdateSubLedgerDto subLedgerDto);
        Task<List<SubLedgerDto>> GetSubLedgerByLedgerService(int ledgerId);
        Task<SubLedgerDto> GetSubLedgerByIdService(int id);
        Task<List<SubLedgerDto>> GetSubLedgersService();
    }
}