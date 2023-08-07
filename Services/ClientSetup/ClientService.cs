using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Exceptions;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Repository.ClientSetup;
using MicroFinance.Role;

namespace MicroFinance.Services.ClientSetup
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientService> _logger;
        private readonly IConfiguration _config;

        public ClientService
        (
            IClientRepository clientRepo,
            IMapper mapper,
            ILogger<ClientService> logger,
            IConfiguration config
        )
        {
            _clientRepo = clientRepo;
            _mapper = mapper;
            _logger = logger;
            _config = config;
        }


        private async Task<Client> UploadClientImages(CreateClientDto clientDto, Client client)
        {
            ImageUploadService uploadService = new ImageUploadService(_config);
            List<string> listOfClientPhotoProperty = new List<string>() { nameof(Client.ClientPhotoFileData), nameof(Client.ClientPhotoFileName), nameof(Client.ClientPhotoFileType) };
            List<string> listOfClientCitizenshipPhotoProperty = new List<string>() { nameof(Client.ClientCitizenshipFileData), nameof(Client.ClientCitizenshipFileName), nameof(Client.ClientCitizenshipFileType) };
            List<string> listOfClientSignaturePhotoProperty = new List<string>() { nameof(Client.ClientSignatureFileData), nameof(Client.ClientSignatureFileName), nameof(Client.ClientSignatureFileType) };
            List<string> listOfNomineePhotoProperty = new List<string>() { nameof(Client.NomineePhotoFileData), nameof(Client.NomineePhotoFileName), nameof(Client.NomineePhotoFileType) };
            client = await uploadService.UploadImage(client, clientDto?.ClientPhoto, listOfClientPhotoProperty);
            client = await uploadService.UploadImage(client, clientDto?.ClientCitizenshipPhoto, listOfClientCitizenshipPhotoProperty);
            client = await uploadService.UploadImage(client, clientDto?.ClientSignaturePhoto, listOfClientSignaturePhotoProperty);
            client = await uploadService.UploadImage(client, clientDto?.NomineePhoto, listOfNomineePhotoProperty);
            return client;
        }
        public async Task<ResponseDto> CreateClientService(CreateClientDto newClient, TokenDto decodedToken)
        {
            if
            (
                (newClient.IsShareAllowed && newClient.ShareType != null)
                ||
                (!newClient.IsShareAllowed && newClient.ShareType == null)
            )
            {
                Client client = _mapper.Map<Client>(newClient);
                // client.ShareType = newClient.ShareType != null ? await _clientRepo.GetShareTypeById((int)newClient.ShareType) : null;
                // client.KYMType = newClient.KYMType != null ? await _clientRepo.GetClientKYMTypeById((int)newClient.KYMType) : null;
                // client.ClientType = await _clientRepo.GetClientTypeById((int)newClient.ClientType);
                // client.ClientGroup = newClient.ClientGroupId != null ? await _clientRepo.GetClientGroupById((int)newClient.ClientGroupId) : null;
                // client.ClientUnit = newClient.ClientUnitId != null ? await _clientRepo.GetClientUnitById((int)newClient.ClientUnitId) : null;
                client.CreatedBy = decodedToken.UserName;
                client.CreatorId = decodedToken.UserId;
                client.BranchCode = decodedToken.BranchCode;
                client.CreatedOn = DateTime.Now;
                client = await UploadClientImages(newClient, client);
                string clientId = await _clientRepo.CreateClient(client);
                return new ResponseDto()
                {
                    Message = $"Client Id '{clientId}' created successfully.",
                    StatusCode = "200",
                    Status = true
                };
            }
            throw new BadRequestExceptionHandler("Share Type Id doesnot match");
        }

        public async Task<ResponseDto> UpdateClientService(UpdateClientDto updateClientDto, TokenDto decodedToken)
        {
            Client existingClient = await _clientRepo.GetClientById(updateClientDto.Id);
            if (!existingClient.IsActive && !updateClientDto.IsActive)
            {
                _logger.LogInformation($"{DateTime.Now}: {decodedToken.UserName} tried to make changes on inactive client {updateClientDto.ClientId}");
                throw new UnAuthorizedExceptionHandler("You are not authorized to update on in-active client");
            }
            else if
            (
                existingClient != null
                &&
                existingClient.ClientId == updateClientDto.ClientId
                &&
                (
                (updateClientDto.IsShareAllowed && updateClientDto.ShareType != null)
                ||
                (!updateClientDto.IsShareAllowed && updateClientDto.ShareType == null)
                )
                && (decodedToken.Role == UserRole.Officer.ToString() || existingClient.BranchCode == decodedToken.BranchCode)
            )
            {
                var updateClient = _mapper.Map<Client>(updateClientDto);
                updateClient.Id = existingClient.Id;
                updateClient.ClientId = existingClient.ClientId;
                updateClient = await UpdateClientType(updateClientDto, updateClient, existingClient);
                updateClient = await UpdateClientShareType(updateClientDto, updateClient, existingClient);
                updateClient = await UpdateClientKYMType(updateClientDto, updateClient, existingClient);
                updateClient = await UpdateGroupAndUnit(updateClientDto, updateClient, existingClient);
                updateClient = await UpdateClientImages(updateClientDto, updateClient, existingClient);
                updateClient = await UpdateClientMetaData(updateClient, existingClient, decodedToken);
                int updateStatus = await _clientRepo.UpdateClient(updateClient);
                if (updateStatus >= 1) return new ResponseDto() { Message = $"Update successfull for client with Id: {existingClient.ClientId}", Status=true, StatusCode="200" };
                throw new Exception("Failed to Update the client's detail");
            }
            throw new BadRequestExceptionHandler("No Data Found for given client");
        }

        public async Task<List<ClientDto>> GetAllClientsService(TokenDto decodedToken)
        {
            List<Client> allClients = await _clientRepo.GetAllClients();
            List<ClientDto> clientDtos = new List<ClientDto>();
            if (allClients != null && allClients.Count >= 1)
            {
                foreach (var client in allClients)
                {
                    if (client.BranchCode == decodedToken.BranchCode || decodedToken.Role == UserRole.Officer.ToString())
                    {
                        clientDtos.Add(_mapper.Map<ClientDto>(client));
                    }
                }

            }
            return clientDtos;
        }
        public async Task<List<ClientDto>> GetActiveClientsByBranchCodeService(string branchCode)
        {
            List<Client> activeClientByBranch = await _clientRepo.GetActiveClientsByBranchCode(branchCode);
            if (activeClientByBranch != null && activeClientByBranch.Count >= 1)
                return _mapper.Map<List<ClientDto>>(activeClientByBranch);
            return new List<ClientDto>();
        }

        public async Task<List<ClientDto>> GetClientByAssignedShareTypeService(int shareTypeId, TokenDto decodedToken)
        {
            if (shareTypeId == 16 || shareTypeId == 17)
            {
                List<Client> clientsByShareType = await _clientRepo.GetClientByAssignedShareType(shareTypeId);
                List<ClientDto> clientDtos = new List<ClientDto>();
                if (clientsByShareType != null && clientsByShareType.Count >= 1)
                {
                    foreach (var client in clientsByShareType)
                    {
                        if (client.BranchCode == decodedToken.BranchCode || decodedToken.Role == UserRole.Officer.ToString())
                        {
                            clientDtos.Add(_mapper.Map<ClientDto>(client));
                        }
                    }
                }
                return clientDtos;
            }
            throw new BadRequestExceptionHandler("Provided id of Share type is not allowed");
        }

        public async Task<ClientDto> GetClientByClientIdService(string clientId, TokenDto decodedToken)
        {
            Client clientByClientId = await _clientRepo.GetClientByClientId(clientId);
            if
            (
                clientByClientId != null
                &&
                (clientByClientId.BranchCode == decodedToken.BranchCode || decodedToken.Role == UserRole.Officer.ToString())
            )
            {
                return _mapper.Map<ClientDto>(clientByClientId);
            }

            throw new NotFoundExceptionHandler("No Data Found");
        }

        public async Task<List<ClientDto>> GetClientByGroupAndUnitService(int groupId, int unitId, TokenDto decodedToken)
        {
            List<Client> clientsByGroupAndUnit = await _clientRepo.GetClientByGroupAndUnit(groupId, unitId);
            if (clientsByGroupAndUnit != null && clientsByGroupAndUnit.Count >= 1)
            {
                List<ClientDto> clientDtos = new List<ClientDto>();
                foreach (var client in clientsByGroupAndUnit)
                {
                    if (client.BranchCode == decodedToken.BranchCode || decodedToken.Role == UserRole.Officer.ToString())
                    {
                        clientDtos.Add(_mapper.Map<ClientDto>(client));
                    }
                }
                return clientDtos;
            }
            throw new NotFoundExceptionHandler("No Data Found");
        }

        public async Task<List<ClientDto>> GetClientsByGroupService(int groupId, TokenDto decodedToken)
        {
            List<Client> clientsByGroup = await _clientRepo.GetClientsByGroup(groupId);
            if (clientsByGroup != null && clientsByGroup.Count >= 1)
            {
                List<ClientDto> clientDtos = new List<ClientDto>();
                foreach (var client in clientsByGroup)
                {
                    if (client.BranchCode == decodedToken.BranchCode || decodedToken.Role == UserRole.Officer.ToString())
                    {
                        clientDtos.Add(_mapper.Map<ClientDto>(client));
                    }
                }
                return clientDtos;
            }
            throw new NotFoundExceptionHandler("No Data Found");
        }

        public async Task<List<ClientDto>> GetClientsByUnitService(int unitId, TokenDto decodedToken)
        {
            List<Client> clientsByUnit = await _clientRepo.GetClientByUnit(unitId);
            if (clientsByUnit != null && clientsByUnit.Count >= 1)
            {
                List<ClientDto> clientDtos = new List<ClientDto>();
                foreach (var client in clientsByUnit)
                {
                    if (client.BranchCode == decodedToken.BranchCode || decodedToken.Role == UserRole.Officer.ToString())
                    {
                        clientDtos.Add(_mapper.Map<ClientDto>(client));
                    }
                }
                return clientDtos;
            }
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

        private async Task<Client> UpdateClientType(UpdateClientDto updateClientDto, Client updateClient, Client existingClient)
        {
            if (existingClient.ClientTypeId != (int)updateClientDto.ClientType)
                updateClient.ClientType = await _clientRepo.GetClientTypeById((int)updateClientDto.ClientType);
            else
                updateClient.ClientType = existingClient.ClientType;
            return updateClient;
        }
        private async Task<Client> UpdateClientShareType(UpdateClientDto updateClientDto, Client updateClient, Client existingClient)
        {
            if (updateClientDto.IsShareAllowed && (int)updateClientDto.ShareType != existingClient.ClientShareTypeInfoId)
                updateClient.ShareType = await _clientRepo.GetShareTypeById((int)updateClientDto.ShareType);
            else if (updateClientDto.IsShareAllowed && (int)updateClientDto.ShareType == existingClient.ClientShareTypeInfoId)
                updateClient.ShareType = existingClient.ShareType;
            else
                updateClient.ShareType = null;
            return updateClient;
        }

        private async Task<Client> UpdateClientKYMType(UpdateClientDto updateClientDto, Client updateClient, Client existingClient)
        {
            if (updateClientDto.KYMType != null && (int)updateClientDto.KYMType != existingClient.KYMTypeId)
                updateClient.KYMType = await _clientRepo.GetClientKYMTypeById((int)updateClientDto.KYMType);
            else
                updateClient.KYMType = existingClient?.KYMType;
            return updateClient;
        }
        private async Task<Client> UpdateGroupAndUnit(UpdateClientDto updateClientDto, Client updateClient, Client existingClient)
        {
            if (updateClientDto.ClientGroupId != null && existingClient.ClientGroupId != updateClientDto.ClientGroupId)
                updateClient.ClientGroup = await _clientRepo.GetClientGroupById((int)updateClientDto.ClientGroupId);
            else
                updateClient.ClientGroup = existingClient?.ClientGroup;

            if (updateClientDto.ClientUnitId != null && updateClientDto.ClientUnitId != existingClient.ClientUnitId)
                updateClient.ClientUnit = await _clientRepo.GetClientUnitById((int)updateClientDto.ClientUnitId);
            else
                updateClient.ClientUnit = existingClient?.ClientUnit;
            return updateClient;
        }
        private async Task<Client> UpdateClientImages(UpdateClientDto updateClientDto, Client updateClient, Client existingClient)
        {
            ImageUploadService updateImage = new ImageUploadService(_config);
            if (updateClientDto.IsClientPhotoChanged)
            {
                List<string> listOfClientPhotoProperty = new List<string>() { nameof(Client.ClientPhotoFileData), nameof(Client.ClientPhotoFileName), nameof(Client.ClientPhotoFileType) };
                updateClient = await updateImage.UploadImage(updateClient, updateClientDto?.ClientPhoto, listOfClientPhotoProperty);
            }
            else
            {
                updateClient.ClientPhotoFileData = existingClient?.ClientPhotoFileData;
                updateClient.ClientPhotoFileName = existingClient?.ClientPhotoFileName;
                updateClient.ClientPhotoFileType = existingClient?.ClientPhotoFileType;
            }
            if (updateClientDto.IsClientCitizenshipPhotoChanged)
            {
                List<string> listOfClientCitizenshipPhotoProperty = new List<string>() { nameof(Client.ClientCitizenshipFileData), nameof(Client.ClientCitizenshipFileName), nameof(Client.ClientCitizenshipFileType) };
                updateClient = await updateImage.UploadImage(updateClient, updateClientDto?.ClientCitizenshipPhoto, listOfClientCitizenshipPhotoProperty);
            }
            else
            {
                updateClient.ClientCitizenshipFileData = existingClient?.ClientCitizenshipFileData;
                updateClient.ClientCitizenshipFileName = existingClient?.ClientCitizenshipFileName;
                updateClient.ClientCitizenshipFileType = existingClient?.ClientCitizenshipFileType;
            }
            if (updateClientDto.IsClientSignatureChanged)
            {
                List<string> listOfClientSignaturePhotoProperty = new List<string>() { nameof(Client.ClientSignatureFileData), nameof(Client.ClientSignatureFileName), nameof(Client.ClientSignatureFileType) };
                updateClient = await updateImage.UploadImage(updateClient, updateClientDto?.ClientSignaturePhoto, listOfClientSignaturePhotoProperty);
            }
            else
            {
                updateClient.ClientSignatureFileData = existingClient?.ClientSignatureFileData;
                updateClient.ClientSignatureFileName = existingClient?.ClientSignatureFileName;
                updateClient.ClientSignatureFileType = existingClient?.ClientSignatureFileType;
            }
            if (updateClientDto.IsNomineePhotoChanged)
            {
                List<string> listOfNomineePhotoProperty = new List<string>() { nameof(Client.NomineePhotoFileData), nameof(Client.NomineePhotoFileName), nameof(Client.NomineePhotoFileType) };
                updateClient = await updateImage.UploadImage(updateClient, updateClientDto?.NomineePhoto, listOfNomineePhotoProperty);
            }
            else
            {
                updateClient.NomineePhotoFileData = existingClient?.NomineePhotoFileData;
                updateClient.NomineePhotoFileName = existingClient?.NomineePhotoFileName;
                updateClient.NomineePhotoFileType = existingClient?.NomineePhotoFileType;
            }
            return updateClient;
        }
        private Task<Client> UpdateClientMetaData(Client updateClient, Client existingClient, TokenDto decodedToken)
        {
            updateClient.CreatedBy = existingClient.CreatedBy;
            updateClient.CreatedOn = existingClient.CreatedOn;
            updateClient.BranchCode = existingClient.BranchCode;
            updateClient.CreatorId = existingClient.CreatorId;
            updateClient.IsModified = true;
            updateClient.ModifiedBy = decodedToken.UserName;
            updateClient.ModifierId = decodedToken.UserId;
            updateClient.ModifiedOn = DateTime.Now;
            updateClient.ModificationCount ??= 0;
            updateClient.ModificationCount++;
            return Task.FromResult(updateClient);
        }

        public async Task<ClientDto> GetClientByIdService(int id, TokenDto decodedToken)
        {
            _logger.LogInformation($"{DateTime.Now}: {decodedToken.UserName} requested to fetch client details of Id: {id}");
            var client = await _clientRepo.GetClientById(id);
            if(client!=null)
            {
                _logger.LogInformation($"{DateTime.Now}: {decodedToken.UserName} sending detail of {client.ClientFirstName} {client.ClientLastName} client (Id: {id} to {decodedToken.UserName})");
                return _mapper.Map<ClientDto>(client);
            }
            _logger.LogError($"{DateTime.Now}: (NOTFOUND) {decodedToken.UserName} requested client with Id {id} details and not found in database");
            throw new NotFoundExceptionHandler("No Data Found for provided client id");
        }
    }
}