using MicroFinance.Dtos;
using MicroFinance.Dtos.LoanSetup;
using MicroFinance.Services;
using MicroFinance.Services.LoanSetup;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.LoanSetup
{
    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class LoanSetupController : BaseApiController
    {
        private readonly ILoanSetupServices _loanSetupServices;
        private readonly ITokenService _tokenService;

        public LoanSetupController(ILoanSetupServices loanSetupServices, ITokenService tokenService)
        {
            _loanSetupServices=loanSetupServices;
            _tokenService=tokenService;
        }
        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }
        [HttpPost("createScheme")]
        public async Task<ActionResult<string>> CreateLoanScheme(CreateLoanSchemeDto createLoanScheme)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _loanSetupServices.CreateLoanSchemeService(createLoanScheme, decodedToken));
        }
        [HttpPost("createAccount")]
        public async Task<ActionResult<string>> CreateLoanAccount([FromForm] CreateLoanAccountDto createLoanAccount)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _loanSetupServices.CreateLoanAccountService(createLoanAccount, decodedToken));
        }
        [HttpGet("loanScheme")]
        public async Task<ActionResult<List<LoanSchemeDto>>> GetLoanScheme([FromQuery] int? loanSchemeId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _loanSetupServices.GetLoanSchemeService(loanSchemeId));
        }
        [HttpGet("loanAccount")]
        public async Task<ActionResult<List<LoanAccountDto>>> GetLoanAccounts([FromQuery] int? loanAccountId)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _loanSetupServices.GetLoanAccountService(loanAccountId));
        }

        [HttpPost("generateLoanSchedule")]
        public async Task<ActionResult<LoanScheduleDtos>> GenerateLoanSchedule(GenerateLoanScheduleDto generateLoanSchedule)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _loanSetupServices.GenerateScheduleService(generateLoanSchedule));
        }
    }
}