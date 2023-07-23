using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.ErrorManage;
using MicroFinance.Services;
using MicroFinance.Services.DepositSetup;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.DepositSetup
{
    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class DepositSchemeController : BaseApiController
    {
        private readonly ILogger<DepositSchemeController> _logger;
        private readonly IDepositSchemeService _depositSchemeService;
        private readonly ITokenService _tokenService;

        public DepositSchemeController
        (
            ILogger<DepositSchemeController> logger,
            IDepositSchemeService depositSchemeService,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _depositSchemeService = depositSchemeService;
            _tokenService = tokenService;

        }

        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }

        [HttpPost("createDepositScheme")]
        public async Task<ActionResult<ResponseDto>> CreateDepositScheme(CreateDepositSchemeDto createDepositSchemeDto)
        {
            var decodedToken = GetDecodedToken();
            return await _depositSchemeService.CreateDepositSchemeService(createDepositSchemeDto, decodedToken);
        }

        [HttpPut("updateDepositScheme")]
        public async Task<ActionResult<ResponseDto>> UpdateDepositScheme(UpdateDepositSchemeDto updateDepositSchemeDto)
        {
            var decodedToken = GetDecodedToken();
            return await _depositSchemeService.UpdateDepositSchemeService(updateDepositSchemeDto, decodedToken);
        }

        [HttpGet("getAllDepositScheme")]
        public async Task<ActionResult<List<DepositSchemeDto>>> GetAllDepositScheme()
        {
           return await _depositSchemeService.GetAllDepositSchemeService();
        }

        [HttpGet("getDepositSchemeById")]
        public async Task<ActionResult<DepositSchemeDto>> GetDepositSchemeById([FromQuery] int id)
        {
            return Ok(await _depositSchemeService.GetDepositSchemeByIdService(id));
        }


    }
}