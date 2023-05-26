using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.ErrorManage;
using MicroFinance.Services;
using MicroFinance.Services.DepositSetup;
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

        public DepositSchemeController(ILogger<DepositSchemeController> logger, IDepositSchemeService depositSchemeService)
        {
            _logger = logger;
            _depositSchemeService = depositSchemeService;

        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateDepositScheme(CreateDepositSchemeDto createDepositSchemeDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            return await _depositSchemeService.CreateDepositSchemeService(createDepositSchemeDto, userName);

        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateDepositScheme(UpdateDepositSchemeDto updateDepositSchemeDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            return await _depositSchemeService.UpdateDepositSchemeService(updateDepositSchemeDto, userName);

        }

        [HttpGet]
        public async Task<ActionResult<List<DepositSchemeDto>>> GetAllDepositScheme()
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            return await _depositSchemeService.GetAllDepositSchemeService();

        }

        [HttpGet("id")]
        public async Task<ActionResult<DepositSchemeDto>> GetDepositSchemeById([FromQuery] int id)
        {
            return Ok(await _depositSchemeService.GetDepositSchemeService(id));
        }


    }
}