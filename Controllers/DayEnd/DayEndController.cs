using MicroFinance.Dtos;
using MicroFinance.Services.DayEnd;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.DayEnd;

public class DayEndController : BaseApiController
{
    private readonly IDayEndTaskService _dayEndTaskService;

    public DayEndController(IDayEndTaskService dayEndTaskService)
    {
        _dayEndTaskService=dayEndTaskService;
    }

    [HttpGet("maturity")]
    public async  Task<ActionResult<ResponseDto>> CheckMaturityOfAccountAndUpdate()
    {
        return Ok(await _dayEndTaskService.CheckMaturityOfAccountAndUpdateService());
    }
    [HttpGet("calculateDailyInterest")]
    public async  Task<ActionResult<ResponseDto>> CalculateDailyInterestRate()
    {
        return Ok(await _dayEndTaskService.CalculateDailyInterestService());
    }
}