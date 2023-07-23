using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;

namespace MicroFinance.Repository.ClientSetup
{
    public interface IClientRepository
    {
        Task<string> CreateClient(Client client);
        Task<int> UpdateClient(Client updateClient);
        Task<Client> GetClientByClientId(string clientId);
        Task<Client> GetClientById(int id);
        Task<List<Client>> GetAllClients();
        Task<List<Client>> GetClientsByGroup(int groupId);
        Task<List<Client>> GetClientByUnit(int unitId);
        Task<List<Client>> GetClientByGroupAndUnit(int groupId, int unitId);
        Task<List<Client>> GetClientByAssignedShareType(int shareTypeId);
        Task<List<Client>> GetActiveClientsByBranchCode(string branchCode);

        Task<List<ClientType>> GetClientTypes();
        Task<ClientType> GetClientTypeById(int id);
        Task<List<ClientKYMType>> GetClientKYMTypes();
        Task<ClientKYMType> GetClientKYMTypeById(int id);
        Task<List<Ledger>> GetShareTypes();
        Task<Ledger> GetShareTypeById(int id);

        Task<List<ClientGroup>> GetClientGroups();
        Task<ClientGroup> GetClientGroupById(int id);
        Task<List<ClientUnit>> GetClientUnits();
        Task<ClientUnit> GetClientUnitById(int id);


    }
}