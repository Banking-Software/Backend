using MicroFinance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroFinance.Controllers;

namespace MicroFinance.Controllers.UserManagement
{
    [Authorize(AuthenticationSchemes = "SuperAdminToken")]
    public class SuperAdminBaseApiController : BaseApiController
    {
        
    }
}