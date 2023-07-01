using System.Data;
using AutoMapper;
using MicroFinance.DBContext;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Models.AccountSetup;
// using MicroFinance.DBContext.CompanyOperations;
using MicroFinance.Models.ClientSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MicroFinance.Repository.ClientSetup
{


    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ClientRepository> _logger;
        private readonly IMapper _mapper;

        public ClientRepository(ApplicationDbContext dbContext, ILogger<ClientRepository> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }



        public async Task<string> CreateClient(Client client)
        {
            _logger.LogInformation($"{DateTime.Now} Creating Client...");
            var highestClientId = await _dbContext.Clients.OrderByDescending(c=>c.ClientId).FirstOrDefaultAsync();
            client.ClientId = highestClientId==null?"1".PadLeft(5,'0'):(Convert.ToInt32(highestClientId.ClientId)+1).ToString().PadLeft(5,'0');
            await _dbContext.Clients.AddAsync(client);
            int result = await _dbContext.SaveChangesAsync();
            if (result >= 1) return client.ClientId;
            throw new Exception("Unable to Create a Client");
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

        private async Task<Client> CheckConditionAndUpdate(Client existingClient, Client updatingClient)
        {
            if (updatingClient.ClientTypeId != existingClient.ClientTypeId)
                existingClient.ClientType = await _dbContext.ClientTypes.FindAsync(updatingClient.ClientTypeId);
            if (updatingClient.KYMTypeId != null && updatingClient.KYMTypeId != existingClient.KYMTypeId)
                existingClient.KYMType = await _dbContext.ClientKYMTypes.FindAsync(updatingClient.KYMTypeId);

            if (updatingClient.ClientShareTypeInfoId != null && updatingClient.ClientShareTypeInfoId != existingClient.ClientShareTypeInfoId)
                existingClient.ShareType = await _dbContext.Ledgers.FindAsync(updatingClient.ClientShareTypeInfoId);
            else
                existingClient.ShareType = null;

            if (updatingClient.ClientGroupId != null && updatingClient.ClientGroupId != existingClient.ClientGroupId)
                existingClient.ClientGroup = await _dbContext.ClientGroups.FindAsync(updatingClient.ClientGroupId);
            if (updatingClient.ClientUnitId != null && updatingClient.ClientUnitId != existingClient.ClientUnitId)
                existingClient.ClientUnit = await _dbContext.ClientUnits.FindAsync(updatingClient.ClientUnitId);

            return existingClient;
        }


        public async Task<int> UpdateClient(UpdateClientDto updateClientDto, Dictionary<string, string> modifierDetails)
        {
            var existingClient = await _dbContext.Clients.FindAsync(updateClientDto.Id);
            // Client updateClient = _mapper.Map<Client>(updateClientDto);
            var propertyBagOfExistingClient = _dbContext.Entry(existingClient).CurrentValues;
            List<string> excludeProperties = new List<string>()
            {"Id", "ClientId", "ClientTypeId", "ClientType", "KYMType", "KYMTypeId","ShareType", "ClientGroup", "ClientGroupId", "ClientUnit", "ClientUnitId"};

            foreach (var property in propertyBagOfExistingClient.Properties)
            {
                if(!excludeProperties.Contains(property.Name))
                {
                    var newValue = updateClientDto.GetType().GetProperty(property.Name)?.GetValue(updateClientDto);
                    if(newValue!=null && !Equals(propertyBagOfExistingClient[property.Name], newValue))
                    {
                        propertyBagOfExistingClient[property.Name] = newValue;
                    }
                }
            }
            if (existingClient.ClientTypeId != (int)updateClientDto.ClientType)
                existingClient.ClientType = await GetClientTypeById((int)updateClientDto.ClientType);
            if (updateClientDto.KYMType != null && (int)updateClientDto.KYMType != existingClient.KYMTypeId)
                existingClient.KYMType = await GetClientKYMTypeById((int)updateClientDto.KYMType);

            if (updateClientDto.IsShareAllowed && (int)updateClientDto.ShareType != existingClient.ClientShareTypeInfoId)
                existingClient.ShareType = await GetShareTypeById((int)updateClientDto.ShareType);
            else
                existingClient.ShareType = null;
                
            if (updateClientDto.ClientGroupId != null && existingClient.ClientGroupId != updateClientDto.ClientGroupId)
                existingClient.ClientGroup = await GetClientGroupById((int)updateClientDto.ClientGroupId);

            if (updateClientDto.ClientUnitId != null && updateClientDto.ClientUnitId != existingClient.ClientUnitId)
                existingClient.ClientUnit = await GetClientUnitById((int)updateClientDto.ClientUnitId);
            existingClient.IsModified = true;
            existingClient.ModifiedBy = modifierDetails["currentUserName"];
            existingClient.ModifierId = modifierDetails["currentUserId"];
            existingClient.ModifiedOn = DateTime.Now;
            existingClient.ModificationCount ??= 0;
            existingClient.ModificationCount++;
            int result = await _dbContext.SaveChangesAsync();
            return result;
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
            return await _dbContext.Clients.FindAsync(id);
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