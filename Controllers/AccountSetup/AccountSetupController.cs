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
        public async Task<ActionResult<List<GroupTypeAccounTypeDetailsDto>>> GetGroupTypes()
        {

            var groupTypes = await _mainLedgerService.GetGroupTypesService();
            return Ok(groupTypes);

        }

        // GET ALL THE GROUP-TYPE DATA WITH RESPECT TO ACCOUNT TYPE 
        [HttpGet("grouptypes/accounttype")]
        public async Task<ActionResult<List<GroupTypeAccounTypeDetailsDto>>> GetGroupTypes([FromQuery] int accountTypeId)
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

        // GROUP TYPE DETAILS

        // GET ALL THE DATA RELATED TO GROUP-TYPE-DETAILS
        // THIS TABLE STORES THE INFORMATION THAT IS RELATED TO BANKS

        [HttpGet("grouptypedetails")]
        public async Task<ActionResult<List<GroupTypeDetailsMappingDetailsDto>>> GetGroupTypeDetails()
        {

            var groupTypesDetails = await _mainLedgerService.GetGroupTypeDetailsService();
            return Ok(groupTypesDetails);

        }

        // GET GROUP-TYPE-DETAILS WITH RESPECT TO ITS ID

        [HttpGet("grouptypedetail")]
        public async Task<ActionResult<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetail([FromQuery] int id)
        {

            var groupTypesDetail = await _mainLedgerService.GetGroupTypeDetailsByIdService(id);
            return Ok(groupTypesDetail);

        }

        // GET ALL THE GROUP-TYPE-DETAILS WITH RESPECT TO ACCOUNT-TYPE 
        [HttpGet("grouptypedetails/accounttype")]
        public async Task<ActionResult<List<GroupTypeDetailsMappingDetailsDto>>> GetGroupTypeDetailsByAccountType([FromQuery] int accountTypeId)
        {

            var groupTypesDetails = await _mainLedgerService.GetGroupTypeDetailsByAccountType(accountTypeId);
            return Ok(groupTypesDetails);

        }

        // GET ALL THE GROUP-TYPE-DETAILS WITH RESPECT TO GROUP-TYPE 
        [HttpGet("grouptypedetails/grouptype")]
        public async Task<ActionResult<List<GroupTypeDetailsMappingDetailsDto>>> GetGroupTypeDetailsByGroupType([FromQuery] int groupTypeId)
        {

            var groupTypesDetails = await _mainLedgerService.GetGroupTypeDetailsByGroupType(groupTypeId);
            return Ok(groupTypesDetails);

        }


        // CREATE GROUP-TYPE-DETAILS
        [HttpPost("grouptypedetails")]
        public async Task<ActionResult<ResponseDto>> CreateGroupTypeDetails(CreateGroupTypeDetailsDto createGroupTypeDetailsDto)
        {

            var response = await _mainLedgerService.CreateGroupTypeDetailsService(createGroupTypeDetailsDto);
            return Ok(response);

        }
        // EDIT GROUP-TYPE-DETAILS 
        [HttpPut("grouptypedetails")]
        public async Task<ActionResult<ResponseDto>> EditGroupTypeDetails(GroupTypeDetailsDto groupTypeDetailsDto)
        {

            var response = await _mainLedgerService.EditGroupTypeDetailsService(groupTypeDetailsDto);
            return Ok(response);

        }


        // LEDGER

        [HttpGet("ledgers")]
        public async Task<ActionResult<List<LedgerDetailsDto>>> GetLedgerDetails()
        {

            var ledgerDetails = await _mainLedgerService.GetLedgers();
            return Ok(ledgerDetails);

        }

        [HttpGet("ledger")]
        public async Task<ActionResult<LedgerDetailsDto>> GetLedgerDetail([FromQuery] int id)
        {

            var ledgerDetail = await _mainLedgerService.GetLedgerByIdService(id);
            return Ok(ledgerDetail);

        }

        [HttpGet("ledgers/accounttype")]
        public async Task<ActionResult<List<LedgerDetailsDto>>> GetLedgerDetailsByAccountType([FromQuery] int accountTypeId)
        {

            var ledgerDetails = await _mainLedgerService.GetLedgerByAccountService(accountTypeId);
            return Ok(ledgerDetails);

        }


        [HttpGet("ledgers/grouptype")]
        public async Task<ActionResult<List<LedgerDetailsDto>>> GetLedgerDetailsByGroupType([FromQuery] int groupTypeId)
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
        public async Task<ActionResult<ResponseDto>> EditLedger(LedgerDto ledgerDto)
        {

            var response = await _mainLedgerService.EditLedgerService(ledgerDto);
            return Ok(response);

        }

        //SUB LEDGER

        [HttpGet("subledgers")]
        public async Task<ActionResult<List<SubLedgerDetailsDto>>> GetSubLedgers()
        {

            var subLedgerDetails = await _mainLedgerService.GetSubLedgersService();
            return Ok(subLedgerDetails);

        }

        [HttpGet("subledger")]
        public async Task<ActionResult<SubLedgerDetailsDto>> GetSubLedger([FromQuery] int id)
        {

            var subLedgerDetail = await _mainLedgerService.GetSubLedgerByIdService(id);
            return Ok(subLedgerDetail);

        }

        [HttpGet("subledgers/ledger")]
        public async Task<ActionResult<List<LedgerDetailsDto>>> GetSubLedgerDetailsByLedger([FromQuery] int ledgerId)
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
        public async Task<ActionResult<ResponseDto>> EditSubLedger(SubLedgerDto subLedgerDto)
        {

            var response = await _mainLedgerService.EditSubLedgerService(subLedgerDto);
            return Ok(response);

        }
    }
}