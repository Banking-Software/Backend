using System.Security.Claims;
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

        public IsActiveAuthorizationFilter(IEmployeeService employeeService, ISuperAdminService superAdminService, ICompanyProfileService companyProfile)
        {   
            _employeeService= employeeService;
            _superAdminService = superAdminService;
            _companyProfile=companyProfile;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string currentUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            string branchCode = context.HttpContext.User.FindFirst("BranchCode")?.Value;
            // if role is superadmin call superadmin service
            if(role!=FintexRole.SuperAdmin.ToString())
            {
                var user = await _employeeService.GetUserByIdService(currentUserId);
                var branch = await _companyProfile.GetBranchServiceByBranchCodeService(branchCode);
                if(user.Message!="Success" || user.IsActive==false || branch==null || branch.IsActive==false)
                {
                    context.Result=new UnauthorizedResult();
                }
            }
            else
            {
                var user = await _superAdminService.GetUserByIdService(currentUserId);
                if(user.Message!="Success" || user.IsActive==false)
                {
                    context.Result=new UnauthorizedResult();
                }
            }

        }
    }
}