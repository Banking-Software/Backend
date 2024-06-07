using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Services;
using MicroFinance.Services.CompanyProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.CompanyProfile
{
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class CompanyProfileController : BaseApiController
    {
        private ICompanyProfileService _companyProfile;

        public CompanyProfileController(ICompanyProfileService companyProfile)
        {
            _companyProfile=companyProfile;
        }

        [Authorize(AuthenticationSchemes = "SuperAdminToken")]
        [HttpPost("createBranch")]
        public async Task<ActionResult<ResponseDto>> CreateBranch(CreateBranchDto createBranchDto)
        {
            Dictionary<string, string> claims = GetClaims();
            string createdBy = claims["currentUserName"];
            return await _companyProfile.CreateBranchService(createBranchDto, createdBy);
        }

        [Authorize(AuthenticationSchemes = "SuperAdminToken")]
        [HttpPut("updateBranch")]
        public async Task<ActionResult<ResponseDto>> UpdateBranch(UpdateBranchDto updateBranchDto)
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.UpdateBranchService(updateBranchDto, modifiedBy);
        }

        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getBranchById")]
        public async Task<ActionResult<BranchDto>> GetBranchById([FromQuery] int id)
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.GetBranchServiceById(id);
        }

        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getBranchByBranchCode")]
        public async Task<ActionResult<BranchDto>> GetBranchByBranchCode([FromQuery] string branchCode)
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.GetBranchServiceByBranchCodeService(branchCode);
        }
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getAllBranch")]
        public async Task<ActionResult<List<BranchDto>>> GetAllbranches()
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.GetAllBranchsService();
        }

        [HttpPost("createCompanyProfile")]
        [Authorize(AuthenticationSchemes = "SuperAdminToken")]
        public async Task<ActionResult<ResponseDto>> CreateCompanyProfile([FromForm] CreateCompanyProfileDto createCompanyProfileDto)
        {
            return await _companyProfile.CreateCompanyProfileService(createCompanyProfileDto);
        }

        [HttpPut("updateCompanyProfile")]
        [Authorize(AuthenticationSchemes = "SuperAdminToken")]
        public async Task<ActionResult<ResponseDto>> UpdateCompanyProfile([FromForm] UpdateCompanyProfileDto updateCompanyProfileDto)
        {
            return await _companyProfile.UpdateCompanyProfileService(updateCompanyProfileDto);
        }

        [HttpGet("getCompanyProfile")]
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<CompanyProfileDto>> GetCompanyProfile()
        {
            return await _companyProfile.GetCompanyProfileService();
        }

        [HttpPost("createCalendars")]
        [Authorize(AuthenticationSchemes="UserToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<ResponseDto>> CreateCalendars(List<CreateCalenderDto> createCalenderDtos)
        {
            Dictionary<string, string> userClaim = GetClaims();
            return await _companyProfile.CreateCalenderService(createCalenderDtos,userClaim);
        }

        [HttpPut("updateCalendar")]
        [Authorize(AuthenticationSchemes="UserToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<ResponseDto>> UpdateCalendar(UpdateCalenderDto updateCalenderDto)
        {
            Dictionary<string, string> userClaim = GetClaims();
            return await _companyProfile.UpdateCalenderService(updateCalenderDto, userClaim);
        }

        [HttpGet("getAllCalendars")]
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<List<CalendarDto>>> GetAllCalendars()
        {
            Dictionary<string, string> userClaim = GetClaims();
            return await _companyProfile.GetAllCalenderService();
        }

        [HttpGet("getCalendarById")]
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<CalendarDto>> GetCalendarById([FromQuery] int id)
        {
            Dictionary<string, string> userClaim = GetClaims();
            return await _companyProfile.GetCalendarByIdService(id);
        }

        [HttpGet("getCalendarByYear")]
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<List<CalendarDto>>> GetCalendarByYear([FromQuery] int year)
        {
            Dictionary<string, string> userClaim = GetClaims();
            return await _companyProfile.GetCalendarByYearService(year);
        }

        [HttpGet("getActiveCalendar")]
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        // [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        public async Task<ActionResult<CalendarDto>> GetActiveCalendar()
        {
            Dictionary<string, string> userClaim = GetClaims();
            return await _companyProfile.GetCurrentActiveCalenderService();
        }

        private Dictionary<string,string> GetClaims()
       {
            var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var isUserActive = HttpContext.User.FindFirst("IsActive").Value;
            string branchCode = HttpContext.User.FindFirst("BranchCode").Value;
            string email = HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            Dictionary<string, string> claims = new Dictionary<string, string>
            {
                {"currentUserName",currentUserName},
                {"currentUserId",currentUserId},
                {"role",role},
                {"isUserActive", isUserActive},
                {"branchCode", branchCode},
                {"email", email}
            };
            return claims;
       }
    }
}