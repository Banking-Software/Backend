using System.Data;
using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Models.AccountSetup;
// using MicroFinance.DBContext.CompanyOperations;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MicroFinance.Repository.ClientSetup
{


    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ClientRepository> _logger;
        private readonly IMapper _mapper;

        public ClientRepository
        (
            ApplicationDbContext dbContext, 
            ILogger<ClientRepository> logger, 
            IMapper mapper
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        private async Task<string> AddClientId(Client client)
        {
            try
            {
                var existingclient = await _dbContext.Clients.FindAsync(client.Id);
                client.ClientId = client.Id.ToString().PadLeft(5, '0');
                var statusUpdate = await _dbContext.SaveChangesAsync();
                if(statusUpdate<1) throw new Exception("Client Creation failed");
                _logger.LogInformation($"{DateTime.Now}: {client.ClientId} has been assiged to newly created client");
                return client.ClientId;
            }
            catch (Exception ex)
            {
                _dbContext.Clients.Remove(client);
                _logger.LogError($"{DateTime.Now}: Failed to assign a unique ClientId. Newly Created Client is deleted");
                _logger.LogError($"{DateTime.Now}:{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CreateClient(Client client)
        {
            _logger.LogInformation($"{DateTime.Now} Creating Client...");
            var highestClientId = await _dbContext.Clients.OrderByDescending(c=>c.ClientId).FirstOrDefaultAsync();
            await _dbContext.Clients.AddAsync(client);
            int result = await _dbContext.SaveChangesAsync();
            if(result<1)
                throw new Exception("Unable to Create a Client");
            _logger.LogInformation($"{DateTime.Now}: New Client has been created by employee {client.CreatedBy}. Sending to assign unique clientId...");
            return await AddClientId(client);
        }

        private void UpdatePropertyBag<TEntity>(PropertyValues propertyBag, TEntity updatedEntity)
        {

            foreach (var prop in propertyBag.Properties)
            {
                var updatedValue = updatedEntity.GetType().GetProperty(prop.Name)?.GetValue(updatedEntity);
                var existingValue = propertyBag[prop.Name];
                var keyName = prop.Name;
                if (!Equals(existingValue, updatedValue) && prop.Name != "Id" && prop.Name != "ClientId")
                {
                    propertyBag[prop.Name] = updatedValue;
                }
            }
        }

        public async Task<int> UpdateClient(Client updateClient)
        {
            var existingClient = await _dbContext.Clients.FindAsync(updateClient.Id);
            if(updateClient.IsActive && !existingClient.IsActive)
            {
                _logger.LogInformation($"{DateTime.Now}: Activating inactive client {updateClient.ClientId} by employee {updateClient.ModifiedBy}");
            }
            else if(!updateClient.IsActive && existingClient.IsActive)
            {
                _logger.LogInformation($"{DateTime.Now}: Deactivating active client {updateClient.ClientId} by employee {updateClient.ModifiedBy}");
            }
            _dbContext.Entry(existingClient).State = EntityState.Detached;
            _dbContext.Clients.Attach(updateClient);
            _dbContext.Entry(updateClient).State = EntityState.Modified;
            var status =  await _dbContext.SaveChangesAsync();
            if(status>=1)
            {
                _logger.LogInformation($"{DateTime.Now}: client {updateClient.ClientId} update successfully by employee {updateClient.ModifiedBy}");
            }
            else
            {
                _logger.LogError($"{DateTime.Now}: Failed to update the Client");
            }
            return status;
        }

       

        public async Task<Client> GetClientByClientId(string clientId)
        {
            Client clientByClientId = await _dbContext.Clients
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Where(c => c.ClientId == clientId).FirstOrDefaultAsync();
            return clientByClientId;
        }
        public async Task<Client> GetClientById(int id)
        {
            return await _dbContext.Clients
            .Where(c=>c.Id==id)
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .SingleOrDefaultAsync();
        }

        public async Task<List<Client>> GetAllClients()
        {
            List<Client> clients = await _dbContext.Clients
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .ToListAsync();
            return clients;
        }
        public async Task<List<Client>> GetActiveClientsByBranchCode(string branchCode)
        {
            return await _dbContext.Clients
            .Where(c=>c.IsActive && c.BranchCode==branchCode)
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .ToListAsync();
        }
        public async Task<List<Client>> GetClientsByGroup(int groupId)
        {
            List<Client> clientByGroupId = await _dbContext.Clients
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .Where(c => c.ClientGroupId == groupId).ToListAsync();
            return clientByGroupId;
        }

        public async Task<List<Client>> GetClientByUnit(int unitId)
        {
            List<Client> clientByUnitId = await _dbContext.Clients
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .Where(c => c.ClientUnitId == unitId).ToListAsync();
            return clientByUnitId;
        }

        public async Task<List<Client>> GetClientByGroupAndUnit(int groupId, int unitId)
        {
            List<Client> clientByGroupIdAndUnitId = await _dbContext.Clients
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .Where(c => c.ClientUnitId == unitId && c.ClientGroupId == groupId).ToListAsync();
            return clientByGroupIdAndUnitId;
        }

        public async Task<List<Client>> GetClientByAssignedShareType(int shareTypeId)
        {
            List<Client> clientByShareId = await _dbContext.Clients
            .Include(c => c.ShareType)
            .Include(c => c.ClientType)
            .Include(c => c.ClientGroup)
            .Include(c => c.ClientUnit)
            .Include(c=>c.KYMType)
            .Where(c => c.ClientShareTypeInfoId == shareTypeId).ToListAsync();
            return clientByShareId;
        }

        public async Task<List<ClientType>> GetClientTypes()
        {
            List<ClientType> clientTypes = await _dbContext.ClientTypes.ToListAsync();
            return clientTypes;
        }

        public async Task<ClientType> GetClientTypeById(int id)
        {
            ClientType clientTypeById = await _dbContext.ClientTypes.FindAsync(id);
            return clientTypeById;
        }

        public async Task<List<ClientKYMType>> GetClientKYMTypes()
        {
            List<ClientKYMType> clientKYMTypes = await _dbContext.ClientKYMTypes.ToListAsync();
            return clientKYMTypes;
        }

        public async Task<ClientKYMType> GetClientKYMTypeById(int id)
        {
            ClientKYMType clientKYMTypeById = await _dbContext.ClientKYMTypes.FindAsync(id);
            return clientKYMTypeById;
        }

        public async Task<List<Ledger>> GetShareTypes()
        {
            List<Ledger> shareTypes = await _dbContext.Ledgers.Where(l => l.Id == 16 || l.Id == 17).ToListAsync();
            return shareTypes;
        }

        public async Task<Ledger> GetShareTypeById(int id)
        {
            Ledger shareType = await _dbContext.Ledgers.FindAsync(id);
            return shareType;
        }

        public async Task<List<ClientGroup>> GetClientGroups()
        {
            List<ClientGroup> clientGroups = await _dbContext.ClientGroups.ToListAsync();
            return clientGroups;
        }

        public async Task<ClientGroup> GetClientGroupById(int id)
        {
            ClientGroup clientGroupById = await _dbContext.ClientGroups.FindAsync(id);
            return clientGroupById;
        }

        public async Task<List<ClientUnit>> GetClientUnits()
        {
            List<ClientUnit> clientUnits = await _dbContext.ClientUnits.ToListAsync();
            return clientUnits;
        }

        public async Task<ClientUnit> GetClientUnitById(int id)
        {
            ClientUnit clientUnitById = await _dbContext.ClientUnits.FindAsync(id);
            return clientUnitById;
        }

        
    }
}