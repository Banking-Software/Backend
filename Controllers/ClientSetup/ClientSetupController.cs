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
        private readonly IClientService _clientService;

        public ClientSetupController(ILogger<ClientSetupController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
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

        [HttpPost("createNewClient")]
        public async Task<ActionResult<ResponseDto>> CreateClient(CreateClientDto createClientDto)
        {
            Dictionary<string, string> claims = GetClaims();
            return await _clientService.CreateClientService(createClientDto, claims);
        }

        [HttpPut("updateClient")]
        public async Task<ActionResult<ResponseDto>> UpdateClient(UpdateClientDto updateClientDto)
        {
            Dictionary<string, string> claims = GetClaims();
            return await _clientService.UpdateClientService(updateClientDto, claims);
        }

        [HttpGet("getAllClients")]
        public async Task<ActionResult<List<ClientDto>>> GetAllClients()
        {
            return await _clientService.GetAllClientsService();
        }

        [HttpGet("getClientByClientId")]
        public async Task<ActionResult<ClientDto>> GetClientByClientId([FromQuery] string clientId)
        {
            return await _clientService.GetClientByClientIdService(clientId);
        }

        [HttpGet("getClientByGroup")]
        public async Task<ActionResult<List<ClientDto>>> GetClientByGroup([FromQuery] int groupId)
        {
            return await _clientService.GetClientsByGroupService(groupId);
        }

        [HttpGet("getClientByUnit")]
        public async Task<ActionResult<List<ClientDto>>> GetClientByUnit([FromQuery] int unitId)
        {
            return await _clientService.GetClientsByUnitService(unitId);
        }
        [HttpGet("getClientByGroupAndUnit")]
        public async Task<ActionResult<List<ClientDto>>> GetClientByGroupAndUnit([FromQuery] int groupId, [FromQuery] int unitId)
        {
            return await _clientService.GetClientByGroupAndUnitService(groupId,unitId);
        }

        [HttpGet("getClientByShareType")]
        public async Task<ActionResult<List<ClientDto>>> GetClientByShareType([FromQuery] int shareId)
        {
            return await _clientService.GetClientByAssignedShareTypeService(shareId);
        }

        [HttpGet("getAllClientTypes")]
        public async Task<ActionResult<List<ClientTypeDto>>> GetAllClientTypes()
        {
            return await _clientService.GetClientTypesService();
        }

        [HttpGet("getAllKYMTypes")]
        public async Task<ActionResult<List<ClientKYMTypeDto>>> GetAllClientKYMTypes()
        {
            return await _clientService.GetClientKYMTypesService();
        }

        [HttpGet("getAllShareTypes")]
        public async Task<ActionResult<List<ClientShareTypeDto>>> GetAllShareTypes()
        {
            return await _clientService.GetClientShareTypesService();
        }

        [HttpGet("getAllGroups")]
        public async Task<ActionResult<List<ClientGroupDto>>> GetAllClientGroups()
        {
            return await _clientService.GetClientGroupsService();
        }

        [HttpGet("getAllUnits")]
        public async Task<ActionResult<List<ClientUnitDto>>> GetAllClientUnits()
        {
            return await _clientService.GetClientUnitsService();
        }

    }
}