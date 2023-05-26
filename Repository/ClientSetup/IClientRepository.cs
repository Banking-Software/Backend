using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Repository.ClientSetup
{
    public interface IClientRepository
    {
        Task<int> CreateClientProfile(Client client);
        Task<int> EditClientProfile(Client client);
        Task<List<ClientResponse>> GetClients();
        Task<ClientResponse> GetClient(int id);
        Task<Client> GetClientById(int id); 

        Task<ClientAccountTypeInfo> GetAccountTypeInfo(string type);
        Task<ClientTypeInfo> GetClientTypeInfo(string type);
        Task<ClientShareTypeInfo> GetShareTypeInfo(string type);
        Task<ClientKYMTypeInfo> GetClientKYMTypeInfo(string type);

    }
}