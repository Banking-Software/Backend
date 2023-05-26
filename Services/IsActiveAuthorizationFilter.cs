using System.Security.Claims;
using MicroFinance.Role;
using MicroFinance.Services.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroFinance.Services
{
    public class IsActiveAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IEmployeeService _employeeService;
        private ISuperAdminService _superAdminService;

        public IsActiveAuthorizationFilter(IEmployeeService employeeService, ISuperAdminService superAdminService)
        {   
            _employeeService= employeeService;
            _superAdminService = superAdminService;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string currentUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            // if role is superadmin call superadmin service
            if(role!=FintexRole.SuperAdmin.ToString())
            {
                var user = await _employeeService.GetUserByIdService(currentUserId);
                if(user.Message!="Success" || user.IsActive==false)
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