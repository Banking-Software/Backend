using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;

namespace MicroFinance.Services.ClientSetup
{
    public interface IClientService
    {
       Task<ResponseDto> CreateClientService(ClientDto client, string createdBy); 
       Task<ResponseDto> EditClientService(ClientDto client, string modifiedBy);
       Task<List<ClientDto>> GetClientsService();
       Task<ClientDto> GetClientService(int id);
    }
}