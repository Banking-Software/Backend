using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;

namespace MicroFinance.Services.ClientSetup
{
    public interface IClientService
    {
       Task<ResponseDto> CreateClientService(CreateClientDto newClient, Dictionary<string, string> creatorDetails); 
       Task<ResponseDto> UpdateClientService(UpdateClientDto updateClientDto, Dictionary<string, string> modifierDetails);
       Task<ClientDto> GetClientByClientIdService(string clientId);
       Task<List<ClientDto>> GetAllClientsService();
       Task<List<ClientDto>> GetClientsByGroupService(int groupId);
       Task<List<ClientDto>> GetClientsByUnitService(int unitId);
       Task<List<ClientDto>> GetClientByGroupAndUnitService(int groupId, int unitId);
       Task<List<ClientDto>> GetClientByAssignedShareTypeService(int shareTypeId);

       Task<List<ClientTypeDto>> GetClientTypesService();
       Task<List<ClientKYMTypeDto>> GetClientKYMTypesService();
       Task<List<ClientShareTypeDto>> GetClientShareTypesService();
       Task<List<ClientGroupDto>> GetClientGroupsService();
       Task<List<ClientUnitDto>> GetClientUnitsService();

       
    }
}