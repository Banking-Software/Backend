using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;

namespace MicroFinance.Services.AccountSetup.MainLedger
{
    public interface IMainLedgerService
    {

        // STRAT: ACCOUNT TYPE
        Task<ResponseDto> CreateAccountTypeService(CreateAccountTypeDto createAccountTypeDto);
        Task<AccountTypeDto> GetAccountTypeByIdService(int id);
        Task<List<AccountTypeDto>> GetAccountTypesService();

        // END

        //START: GROUP TYPE

        Task<ResponseDto> CreateGroupTypeService(CreateGroupTypeDto createGroupTypeDto);
        Task<List<GroupTypeAccounTypeDetailsDto>> GetGroupTypesByAccountService(int accountTypeId);
        Task<GroupTypeAccounTypeDetailsDto> GetGroupTypeByIdService(int id);
        Task<List<GroupTypeAccounTypeDetailsDto>> GetGroupTypesService();

        // END

        // START: GROUP DETAILS
        Task<ResponseDto> CreateGroupTypeDetailsService(CreateGroupTypeDetailsDto createGroupTypeDetailsDto);
        Task<ResponseDto> EditGroupTypeDetailsService(GroupTypeDetailsDto groupTypeDetailsDto);
        Task<List<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetailsService();
        Task<GroupTypeDetailsMappingDetailsDto> GetGroupTypeDetailsByIdService(int id);
        Task<List<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetailsByGroupType(int groupTypeId);
        Task<List<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetailsByAccountType(int accountTypeId);

        // START: LEDGER

        Task<ResponseDto> CreateLedgerService(CreateLedgerDto createLedgerDto);
        Task<ResponseDto> EditLedgerService(LedgerDto ledgerDto);
        Task<LedgerDetailsDto> GetLedgerByIdService(int id);
        Task<List<LedgerDetailsDto>> GetLedgers();
        Task<List<LedgerDetailsDto>> GetLedgerByAccountService(int accountTypeId);
        Task<List<LedgerDetailsDto>> GetLedgerByGroupService(int groupTypeId);

        // END

        // START: SUB LEDGER

        // First check if entry exist and only then create ledger and map it
        Task<ResponseDto> CreateSubLedgerService(CreateSubLedgerDto createSubLedgerDto);
        
        Task<ResponseDto> EditSubLedgerService(SubLedgerDto subLedgerDto);
        Task<List<SubLedgerDetailsDto>> GetSubLedgerByLedgerService(int ledgerId);
        Task<SubLedgerDetailsDto> GetSubLedgerByIdService(int id);
        Task<List<SubLedgerDetailsDto>> GetSubLedgersService();

        // START: GROUPLEDGER MAP

        // Task<GroupLedgerDto> GetAccountGroupLedgerMapsByIdService(int id);
        // Task<List<GroupLedgerDto>> GetAccountGroupLedgerMapsService();
        
    }
}