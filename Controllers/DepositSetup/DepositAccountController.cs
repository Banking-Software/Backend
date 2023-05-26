using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Services;
using MicroFinance.Services.DepositSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.DepositSetup
{

    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class DepositAccountController : BaseApiController
    {
        private readonly ILogger<DepositAccountController> _logger;
        private readonly IDepositSchemeService _depositService;

        public DepositAccountController(ILogger<DepositAccountController> logger, IDepositSchemeService depositService)
        {
            _logger = logger;
            _depositService = depositService;
        }

        private void LogInformation(string functionName, string userName)
        {
            _logger.LogInformation($"{DateTime.Now}: {userName} is accessing {functionName}");
            return;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateDepositAccount(CreateDepositAccountDto createDepositAccountDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("CreateDepositAccount", userName);
            return Ok(await _depositService.CreateDepositAccountService(createDepositAccountDto, userName));

        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateDepositScheme(UpdateDepositAccountDto updateDepositAccountDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("UpdateDepositScheme", userName);
            return Ok(await _depositService.UpdateDepositAccountService(updateDepositAccountDto, userName));

        }

        [HttpGet("all")]
        public async Task<ActionResult<List<DepositAccountDto>>> GetAllDepositAccount()
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("GetAllDepositAccount", userName);
            return Ok(await _depositService.GetAllDepositAccountService());
        }

        [HttpGet("id")]
        public async Task<ActionResult<DepositAccountDto>> GetDepositAccountById([FromQuery] int id)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("GetDepositAccountById", userName);
            return Ok(await _depositService.GetDepositAccountByIdService(id));
        }

        [HttpGet("uniqueAccountNumber")]
        public async Task<ActionResult<AccountNumberDto>> GetUniqueAccountNumber([FromQuery] int depositSchemeId)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("GetUniqueAccountNumber", userName);
            return Ok(await _depositService.GetUniqueAccountNumberService(depositSchemeId));
        }

        [HttpGet("accountNumber")]
        public async Task<ActionResult<DepositAccountDto>> GetDepositAccountByAccountNumber([FromQuery] string accountNumber)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("GetDepositAccountByAccountNumber", userName);
            return Ok(await _depositService.GetDepositAccountByAccountNumberService(accountNumber));
        }

        [HttpGet("byDepositScheme")]
        public async Task<ActionResult<List<DepositAccountDto>>> GetDepositAccountsByDepositScheme([FromQuery] int depositSchemeId)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("GetDepositAccountsByDepositScheme", userName);
            return Ok(await _depositService.GetAllDepositAccountByDepositSchemeService(depositSchemeId));
        }

        [HttpPost("flexibleInterestRate")]
        public async Task<ActionResult<ResponseDto>> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRateSetupDto flexibleInterestRateSetupDto)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("UpdateInterestRateAccordingToFlexibleInterestRate", userName);
            return Ok(await _depositService.UpdateInterestRateAccordingToFlexibleInterestRateService(flexibleInterestRateSetupDto));
        }

        [HttpPost("changeInterestRate")]
        public async Task<ActionResult<ResponseDto>> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("ChangeInterestRateAccordingToPastInterestRate", userName);
            return Ok(await _depositService.ChangeInterestRateAccordingToPastInterestRateService(changeInterestRateByDepositSchemeDto));
        }

        [HttpPost("incrementDecrementInterestRate")]
        public async Task<ActionResult<ResponseDto>> IncrementOrDecrementOfInterestRateBasedOnDepositScheme(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto)
        {
            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            LogInformation("IncrementOrDecrementOfInterestRateBasedOnDepositScheme", userName);
            return Ok(await _depositService.IncrementOrDecrementOfInterestRateService(updateInterestRateByDepositSchemeDto));
        }


    }
}