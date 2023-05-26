using System.Data;
using AutoMapper;
using MicroFinance.DBContext;
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
        public async Task<int> CreateClientProfile(Client client)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable))
            {
                _logger.LogInformation($"{DateTime.Now} Creating Client...");
                await _dbContext.ClientInfos.AddAsync(client.ClientInfo);
                if (client.ClientContactInfo != null)
                    await _dbContext.ClientContactInfos.AddAsync(client.ClientContactInfo);

                await _dbContext.ClientFamilyInfos.AddAsync(client.ClientFamilyInfo);
                await _dbContext.ClientAddressInfos.AddAsync(client.ClientAddressInfo);

                if (client.ClientNomineeInfo != null)
                    await _dbContext.ClientNomineeInfos.AddAsync(client.ClientNomineeInfo);

                await _dbContext.Clients.AddAsync(client);
                int result = await _dbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    _logger.LogError($"{DateTime.Now} Failed to create Client");
                    await transaction.RollbackAsync();
                    return result;
                }
                _logger.LogInformation($"{DateTime.Now} Client Created");
                await transaction.CommitAsync();
                return result;
            }
        }



        private void UpdatePropertyBag<TEntity>(PropertyValues propertyBag, TEntity updatedEntity)
        {

            foreach (var prop in propertyBag.Properties)
            {
                var updatedValue = updatedEntity.GetType().GetProperty(prop.Name)?.GetValue(updatedEntity);
                var existingValue = propertyBag[prop.Name];
                if (!Equals(existingValue, updatedValue))
                {
                    propertyBag[prop.Name] = updatedValue;
                }
            }
        }
        public async Task<int> EditClientProfile(Client client)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable))
            {
                var existingClient = await _dbContext.Clients.FindAsync(client.Id);

                // START: DATA EXIST MANDATORY
                // Client Info
                var existingClientInfo = await _dbContext.ClientInfos.FindAsync(existingClient.ClientInfoId);
                var propertyBagClientInfo = _dbContext.Entry(existingClientInfo).CurrentValues;
                UpdatePropertyBag(propertyBagClientInfo, client.ClientInfo);
                // Family
                var existingFamilyInfo = await _dbContext.ClientFamilyInfos.FindAsync(existingClient.ClientFamilyInfoId);
                var propertyBagFamilyInfo = _dbContext.Entry(existingFamilyInfo).CurrentValues;
                UpdatePropertyBag(propertyBagFamilyInfo, client.ClientFamilyInfo);
                // Address
                var existingAddressInfo = await _dbContext.ClientAddressInfos.FindAsync(existingClient.ClientAddressInfoId);
                var propertyBagAddressInfo = _dbContext.Entry(existingAddressInfo).CurrentValues;
                UpdatePropertyBag(propertyBagAddressInfo, client.ClientAddressInfo);
                // END

                // START: DATA EXIST OPTIONAL
                // Contact
                if (existingClient.ClientContactInfo != null || client.ClientContactInfo != null)
                {
                    var existingContactInfo = await _dbContext.ClientContactInfos.FindAsync(existingClient.ClientContactInfoId);
                    if (existingContactInfo != null && client.ClientContactInfo != null)
                    {
                        var propertyBagContactInfo = _dbContext.Entry(existingContactInfo).CurrentValues;
                        UpdatePropertyBag(propertyBagContactInfo, client.ClientContactInfo);
                    }
                    else if (existingContactInfo != null && client.ClientContactInfo == null)
                        _dbContext.ClientContactInfos.Remove(existingContactInfo);

                    else if (existingContactInfo == null && client.ClientContactInfo != null)
                    {
                        await _dbContext.ClientContactInfos.AddAsync(client.ClientContactInfo);
                        existingClient.ClientContactInfo = client.ClientContactInfo;
                    }
                }

                // Nominee
                if (existingClient.ClientNomineeInfo != null || client.ClientNomineeInfo != null)
                {
                    var existingNomineeInfo = await _dbContext.ClientNomineeInfos.FindAsync(existingClient.ClientNomineeInfoId);
                    if (existingNomineeInfo != null && client.ClientNomineeInfo != null)
                    {
                        var propertyBagNomineeInfo = _dbContext.Entry(existingNomineeInfo).CurrentValues;
                        UpdatePropertyBag(propertyBagNomineeInfo, client.ClientNomineeInfo);
                    }
                    else if (existingNomineeInfo != null && client.ClientNomineeInfo == null)
                        _dbContext.ClientNomineeInfos.Remove(existingNomineeInfo);
                    else if (existingNomineeInfo == null && client.ClientNomineeInfo != null)
                    {
                        await _dbContext.ClientNomineeInfos.AddAsync(client.ClientNomineeInfo);
                        existingClient.ClientNomineeInfo = client.ClientNomineeInfo;
                    }
                }
                // END
                existingClient.ClientAccountTypeInfoId = client.ClientAccountTypeInfoId;
                existingClient.ClientKYMTypeInfoId = client.ClientKYMTypeInfoId;
                existingClient.ClientShareTypeInfoId = client.ClientShareTypeInfoId;
                existingClient.ClientTypeInfoId = client.ClientTypeInfoId;
                existingClient.RegistrationDate = client.RegistrationDate;
                existingClient.IsKYMUpdated = client.IsKYMUpdated;
                if (client.IsActive == false && existingClient.IsActive == true)
                    existingClient.EndedOn = DateTime.Now;

                existingClient.IsActive = client.IsActive;
                existingClient.IsModified = true;
                existingClient.ModifiedBy = client.ModifiedBy;
                existingClient.ModifiedOn = DateTime.Now;
                existingClient.ModificationCount ??= 0;
                existingClient.ModificationCount++;
                //_dbContext.Entry(existingClient).State = EntityState.Modified;
                int result = await _dbContext.SaveChangesAsync();
                if (result >= 1)
                {
                    await transaction.CommitAsync();
                    return result;
                }
                _logger.LogError($"{DateTime.Now} Failed to Update Clients Information");
                await transaction.RollbackAsync();
                return result;
        }
    }

    public async Task<ClientAccountTypeInfo> GetAccountTypeInfo(string type)
    {
        return await _dbContext.ClientAccountTypeInfos
        .Where(at => at.Type == type)
        .SingleOrDefaultAsync();
    }

    public async Task<ClientResponse> GetClient(int id)
    {
        var client = await _dbContext.Clients
        .Where(c => c.Id == id)
        .Include(c => c.ClientInfo)
        .Include(c => c.ClientFamilyInfo)
        .Include(c => c.ClientContactInfo)
        .Include(c => c.ClientAddressInfo)
        .Include(c => c.ClientNomineeInfo)
        .SingleOrDefaultAsync();
        var accountType = await _dbContext.ClientAccountTypeInfos.FindAsync(client.ClientAccountTypeInfoId);
        var clientType = await _dbContext.ClientTypeInfos.FindAsync(client.ClientTypeInfoId);
        var kymType = await _dbContext.ClientKYMTypeInfos.FindAsync(client.ClientKYMTypeInfoId);
        var shareType = await _dbContext.ClientShareTypeInfos.FindAsync(client.ClientShareTypeInfoId);
        var clientResponse = _mapper.Map<ClientResponse>(client);
        clientResponse.AccountType = accountType.Type;
        clientResponse.ClientType = clientType.Type;
        clientResponse.ShareType = shareType != null ? shareType.Type : null;
        clientResponse.KYMType = kymType != null ? kymType.Type : null;
        return clientResponse;

    }

    public async Task<Client> GetClientById(int id)
    {
        return await _dbContext.Clients.FindAsync(id);
    }

    public async Task<ClientKYMTypeInfo> GetClientKYMTypeInfo(string type)
    {
        return await _dbContext.ClientKYMTypeInfos
        .Where(kym => kym.Type == type)
        .SingleOrDefaultAsync();
    }

    public async Task<List<ClientResponse>> GetClients()
    {

        var clients = await _dbContext.Clients
        .Include(c => c.ClientInfo)
        .Include(c => c.ClientFamilyInfo)
        .Include(c => c.ClientContactInfo)
        .Include(c => c.ClientAddressInfo)
        .Include(c => c.ClientNomineeInfo)
        .ToListAsync();
        var clientResponses = new List<ClientResponse>();
        foreach (var client in clients)
        {
            var clientResponse = _mapper.Map<ClientResponse>(client);
            var accountType = await _dbContext.ClientAccountTypeInfos.FindAsync(client.ClientAccountTypeInfoId);
            var clientType = await _dbContext.ClientTypeInfos.FindAsync(client.ClientTypeInfoId);
            var kymType = await _dbContext.ClientKYMTypeInfos.FindAsync(client.ClientKYMTypeInfoId);
            var shareType = await _dbContext.ClientShareTypeInfos.FindAsync(client.ClientShareTypeInfoId);
            clientResponse.AccountType = accountType.Type;
            clientResponse.ClientType = clientType.Type;
            clientResponse.ShareType = shareType != null ? shareType.Type : null;
            clientResponse.KYMType = kymType != null ? kymType.Type : null;
            clientResponses.Add(clientResponse);
        }
        return clientResponses;

    }

    public async Task<ClientTypeInfo> GetClientTypeInfo(string type)
    {
        return await _dbContext.ClientTypeInfos
        .Where(ct => ct.Type == type)
        .SingleOrDefaultAsync();
    }

    public async Task<ClientShareTypeInfo> GetShareTypeInfo(string type)
    {
        return await _dbContext.ClientShareTypeInfos
        .Where(st => st.Type == type)
        .SingleOrDefaultAsync();
    }
}
}