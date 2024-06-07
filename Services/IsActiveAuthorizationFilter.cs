using System.Security.Claims;
using MicroFinance.Enums;
using MicroFinance.Role;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroFinance.Services
{
    public class IsActiveAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IEmployeeService _employeeService;
        private ISuperAdminService _superAdminService;
        private readonly ICompanyProfileService _companyProfile;
        private readonly ILogger<IsActiveAuthorizationFilter> _logger;

        public IsActiveAuthorizationFilter(
            IEmployeeService employeeService,
            ISuperAdminService superAdminService,
            ICompanyProfileService companyProfile,
            ILogger<IsActiveAuthorizationFilter> logger
            )
        {
            _employeeService = employeeService;
            _superAdminService = superAdminService;
            _companyProfile = companyProfile;
            _logger = logger;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            string currentUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != RoleEnum.SuperAdmin.ToString())
            {
                string branchCode = context.HttpContext.User.FindFirst("BranchCode")?.Value;
                var companyDetail = await _companyProfile.GetCompanyProfileService();
                var branch = await _companyProfile.GetBranchServiceByBranchCodeService(branchCode);
                var user = await _employeeService.GetUserByIdService(currentUserId);
                if (companyDetail.CompanyValidityEndDate < DateTime.Now)
                {
                    string errorMessage = $"Software Validity ended on {companyDetail.CompanyValidityEndDate}. Please contact software provider";
                    _logger.LogError($"{DateTime.Now}: Tried to Use software even after validity ended on {companyDetail.CompanyValidityEndDate}");
                    context.Result = new ObjectResult(errorMessage)
                    {
                        StatusCode = 401
                    };
                }
                else if (!branch.IsActive || user.IsActive==false)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                var user = await _superAdminService.GetUserByIdService(currentUserId);
                if (user.Message != "Success" || user.IsActive == false)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}