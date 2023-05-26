using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Repository.ClientSetup;

namespace MicroFinance.Services.ClientSetup
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientService> _logger;

        public ClientService
        (
            IClientRepository clientRepo,
            IMapper mapper,
            ILogger<ClientService> logger
        )
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task<Client> ClientDtoToClientMapAsync(ClientDto clientdto)
        {
            var client = new Client();
            if (clientdto.KYMType != null)
            {
                var KymType = await _clientRepo.GetClientKYMTypeInfo(clientdto.KYMType);
                if (KymType == null) throw new Exception($"{clientdto.KYMType} doesn't exist");
                client.ClientKYMTypeInfoId = KymType.Id;
            }

            if (clientdto.ShareType != null)
            {
                var shareType = await _clientRepo.GetShareTypeInfo(clientdto.ShareType);
                if (shareType == null) throw new Exception($"{clientdto.ShareType} doesn't exist");
                client.ClientShareTypeInfoId = shareType.Id;
            }

            client.ClientInfo = _mapper.Map<ClientInfo>(clientdto.ClientInfo);
            client.ClientAddressInfo = _mapper.Map<ClientAddressInfo>(clientdto.ClientAddress);
            client.ClientFamilyInfo = _mapper.Map<ClientFamilyInfo>(clientdto.ClientFamily);

            if (clientdto.ClientContact != null)
                client.ClientContactInfo = _mapper.Map<ClientContactInfo>(clientdto.ClientContact);

            if (clientdto.ClientNominee != null)
                client.ClientNomineeInfo = _mapper.Map<ClientNomineeInfo>(clientdto.ClientNominee);

            client.RegistrationDate = clientdto.RegistrationDate;
            client.IsKYMUpdated = clientdto.IsKYMUpdated;

            return client;
        }
        public async Task<ResponseDto> CreateClientService(ClientDto client, string createdBy)
        {
            if (
            (client.ClientType != "Minor" && client.ShareType != null)
            || (client.ClientType == "Minor" && client.ShareType == null)
            )
            {
                var accountType = await _clientRepo.GetAccountTypeInfo(client.AccountType);
                var clientType = await _clientRepo.GetClientTypeInfo(client.ClientType);
                if (accountType != null && clientType != null)
                {
                    var newClient = await ClientDtoToClientMapAsync(client);
                    newClient.ClientAccountTypeInfoId = accountType.Id;
                    newClient.ClientTypeInfoId = clientType.Id;
                    newClient.IsActive = true;
                    newClient.CreatedBy = createdBy;
                    newClient.CreateOn = DateTime.Now.Date;
                    await _clientRepo.CreateClientProfile(newClient);
                    return new ResponseDto()
                    {
                        Status = true,
                        StatusCode = "200",
                        Message = "Client Created"
                    };
                }
                return new ResponseDto()
                {
                    Status = false,
                    StatusCode = "200",
                    Message = "Data are missiong in the database for either 'ClientAccountType', or 'ClientClientType'"
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Message = "For Client Type: 'Minor', Share Type has to be 'NULL' otherwise Share Type needs be selected",
                    Status = false,
                    StatusCode = "404"
                };
            }
        }

        public async Task<ResponseDto> EditClientService(ClientDto client, string modifiedBy)
        {

            var clientExist = await _clientRepo.GetClient(client.Id ??= 0);
            if (clientExist != null &&
            ((client.ClientType != "Minor" && client.ShareType != null)
            || (client.ClientType == "Minor" && client.ShareType == null)))
            {
                var accountType = await _clientRepo.GetAccountTypeInfo(client.AccountType);
                var clientType = await _clientRepo.GetClientTypeInfo(client.ClientType);

                if (accountType != null && clientType != null)
                {
                    var newClient = await ClientDtoToClientMapAsync(client);
                    newClient.Id = client.Id ??= 0;
                    // START: Update Type
                    newClient.ClientAccountTypeInfoId = accountType.Id;
                    newClient.ClientTypeInfoId = clientType.Id;
                    newClient.IsActive = client.IsActive ??= true;
                    newClient.ModifiedBy = modifiedBy;

                    await _clientRepo.EditClientProfile(newClient);
                    return new ResponseDto()
                    {
                        Status = true,
                        StatusCode = "200",
                        Message = "Profile Edited Sucessfully"
                    };
                }

                return new ResponseDto()
                {
                    Status = false,
                    StatusCode = "200",
                    Message = "Data are missiong in the database for either 'ClientAccountType', or 'ClientClientType'"
                };
            }
            else
            {
                return new ResponseDto()
                {
                    Message = "Wrong Data",
                    Status = false,
                    StatusCode = "404"
                };
            }

        }


        
        public async Task<ClientDto> GetClientService(int id)
        {
            var client = await _clientRepo.GetClient(id);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<List<ClientDto>> GetClientsService()
        {
            var clients = await _clientRepo.GetClients();
            if (clients != null && clients.Count >= 1)
            {
                return _mapper.Map<List<ClientDto>>(clients);

            }
            throw new Exception("Unable to Fetch the data");
        }
    }
}