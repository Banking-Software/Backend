using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;

namespace MicroFinance.Services.ClientSetup
{
    public interface IClientService
    {
       Task<ResponseDto> CreateClientService(CreateClientDto newClient, TokenDto decodedToken); 
       Task<ResponseDto> UpdateClientService(UpdateClientDto updateClientDto, TokenDto decodedToken);
       Task<ClientDto> GetClientByClientIdService(string clientId, TokenDto decodedToken);
       Task<ClientDto> GetClientByIdService(int id, TokenDto decodedToken);
       Task<List<ClientDto>> GetAllClientsService(TokenDto decodedToken);
       Task<List<ClientDto>> GetActiveClientsByBranchCodeService(string branchCode);
       Task<List<ClientDto>> GetClientsByGroupService(int groupId, TokenDto decodedToken);
       Task<List<ClientDto>> GetClientsByUnitService(int unitId, TokenDto decodedToken);
       Task<List<ClientDto>> GetClientByGroupAndUnitService(int groupId, int unitId, TokenDto decodedToken);
       Task<List<ClientDto>> GetClientByAssignedShareTypeService(int shareTypeId,TokenDto decodedToken);

       Task<List<ClientTypeDto>> GetClientTypesService();
       Task<List<ClientKYMTypeDto>> GetClientKYMTypesService();
       Task<List<ClientShareTypeDto>> GetClientShareTypesService();
       Task<List<ClientGroupDto>> GetClientGroupsService();
       Task<List<ClientUnitDto>> GetClientUnitsService();

       
    }
}