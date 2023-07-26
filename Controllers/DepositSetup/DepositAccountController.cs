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


        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateDepositAccount(CreateDepositAccountDto createDepositAccountDto)
        {

            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.CreateDepositAccountService(createDepositAccountDto, decodedToken));

        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateNonClosedDepositAccount(UpdateDepositAccountDto updateDepositAccountDto)
        {

            var decodedToken = GetDecodedToken();

            return Ok(await _depositService.UpdateNonClosedDepositAccountService(updateDepositAccountDto, decodedToken));

        }

        [HttpGet("getAllNonClosedDepositAccount")]
        public async Task<ActionResult<List<DepositAccountWrapperDto>>> GetAllNonClosedDepositAccount()
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetAllNonClosedDepositAccountService(decodedToken));
        }

        [HttpGet("getNonClosedDepositAccountById")]
        public async Task<ActionResult<List<DepositAccountWrapperDto>>> GetNonClosedDepositAccountById([FromQuery] int depositAccountId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetNonClosedDepositAccountByIdService(depositAccountId, decodedToken));
        }

        [HttpGet("getNonClosedDepositAccountByDepositSchemeId")]
        public async Task<ActionResult<List<DepositAccountWrapperDto>>> GetAllNonClosedDepositAccountByDepositScheme([FromQuery] int depositSchemeId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetNonClosedDepositAccountByDepositSchemeService(depositSchemeId, decodedToken));
        }

        [HttpGet("getNonClosedDepositAccountByAccountNumber")]
        public async Task<ActionResult<List<DepositAccountWrapperDto>>> GetAllNonClosedDepositAccountByAccountNumber([FromQuery] string accountNumber)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositService.GetNonClosedDepositAccountByAccountNumberService(accountNumber, decodedToken));
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