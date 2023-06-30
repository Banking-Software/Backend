using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Exceptions;
using MicroFinance.Models.AccountSetup;
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

        

        public async Task<ResponseDto> CreateClientService(CreateClientDto newClient, Dictionary<string, string> creatorDetails)
        {
            if
            (
                (newClient.IsShareAllowed && newClient.ShareType!=null)
                ||  
                (!newClient.IsShareAllowed && newClient.ShareType==null)
            )
            {
                Client client = _mapper.Map<Client>(newClient);
                if(newClient.ShareType!=null)
                    client.ShareType = await _clientRepo.GetShareTypeById((int)newClient.ShareType);
                if(newClient.KYMType!=null)
                    client.KYMType = await _clientRepo.GetClientKYMTypeById((int)newClient.KYMType);
                client.ClientType = await _clientRepo.GetClientTypeById((int)newClient.ClientType);
                if(newClient.ClientGroupId!=null)
                    client.ClientGroup = await _clientRepo.GetClientGroupById((int)newClient.ClientGroupId);
                if(newClient.ClientUnitId!=null)
                    client.ClientUnit = await _clientRepo.GetClientUnitById((int)newClient.ClientUnitId);

                client.CreatedBy = creatorDetails["currentUserName"];
                client.CreatorId = creatorDetails["currentUserId"];
                client.CreatorBranchCode = creatorDetails["branchCode"];
                client.CreatedOn = DateTime.Now;
                string clientId = await _clientRepo.CreateClient(client);
                return new ResponseDto()
                {
                    Message=$"Client Id '{clientId}' created successfully.",
                    StatusCode="200",
                    Status=true
                };
            }
            throw new BadRequestExceptionHandler("Share Type Id doesnot match");
        }

        

        public async Task<ResponseDto> UpdateClientService(UpdateClientDto updateClientDto, Dictionary<string, string> modifierDetails)
        {
            Client existingClient = await _clientRepo.GetClientById(updateClientDto.Id);
            if
            (
                existingClient!=null
                && 
                existingClient.ClientId == updateClientDto.ClientId 
                && 
                (
                (updateClientDto.IsShareAllowed && updateClientDto.ShareType!=null)
                ||  
                (!updateClientDto.IsShareAllowed && updateClientDto.ShareType==null)
                )
            )
            {
                int updateStatus = await _clientRepo.UpdateClient(updateClientDto, modifierDetails);
                if(updateStatus>=1) return new ResponseDto(){Message=$"Update successfull for client with Id: {existingClient.ClientId}"};
                throw new Exception("Failed to Update the client's detail");
            }
            throw new BadRequestExceptionHandler("No Data Found for given client");
        }

        public async Task<List<ClientDto>> GetAllClientsService()
        {
            List<Client> allClients = await _clientRepo.GetAllClients();
            if(allClients!=null && allClients.Count>=1)
                return _mapper.Map<List<ClientDto>>(allClients);
            return new List<ClientDto>();
        }

        public async Task<List<ClientDto>> GetClientByAssignedShareTypeService(int shareTypeId)
        {
            if(shareTypeId==16||shareTypeId==17)
            {
                List<Client> clientsByShareType = await _clientRepo.GetClientByAssignedShareType(shareTypeId);
                if(clientsByShareType!=null && clientsByShareType.Count>=1)
                    return _mapper.Map<List<ClientDto>>(clientsByShareType);
                return new List<ClientDto>();
            }
            throw new BadRequestExceptionHandler("Provided id of Share type is not allowed");
        }

        public async Task<ClientDto> GetClientByClientIdService(string clientId)
        {
           Client clientByClientId = await _clientRepo.GetClientByClientId(clientId);
           if(clientByClientId!=null) return _mapper.Map<ClientDto>(clientByClientId);
           throw new NotFoundExceptionHandler("No Data Found");
        }

        public async Task<List<ClientDto>> GetClientByGroupAndUnitService(int groupId, int unitId)
        {
            List<Client> clientsByGroupAndUnit = await _clientRepo.GetClientByGroupAndUnit(groupId, unitId);
            if(clientsByGroupAndUnit!=null && clientsByGroupAndUnit.Count>=1)
                return _mapper.Map<List<ClientDto>>(clientsByGroupAndUnit);
            throw new NotFoundExceptionHandler("No Data Found"); 
        }

        public async Task<List<ClientDto>> GetClientsByGroupService(int groupId)
        {
            List<Client> clientsByGroup = await _clientRepo.GetClientsByGroup(groupId);
            if(clientsByGroup!=null && clientsByGroup.Count>=1)
                return _mapper.Map<List<ClientDto>>(clientsByGroup);
            throw new NotFoundExceptionHandler("No Data Found");
        }

        public async Task<List<ClientDto>> GetClientsByUnitService(int unitId)
        {
            List<Client> clientsByUnit = await _clientRepo.GetClientByUnit(unitId);
            if(clientsByUnit!=null && clientsByUnit.Count>=1)
                return _mapper.Map<List<ClientDto>>(clientsByUnit);
            throw new NotFoundExceptionHandler("No Data Found");
        }

        public async Task<List<ClientTypeDto>> GetClientTypesService()
        {
            List<ClientType> clientTypes = await _clientRepo.GetClientTypes();
            List<ClientTypeDto> clientTypeDtos = new List<ClientTypeDto>();
            foreach (ClientType clientType in clientTypes)
            {
                ClientTypeDto clientTypeDto = new ClientTypeDto()
                {
                    Id = clientType.Id,
                    Type = clientType.Type
                };
                clientTypeDtos.Add(clientTypeDto);
            }
            return clientTypeDtos;
        }

        public async Task<List<ClientKYMTypeDto>> GetClientKYMTypesService()
        {
            List<ClientKYMType> clientKYMTypes = await _clientRepo.GetClientKYMTypes();
            List<ClientKYMTypeDto> clientKYMTypeDtos = new List<ClientKYMTypeDto>();
            foreach (ClientKYMType clientKYMType in clientKYMTypes)
            {
                ClientKYMTypeDto clientKYMTypeDto = new ClientKYMTypeDto()
                {
                    Id = clientKYMType.Id,
                    Type = clientKYMType.Type
                };
                clientKYMTypeDtos.Add(clientKYMTypeDto);
            }
            return clientKYMTypeDtos;
        }

        public async Task<List<ClientShareTypeDto>> GetClientShareTypesService()
        {
            List<Ledger> shareTypes = await _clientRepo.GetShareTypes();
            List<ClientShareTypeDto> clientShareTypeDtos = new List<ClientShareTypeDto>();
            foreach (Ledger shareType in shareTypes)
            {
                ClientShareTypeDto clientShareTypeDto = new ClientShareTypeDto()
                {
                    Id = shareType.Id,
                    Type = shareType.Name
                };
                clientShareTypeDtos.Add(clientShareTypeDto);
            }
            return clientShareTypeDtos;
        }

        public async Task<List<ClientGroupDto>> GetClientGroupsService()
        {
            List<ClientGroup> clientGroups = await _clientRepo.GetClientGroups();
            List<ClientGroupDto> clientGroupDtos = new List<ClientGroupDto>();
            foreach (ClientGroup clientGroup in clientGroups)
            {
                ClientGroupDto clientGroupDto = new ClientGroupDto()
                {
                    Id = clientGroup.Id,
                    Code = clientGroup.Code
                };
                clientGroupDtos.Add(clientGroupDto);
            }
            return clientGroupDtos;
        }

        public async Task<List<ClientUnitDto>> GetClientUnitsService()
        {
            List<ClientUnit> clientUnits = await _clientRepo.GetClientUnits();
            List<ClientUnitDto> clientUnitDtos = new List<ClientUnitDto>();
            foreach (ClientUnit clientUnit in clientUnits)
            {
                ClientUnitDto clientUnitDto = new ClientUnitDto()
                {
                    Id = clientUnit.Id,
                    Code = clientUnit.Code
                };
                clientUnitDtos.Add(clientUnitDto);
            }
            return clientUnitDtos;
        }
    }
}