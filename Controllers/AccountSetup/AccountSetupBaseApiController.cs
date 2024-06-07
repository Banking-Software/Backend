using Microsoft.AspNetCore.Authorization;
using MicroFinance.Controllers;
using Microsoft.AspNetCore.Mvc;
using MicroFinance.Services;

namespace MicroFinance.Controllers.AccountSetup
{
    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class AccountSetupBaseApiController : BaseApiController
    {
        
    }
}