using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroFinance.Controllers;

namespace MicroFinance.Controllers.UserManagement
{
    [Authorize(AuthenticationSchemes = "UserToken")]
    // [Authorize(Policy = "ActiveUsers")]
    public class FinanceCompanyBaseApiController : BaseApiController
    {
        
    }
}