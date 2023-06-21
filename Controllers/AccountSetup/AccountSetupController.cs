using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.ErrorManage;
using MicroFinance.Services.AccountSetup.MainLedger;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.AccountSetup
{
    public class AccountSetupController : AccountSetupBaseApiController
    {
        private readonly IMainLedgerService _mainLedgerService;
        private readonly ILogger<AccountSetupController> _logger;
        private readonly IMapper _mapper;

        public AccountSetupController
        (
            IMainLedgerService mainLedgerService,
            ILogger<AccountSetupController> logger,
            IMapper mapper
        )
        {
            _mainLedgerService = mainLedgerService;
            _logger = logger;
            _mapper = mapper;
        }


        // GET ALL THE ACCOUNT TYPE EXISTED
        [HttpGet("accounttypes")]
        public async Task<ActionResult<List<AccountTypeDto>>> GetAccountTypes()
        {
            var accountTypes = await _mainLedgerService.GetAccountTypesService();
            return Ok(accountTypes);
        }

        // GROUP TYPE

        // GET ALL THE GROUP-TYPE EXISTED WITH ALL THE RELEVENT DATA ASSOCIATED WITH IT
        [HttpGet("grouptypes")]
        public async Task<ActionResult<List<GroupTypeDto>>> GetGroupTypes()
        {

            var groupTypes = await _mainLedgerService.GetGroupTypesService();
            return Ok(groupTypes);

        }

        // GET ALL THE GROUP-TYPE DATA WITH RESPECT TO ACCOUNT TYPE 
        [HttpGet("grouptypes/accounttype")]
        public async Task<ActionResult<List<GroupTypeDto>>> GetGroupTypes([FromQuery] int accountTypeId)
        {

            var groupTypes = await _mainLedgerService.GetGroupTypesByAccountService(accountTypeId);
            return Ok(groupTypes);

        }


        // CREATATION OF GROUP TYPE 

        [HttpPost("grouptype")]
        public async Task<ActionResult<ResponseDto>> CreateGroupType(CreateGroupTypeDto createGroupTypeDto)
        {

            var response = await _mainLedgerService.CreateGroupTypeService(createGroupTypeDto);
            return Ok(response);

        }

        [HttpPut("update-grouptype")]
        public async Task<ActionResult<ResponseDto>> UpdateGroupType(UpdateGroupTypeDto updateGroupTypeDto)
        {

            var response = await _mainLedgerService.UpdateGroupTypeService(updateGroupTypeDto);
            return Ok(response);

        }

        // BANK SETUP

        // GET ALL THE DATA RELATED TO BANK SETUP
        // THIS TABLE STORES THE INFORMATION THAT IS RELATED TO BANKS

        [HttpGet("banksetups")]
        public async Task<ActionResult<List<BankSetupDetailsDto>>> GetAllBankSetup()
        {

            var groupTypesDetails = await _mainLedgerService.GetBankSetupService();
            return Ok(groupTypesDetails);

        }

        [HttpGet("banksetup")]
        public async Task<ActionResult<BankSetupDetailsDto>> GetBankSetupById([FromQuery] int id)
        {

            var groupTypesDetail = await _mainLedgerService.GetBankSetupByIdService(id);
            return Ok(groupTypesDetail);

        }

        // GET ALL THE GROUP-TYPE-DETAILS WITH RESPECT TO ACCOUNT-TYPE 
        // [HttpGet("grouptypedetails/accounttype")]
        // public async Task<ActionResult<List<BankSetupDetailsDto>>> GetBankSetupByAccountType([FromQuery] int accountTypeId)
        // {

        //     var groupTypesDetails = await _mainLedgerService.GetBankSetupByAccountType(accountTypeId);
        //     return Ok(groupTypesDetails);

        // }

        // GET ALL THE GROUP-TYPE-DETAILS WITH RESPECT TO GROUP-TYPE 
        // [HttpGet("grouptypedetails/grouptype")]
        // public async Task<ActionResult<List<BankSetupDetailsDto>>> GetBankSetupByGroupType([FromQuery] int groupTypeId)
        // {

        //     var groupTypesDetails = await _mainLedgerService.GetBankSetupByGroupType(groupTypeId);
        //     return Ok(groupTypesDetails);

        // }

        [HttpGet("banksetup/ledger")]
        public async Task<ActionResult<List<BankSetupDetailsDto>>> GetBankSetupByLedger([FromQuery] int ledgerId)
        {

            var groupTypesDetails = await _mainLedgerService.GetBankSetupByLedgerService(ledgerId);
            return Ok(groupTypesDetails);

        }

        [HttpGet("banktypes")]
        public async Task<ActionResult<List<BankTypeDto>>> GetAllBankTypes()
        {
            return await _mainLedgerService.GetAllBankTypeService();
        }

        [HttpPost("banksetup")]
        public async Task<ActionResult<ResponseDto>> CreateBankSetup(CreateBankSetupDto createBankSetupDto)
        {
            string branchCode = HttpContext.User.FindFirst("BranchCode").Value;
            var response = await _mainLedgerService.CreateBankSetupService(createBankSetupDto, branchCode);
            return Ok(response);

        }

        [HttpPut("update-banksetup")]
        public async Task<ActionResult<ResponseDto>> EditBankSetup(UpdateBankSetup bankSetupDto)
        {

            var response = await _mainLedgerService.EditBankSetupService(bankSetupDto);
            return Ok(response);

        }


        // LEDGER

        [HttpGet("ledgers")]
        public async Task<ActionResult<List<LedgerDto>>> GetLedgers()
        {

            var ledgerDetails = await _mainLedgerService.GetLedgers();
            return Ok(ledgerDetails);

        }

        [HttpGet("unique-ledgerId")]
        public async Task<ActionResult<int>> GetUniqueIdForLedger()
        {
            int id = await _mainLedgerService.GetUniqueIdForLedgerService();
            return id;
        }

        [HttpGet("ledger")]
        public async Task<ActionResult<LedgerDto>> GetLedgerById([FromQuery] int id)
        {

            var ledgerDetail = await _mainLedgerService.GetLedgerByIdService(id);
            return Ok(ledgerDetail);

        }

        [HttpGet("ledgers/accounttype")]
        public async Task<ActionResult<List<LedgerDto>>> GetLedgersByAccountType([FromQuery] int accountTypeId)
        {

            var ledgerDetails = await _mainLedgerService.GetLedgerByAccountService(accountTypeId);
            return Ok(ledgerDetails);

        }


        [HttpGet("ledgers/grouptype")]
        public async Task<ActionResult<List<LedgerDetailsDto>>> GetLedgersByGroupType([FromQuery] int groupTypeId)
        {

            var ledgerDetails = await _mainLedgerService.GetLedgerByGroupService(groupTypeId);
            return Ok(ledgerDetails);

        }

        [HttpPost("ledger")]
        public async Task<ActionResult<ResponseDto>> CreateLedger(CreateLedgerDto createLedgerDto)
        {

            var response = await _mainLedgerService.CreateLedgerService(createLedgerDto);
            return Ok(response);

        }

        [HttpPut("ledger")]
        public async Task<ActionResult<ResponseDto>> EditLedger(UpdateLedgerDto ledgerDto)
        {

            var response = await _mainLedgerService.EditLedgerService(ledgerDto);
            return Ok(response);

        }

        //SUB LEDGER

        [HttpGet("subledgers")]
        public async Task<ActionResult<List<SubLedgerDto>>> GetSubLedgers()
        {

            var subLedgerDetails = await _mainLedgerService.GetSubLedgersService();
            return Ok(subLedgerDetails);

        }

        [HttpGet("subledger")]
        public async Task<ActionResult<SubLedgerDto>> GetSubLedger([FromQuery] int id)
        {

            var subLedgerDetail = await _mainLedgerService.GetSubLedgerByIdService(id);
            return Ok(subLedgerDetail);

        }

        [HttpGet("subledgers/ledger")]
        public async Task<ActionResult<List<SubLedgerDto>>> GetSubLedgerDetailsByLedger([FromQuery] int ledgerId)
        {

            var subLedgerDetails = await _mainLedgerService.GetSubLedgerByLedgerService(ledgerId);
            return Ok(subLedgerDetails);

        }

        [HttpPost("subledger")]
        public async Task<ActionResult<ResponseDto>> CreateSubLedger(CreateSubLedgerDto createSubLedgerDto)
        {

            var response = await _mainLedgerService.CreateSubLedgerService(createSubLedgerDto);
            return Ok(response);

        }

        [HttpPut("subledger")]
        public async Task<ActionResult<ResponseDto>> EditSubLedger(UpdateSubLedgerDto subLedgerDto)
        {

            var response = await _mainLedgerService.EditSubLedgerService(subLedgerDto);
            return Ok(response);

        }

        [HttpGet("unique-subledgerId")]
        public async Task<ActionResult<int>> GetUniqueIdForSubLedger()
        {
            int id = await _mainLedgerService.GetUniqueIdForSubLedgerService();
            return id;
        }
    }
}