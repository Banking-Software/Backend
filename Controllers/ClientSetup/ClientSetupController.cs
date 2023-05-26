using System.Security.Claims;
using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.ErrorManage;
using MicroFinance.Services;
using MicroFinance.Services.ClientSetup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.ClientSetup
{

    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class ClientSetupController : BaseApiController
    {
        private readonly ILogger<ClientSetupController> _logger;
        private readonly IClientService _service;

        public ClientSetupController(ILogger<ClientSetupController> logger, IClientService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> CreateClient(ClientDto clientDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            return await _service.CreateClientService(clientDto, userName);

        }

        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateClient(ClientDto clientDto)
        {

            var userName = HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            return await _service.EditClientService(clientDto, userName);

        }

        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetClients()
        {

            return await _service.GetClientsService();

        }

        [HttpGet("id")]
        public async Task<ActionResult<ClientDto>> GetClient([FromQuery] int id)
        {
            return await _service.GetClientService(id);

        }
    }
}