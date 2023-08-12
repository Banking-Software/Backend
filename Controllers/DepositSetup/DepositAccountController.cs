using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Models.Wrapper;
using MicroFinance.Services;
using MicroFinance.Services.DepositSetup;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.DepositSetup
{

    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class DepositAccountController : BaseApiController
    {
        private readonly IDepositSchemeService _depositService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<DepositAccountController> _logger;

        public DepositAccountController
        (
            IDepositSchemeService depositService, 
            ITokenService tokenService,
            ILogger<DepositAccountController> logger
        )
        {
            _depositService = depositService;
            _tokenService = tokenService;
            _logger=logger;
        }

        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }


        [HttpPost("create")]
        public async Task<ActionResult<ResponseDto>> CreateDepositAccount([FromForm] CreateDepositAccountDto createDepositAccountDto)
        {

            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.CreateDepositAccountService(createDepositAccountDto, decodedToken));

        }

        [HttpPut("update")]
        public async Task<ActionResult<ResponseDto>> UpdateNonClosedDepositAccount([FromForm] UpdateDepositAccountDto updateDepositAccountDto)
        {

            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.UpdateNonClosedDepositAccountService(updateDepositAccountDto, decodedToken));

        }

        [HttpGet("all")]
        public async Task<ActionResult<List<DepositAccountWrapperDto>>> GetAllDepositAccount()
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetAllDepositAccountWrapperService(decodedToken));
        }

        [HttpGet("byId")]
        public async Task<ActionResult<DepositAccountWrapperDto>> GetDepositAccountById([FromQuery] int depositAccountId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetDepositAccountWrapperByIdService(depositAccountId, null,decodedToken));
        }

        [HttpGet("byDepositSchemeId")]
        public async Task<ActionResult<List<DepositAccountWrapperDto>>> GetDepositAccountByDepositScheme([FromQuery] int depositSchemeId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetDepositAccountWrapperByDepositSchemeService(depositSchemeId, decodedToken));
        }

        [HttpGet("byAccountNumber")]
        public async Task<ActionResult<DepositAccountWrapperDto>> GetDepositAccountByAccountNumber([FromQuery] string accountNumber)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetDepositAccountWrapperByAccountNumberService(accountNumber, null,decodedToken));
        }
        [HttpPost("getMatureDate")]
        public async Task<ActionResult<string>> GetMatureDate(GenerateMatureDateDto generateMatureDateDto)
        {
            return Ok(await _depositService.GenerateMatureDateOfDepositAccountService(generateMatureDateDto));
        }
        // [HttpPost("flexibleInterestRate")]
        // public async Task<ActionResult<ResponseDto>> UpdateInterestRateAccordingToFlexibleInterestRate(FlexibleInterestRateSetupDto flexibleInterestRateSetupDto)
        // {
        //     var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
        //     LogInformation("UpdateInterestRateAccordingToFlexibleInterestRate", userName);
        //     return Ok(await _depositService.UpdateInterestRateAccordingToFlexibleInterestRateService(flexibleInterestRateSetupDto));
        // }

        // [HttpPost("changeInterestRate")]
        // public async Task<ActionResult<ResponseDto>> ChangeInterestRateAccordingToPastInterestRate(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto)
        // {
        //     var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
        //     LogInformation("ChangeInterestRateAccordingToPastInterestRate", userName);
        //     return Ok(await _depositService.ChangeInterestRateAccordingToPastInterestRateService(changeInterestRateByDepositSchemeDto));
        // }

        // [HttpPost("incrementDecrementInterestRate")]
        // public async Task<ActionResult<ResponseDto>> IncrementOrDecrementOfInterestRateBasedOnDepositScheme(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto)
        // {
        //     var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
        //     LogInformation("IncrementOrDecrementOfInterestRateBasedOnDepositScheme", userName);
        //     return Ok(await _depositService.IncrementOrDecrementOfInterestRateService(updateInterestRateByDepositSchemeDto));
        // }
    }
}