using System.Security.Claims;
using MicroFinance.Role;
using MicroFinance.Services.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroFinance.Services
{
    public class IsUserOfficerFilter : IAsyncAuthorizationFilter
    {
        private readonly IEmployeeService _employeeService;

        public IsUserOfficerFilter(IEmployeeService employeeService)
        {
            _employeeService= employeeService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            string currentUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if(role==UserRole.Officer.ToString())
            {
                var user = await _employeeService.GetUserByIdService(currentUserId);
                if(
                    user.Message!="Success"
                    || 
                    user.IsActive==false || 
                    await _employeeService.GetRole(currentUserId)!=UserRole.Officer.ToString()
                    )
                {
                    context.Result=new UnauthorizedResult();
                }
            }
            else{
                 context.Result=new UnauthorizedResult();
            }
        }
    }
}