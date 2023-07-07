using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.CompanyProfile;
using MicroFinance.Services;
using MicroFinance.Services.CompanyProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.CompanyProfile
{
    public class CompanyProfileController : BaseApiController
    {
        private ICompanyProfileService _companyProfile;

        public CompanyProfileController(ICompanyProfileService companyProfile)
        {
            _companyProfile=companyProfile;
        }

        [Authorize(AuthenticationSchemes = "SuperAdminToken")]
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPost("createBranch")]
        public async Task<ActionResult<ResponseDto>> CreateBranch(CreateBranchDto createBranchDto)
        {
            Dictionary<string, string> claims = GetClaims();
            string createdBy = claims["currentUserName"];
            return await _companyProfile.CreateBranchService(createBranchDto, createdBy);
        }

        [Authorize(AuthenticationSchemes = "SuperAdminToken")]
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpPut("updateBranch")]
        public async Task<ActionResult<ResponseDto>> UpdateBranch(UpdateBranchDto updateBranchDto)
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.UpdateBranchService(updateBranchDto, modifiedBy);
        }
        
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getBranchById")]
        public async Task<ActionResult<BranchDto>> GetBranchById([FromQuery] int id)
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.GetBranchServiceById(id);
        }
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getBranchByBranchCode")]
        public async Task<ActionResult<BranchDto>> GetBranchByBranchCode([FromQuery] string branchCode)
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.GetBranchServiceByBranchCodeService(branchCode);
        }
        [Authorize(AuthenticationSchemes = "UserToken,SuperAdminToken")]
        [TypeFilter(typeof(IsActiveAuthorizationFilter))]
        [HttpGet("getAllBranch")]
        public async Task<ActionResult<List<BranchDto>>> GetAllbranches()
        {
            Dictionary<string, string> claims = GetClaims();
            string modifiedBy = claims["currentUserName"];
            return await _companyProfile.GetAllBranchsService();
        }
        private Dictionary<string,string> GetClaims()
       {
            var currentUserName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            var isUserActive = HttpContext.User.FindFirst("IsActive").Value;
            string branchCode = HttpContext.User.FindFirst("BranchCode").Value;
            string companyName = HttpContext.User.FindFirst("CompanyName").Value;
            string email = HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            Dictionary<string, string> claims = new Dictionary<string, string>
            {
                {"currentUserName",currentUserName},
                {"currentUserId",currentUserId},
                {"role",role},
                {"isUserActive", isUserActive},
                {"branchCode", branchCode},
                {"companyName", companyName},
                {"email", email}
            };
            return claims;
       }
    }
}