using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.ErrorManage;
using MicroFinance.Services.AccountSetup.MainLedger;
using MicroFinance.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace MicroFinance.Controllers.AccountSetup
{
    public class AccountSetupController : AccountSetupBaseApiController
    {
        private readonly IMainLedgerService _mainLedgerService;
        private readonly ILogger<AccountSetupController> _logger;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountSetupController
        (
            IMainLedgerService mainLedgerService,
            ILogger<AccountSetupController> logger,
            IMapper mapper,
            ITokenService tokenService
        )
        {
            _mainLedgerService = mainLedgerService;
            _logger = logger;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }
        private string GetActionName()
        {
            var actionDescriptor = ControllerContext.ActionDescriptor as ControllerActionDescriptor;
            return actionDescriptor?.ActionName;
        }
        private TokenDto log()
        {
            var decodedToken = GetDecodedToken();
            string actionName = GetActionName();
            _logger.LogInformation($"{DateTime.Now}: {decodedToken.UserName} called {actionName} api");
            return decodedToken;
        }
        // GET ALL THE ACCOUNT TYPE EXISTED
        [HttpGet("getAllAccounttypes")]
        public async Task<ActionResult<List<AccountTypeDto>>> GetAccountTypes()
        {
            var decodedToken = log();
            var accountTypes = await _mainLedgerService.GetAccountTypesService();
            return Ok(accountTypes);
        }

        // GROUP TYPE

        // GET ALL THE GROUP-TYPE EXISTED WITH ALL THE RELEVENT DATA ASSOCIATED WITH IT
        [HttpGet("getAllGrouptypes")]
        public async Task<ActionResult<List<GroupTypeDto>>> GetGroupTypes()
        {
            var decodedToken = log();
            var groupTypes = await _mainLedgerService.GetGroupTypesService();
            return Ok(groupTypes);

        }

        // GET ALL THE GROUP-TYPE DATA WITH RESPECT TO ACCOUNT TYPE 
        [HttpGet("grouptypes/accounttype")]
        public async Task<ActionResult<List<GroupTypeDto>>> GetGroupTypes([FromQuery] int accountTypeId)
        {
            var decodedToken = log();
            var groupTypes = await _mainLedgerService.GetGroupTypesByAccountService(accountTypeId);
            return Ok(groupTypes);

        }


        // CREATATION OF GROUP TYPE 

        [HttpPost("createGrouptype")]
        public async Task<ActionResult<ResponseDto>> CreateGroupType(CreateGroupTypeDto createGroupTypeDto)
        {
            var decodedToken = log();
            var response = await _mainLedgerService.CreateGroupTypeService(createGroupTypeDto);
            return Ok(response);

        }

        [HttpPut("update-grouptype")]
        public async Task<ActionResult<ResponseDto>> UpdateGroupType(UpdateGroupTypeDto updateGroupTypeDto)
        {
            var decodedToken = log();
            var response = await _mainLedgerService.UpdateGroupTypeService(updateGroupTypeDto);
            return Ok(response);

        }

        // BANK SETUP

        // GET ALL THE DATA RELATED TO BANK SETUP
        // THIS TABLE STORES THE INFORMATION THAT IS RELATED TO BANKS

        [HttpGet("getAllBankSetup")]
        public async Task<ActionResult<List<BankSetupDetailsDto>>> GetAllBankSetup()
        {
            var decodedToken = log();
            var groupTypesDetails = await _mainLedgerService.GetBankSetupService();
            return Ok(groupTypesDetails);

        }

        [HttpGet("getBankSetupById")]
        public async Task<ActionResult<BankSetupDetailsDto>> GetBankSetupById([FromQuery] int id)
        {
            var decodedToken = log();
            var groupTypesDetail = await _mainLedgerService.GetBankSetupByIdService(id);
            return Ok(groupTypesDetail);

        }

        [HttpGet("bankSetup/ledger")]
        public async Task<ActionResult<List<BankSetupDetailsDto>>> GetBankSetupByLedger([FromQuery] int ledgerId)
        {
            var decodedToken = log();
            var groupTypesDetails = await _mainLedgerService.GetBankSetupByLedgerService(ledgerId);
            return Ok(groupTypesDetails);

        }

        [HttpGet("getAllBankTypes")]
        public async Task<ActionResult<List<BankTypeDto>>> GetAllBankTypes()
        {
            var decodedToken = log();
            return await _mainLedgerService.GetAllBankTypeService();
        }

        [HttpPost("creatBankSetup")]
        public async Task<ActionResult<ResponseDto>> CreateBankSetup(CreateBankSetupDto createBankSetupDto)
        {
            var decodedToken = log();
            // string branchCode = HttpContext.User.FindFirst("BranchCode").Value;
            var response = await _mainLedgerService.CreateBankSetupService(createBankSetupDto);
            return Ok(response);

        }

        [HttpPut("updateBankSetup")]
        public async Task<ActionResult<ResponseDto>> UpdateBankSetup(UpdateBankSetup bankSetupDto)
        {
            var decodedToken = log();
            var response = await _mainLedgerService.EditBankSetupService(bankSetupDto);
            return Ok(response);

        }


        // LEDGER

        [HttpGet("getAllLedgers")]
        public async Task<ActionResult<List<LedgerDto>>> GetLedgers()
        {
            var decodedToken = log();
            var ledgerDetails = await _mainLedgerService.GetLedgers();
            return Ok(ledgerDetails);

        }

        [HttpGet("getLedgerById")]
        public async Task<ActionResult<LedgerDto>> GetLedgerById([FromQuery] int id)
        {
            var decodedToken = log();
            var ledgerDetail = await _mainLedgerService.GetLedgerByIdService(id);
            return Ok(ledgerDetail);

        }

        [HttpGet("ledgers/accounttype")]
        public async Task<ActionResult<List<LedgerDto>>> GetLedgersByAccountType([FromQuery] int accountTypeId)
        {
            var decodedToken = log();
            var ledgerDetails = await _mainLedgerService.GetLedgerByAccountService(accountTypeId);
            return Ok(ledgerDetails);

        }


        [HttpGet("ledgers/grouptype")]
        public async Task<ActionResult<List<LedgerDto>>> GetLedgersByGroupType([FromQuery] int groupTypeId)
        {
            var decodedToken = log();
            var ledgerDetails = await _mainLedgerService.GetLedgerByGroupService(groupTypeId);
            return Ok(ledgerDetails);

        }

        [HttpPost("createLedger")]
        public async Task<ActionResult<ResponseDto>> CreateLedger(CreateLedgerDto createLedgerDto)
        {
            var decodedToken = log();
            var response = await _mainLedgerService.CreateLedgerService(createLedgerDto);
            return Ok(response);

        }

        [HttpPut("updateLedger")]
        public async Task<ActionResult<ResponseDto>> UpdateLedger(UpdateLedgerDto ledgerDto)
        {
            var decodedToken = log();
            var response = await _mainLedgerService.EditLedgerService(ledgerDto);
            return Ok(response);

        }

        //SUB LEDGER

        [HttpGet("getAllSubLedgers")]
        public async Task<ActionResult<List<SubLedgerDto>>> GetAllSubLedgers()
        {

            var subLedgerDetails = await _mainLedgerService.GetSubLedgersService();
            return Ok(subLedgerDetails);

        }

        [HttpGet("getSubLedgerById")]
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

        [HttpPost("createSubLedger")]
        public async Task<ActionResult<ResponseDto>> CreateSubLedger(CreateSubLedgerDto createSubLedgerDto)
        {

            var response = await _mainLedgerService.CreateSubLedgerService(createSubLedgerDto);
            return Ok(response);

        }

        [HttpPut("updateSubLedger")]
        public async Task<ActionResult<ResponseDto>> UpdateSubLedger(UpdateSubLedgerDto subLedgerDto)
        {

            var response = await _mainLedgerService.EditSubLedgerService(subLedgerDto);
            return Ok(response);

        }
    }
}