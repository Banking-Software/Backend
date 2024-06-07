using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums;
using MicroFinance.Helpers;
using MicroFinance.Services;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers;

[Authorize(AuthenticationSchemes = "UserToken")]
[TypeFilter(typeof(IsActiveAuthorizationFilter))]
public class CommonApiController : BaseApiController
{
    private readonly ITokenService _tokenService;
    private readonly IHelper _helper;

    public CommonApiController(ITokenService tokenService, IHelper helper)
    {
        _tokenService=tokenService;
        _helper=helper;
    }

    [HttpGet("generateMatureDate")]
    public async Task<ActionResult<MatureDateDto>> GenerateMatureDate([FromQuery] string openingDate, [FromQuery] int periodType, [FromQuery] int period)
    {
        GenerateMatureDateDto generateMatureDateDto = new GenerateMatureDateDto()
        {
            OpeningDate=openingDate,
            Period = period,
            PeriodType = (PeriodTypeEnum) periodType
        };
        return Ok(await _helper.GenerateMatureDateOfAccount(generateMatureDateDto));
    }
}