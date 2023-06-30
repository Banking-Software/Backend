using MicroFinance.Dtos.RecordsWithCode;
using MicroFinance.Services;
using MicroFinance.Services.RecordsWithCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.RecordsWithCode
{
    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class RecordsWithCodeController : BaseApiController  
    {
        private readonly IRecordsWithCodeService _recordsWitCodeService;

        public RecordsWithCodeController(IRecordsWithCodeService recordsWitCodeService)
        {
            _recordsWitCodeService = recordsWitCodeService;
        }
        [HttpGet("allRecordsWithCode")]
        public async Task<ActionResult<RecordsWithCodeDto>> GetAllRecordsWithCode()
        {
            return Ok(await _recordsWitCodeService.GetAllRecordWithCodeService());
        }
        [HttpGet("getAllCasts")]
        public async Task<ActionResult<List<CastDto>>> GetAllCasts()
        {
            return Ok(await _recordsWitCodeService.GetAllCastsDetailService());
        }
        [HttpGet("getAllDistricts")]
        public async Task<ActionResult<List<DistrictDto>>> GetAllDistricts()
        {
            return Ok(await _recordsWitCodeService.GetAllDistrictsDetailService());
        }
        [HttpGet("getAllMartialStatus")]
        public async Task<ActionResult<List<MaritalStatusDto>>> GetAllMartialStatus()
        {
            return Ok(await _recordsWitCodeService.GetAllMartialStatusDetailService());
        }
        [HttpGet("getAllGenders")]
        public async Task<ActionResult<List<GenderDto>>> GetAllGenders()
        {
            return Ok(await _recordsWitCodeService.GetAllGendersDetailService());
        }
        [HttpGet("getAllStates")]
        public async Task<ActionResult<List<StateDto>>> GetAllStates()
        {
            return Ok(await _recordsWitCodeService.GetAllStatesDetailsService());
        }

    }
}