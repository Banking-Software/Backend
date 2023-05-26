using System.Reflection;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Repository.AccountSetup.MainLedger;

namespace MicroFinance.Services.AccountSetup.MainLedger
{
    public class MainLedgerService : IMainLedgerService
    {
        private readonly IMainLedgerRepository _mainLedgerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MainLedgerService> _logger;

        public MainLedgerService(IMainLedgerRepository mainLedgerRepository, IMapper mapper, ILogger<MainLedgerService> logger)
        {
            _mainLedgerRepository = mainLedgerRepository;
            _mapper = mapper;
            _logger=logger;
        }
        // START: ACCOUNT TYPE
        public Task<ResponseDto> CreateAccountTypeService(CreateAccountTypeDto createAccountTypeDto)
        {
            throw new NotImplementedException();
        }
        public async Task<AccountTypeDto> GetAccountTypeByIdService(int id)
        {
            var accountType = await _mainLedgerRepository.GetAccountType(id);
            if (accountType != null)
            {
                return _mapper.Map<AccountTypeDto>(accountType);
            }
            throw new ArgumentNullException($"Content not found");
        }

        public async Task<List<AccountTypeDto>> GetAccountTypesService()
        {
            var accountTypes = await _mainLedgerRepository.GetAccountTypes();
            if (accountTypes != null && accountTypes.Count >= 1)
                return _mapper.Map<List<AccountTypeDto>>(accountTypes);
            else if (accountTypes.Count < 1)
                return new List<AccountTypeDto>();
            throw new ArgumentNullException($"Content not found");

        }

        //END

        // START: GROUP TYPE
        public async Task<ResponseDto> CreateGroupTypeService(CreateGroupTypeDto createGroupTypeDto)
        {
            // Check if account type exist or not
            var accountType = await _mainLedgerRepository.GetAccountType(createGroupTypeDto.AccountTypeId);
            if (accountType != null)
            {
                var groupType = _mapper.Map<GroupType>(createGroupTypeDto);
                groupType.AccountType = accountType;
                groupType.Name = createGroupTypeDto.Name;
                var createStatus = await _mainLedgerRepository.CreateGroupType(groupType);
                if (createStatus >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Group Type Created Succesfully",
                        Status = true,
                        StatusCode = "200"
                    };
                }
                return new ResponseDto()
                {
                    Message = "Failed to Create Group Type",
                    Status = false,
                    StatusCode = "500"
                };
            }
            throw new ArgumentNullException($"Account Type with Id: {createGroupTypeDto.AccountTypeId} does not Exist");

        }

        public async Task<GroupTypeAccounTypeDetailsDto> GetGroupTypeByIdService(int id)
        {
            var groupType = await _mainLedgerRepository.GetGroupTypeById(id);
            if (groupType != null)
            {
                var groupTypeDto = _mapper.Map<GroupTypeAccounTypeDetailsDto>(groupType);
                return groupTypeDto;
            }

            throw new ArgumentNullException($"Content does not Exist");

        }



        public async Task<List<GroupTypeAccounTypeDetailsDto>> GetGroupTypesByAccountService(int accountTypeId)
        {
            var accountType = await _mainLedgerRepository.GetAccountType(accountTypeId);
            if (accountType != null)
            {
                var groupTypes = await _mainLedgerRepository.GetGroupTypesByAccountType(accountTypeId);
                if (groupTypes != null && groupTypes.Count >= 1)
                {
                    var groupTypeDtos = _mapper.Map<List<GroupTypeAccounTypeDetailsDto>>(groupTypes);
                    return groupTypeDtos;
                }
                return new List<GroupTypeAccounTypeDetailsDto>();
            }
            throw new ArgumentNullException($"Account Type with Id: {accountTypeId} does not exist");
        }

        public async Task<List<GroupTypeAccounTypeDetailsDto>> GetGroupTypesService()
        {
            var groupTypes = await _mainLedgerRepository.GetGroupTypes();
            if (groupTypes != null && groupTypes.Count >= 1)
                return _mapper.Map<List<GroupTypeAccounTypeDetailsDto>>(groupTypes);

            else if (groupTypes.Count < 1)
                return new List<GroupTypeAccounTypeDetailsDto>();
            throw new ArgumentNullException($"Content does not Exist");

        }

        // END

        // START: GROUP TYPE DETAILS
        public async Task<ResponseDto> CreateGroupTypeDetailsService(CreateGroupTypeDetailsDto createGroupTypeDetailsDto)
        {
            var groupType = await _mainLedgerRepository.GetGroupTypeById(createGroupTypeDetailsDto.GroupTypeId);
            if (groupType != null)
            {
                var groupTypeDetails = _mapper.Map<GroupTypeDetails>(createGroupTypeDetailsDto);
                groupTypeDetails.GroupType = groupType;
                var createStatus = await _mainLedgerRepository.CreateGroupTypeDetails(groupTypeDetails);
                if (createStatus >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Group Type Details Create Successfully",
                        Status = true,
                        StatusCode = "200"
                    };
                }
                return new ResponseDto()
                {
                    Message = "Failed to Create Group Type Details",
                    Status = false,
                    StatusCode = "500"
                };
            }
            throw new ArgumentNullException($"Group Type with Id: {createGroupTypeDetailsDto.GroupTypeId} does not Exist");
        }
        public async Task<ResponseDto> EditGroupTypeDetailsService(GroupTypeDetailsDto groupTypeDetailsDto)
        {
            var groupTypeDetails = await _mainLedgerRepository.GetGroupTypeDetailsById(groupTypeDetailsDto.Id);
            if (groupTypeDetails != null)
            {
                var updatedGroupTypeDetails = _mapper.Map<GroupTypeDetails>(groupTypeDetailsDto);
                updatedGroupTypeDetails.GroupType=groupTypeDetails.GroupType;
                updatedGroupTypeDetails.GroupTypeId = groupTypeDetails.GroupTypeId;
                // groupTypeDetails.Name = updateGroupTypeDetails.Name;
                // groupTypeDetails.NepaliName = updateGroupTypeDetails.NepaliName;
                // groupTypeDetails.BankBranch = updateGroupTypeDetails.BankBranch;
                // groupTypeDetails.AccountNumber = updateGroupTypeDetails.AccountNumber;
                // groupTypeDetails.BankType = updateGroupTypeDetails.BankType;
                // groupTypeDetails.InterestRate = updateGroupTypeDetails.InterestRate;
                // groupTypeDetails.Branch = updateGroupTypeDetails.Branch;
                var editStatus = await _mainLedgerRepository.EditGroupTypeDetails(updatedGroupTypeDetails);
                if (editStatus >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Edit Succesfull",
                        Status = true,
                        StatusCode = "200"
                    };
                }
                return new ResponseDto()
                {
                    Message = "Edit Failed",
                    Status = false,
                    StatusCode = "500"
                };

            }
            throw new ArgumentNullException("Content does not Exist");
        }
        public async Task<List<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetailsByGroupType(int groupTypeId)
        {
            var groupTypeDetails = await _mainLedgerRepository.GetGroupTypeDetailsByGroupType(groupTypeId);
            if (groupTypeDetails != null && groupTypeDetails.Count >= 1)
                return _mapper.Map<List<GroupTypeDetailsMappingDetailsDto>>(groupTypeDetails);

            else if (groupTypeDetails.Count < 1)
                return new List<GroupTypeDetailsMappingDetailsDto>();

            throw new ArgumentNullException("Content Does not Exist");
        }

        public async Task<List<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetailsByAccountType(int accountTypeId)
        {
            var groupTypeDetails = await _mainLedgerRepository.GetGroupTypeDetailsByAccountType(accountTypeId);
            if (groupTypeDetails != null && groupTypeDetails.Count >= 1)
                return _mapper.Map<List<GroupTypeDetailsMappingDetailsDto>>(groupTypeDetails);

            else if (groupTypeDetails.Count < 1)
                return new List<GroupTypeDetailsMappingDetailsDto>();

            throw new ArgumentNullException("Content Does not Exist");
        }

        public async Task<GroupTypeDetailsMappingDetailsDto> GetGroupTypeDetailsByIdService(int id)
        {
            var groupTypeDetails = await _mainLedgerRepository.GetGroupTypeDetailsById(id);

            if (groupTypeDetails != null)
                return _mapper.Map<GroupTypeDetailsMappingDetailsDto>(groupTypeDetails);

            throw new ArgumentNullException("Content Does not Exist");
        }

        public async Task<List<GroupTypeDetailsMappingDetailsDto>> GetGroupTypeDetailsService()
        {
            var groupTypeDetails = await _mainLedgerRepository.GetGroupTypeDetails();
            if (groupTypeDetails != null)
                return _mapper.Map<List<GroupTypeDetailsMappingDetailsDto>>(groupTypeDetails);
            throw new ArgumentNullException("Content Does not Exist");
        }

        // END

        // START: LEDGER
        public async Task<ResponseDto> CreateLedgerService(CreateLedgerDto createLedgerDto)
        {
            var groupType = await _mainLedgerRepository.GetGroupTypeById(createLedgerDto.GroupTypeId);
            var isExist = await _mainLedgerRepository
            .CheckIfLedgerAndGroupNameExist(createLedgerDto.GroupTypeId, createLedgerDto.Name);
            if (groupType==null || isExist)
            {
                return new ResponseDto()
                {
                    Message = "Information Existed or Group type doesnot exist",
                    Status = false,
                    StatusCode = "400",
                };
            }
            var ledger = _mapper.Map<Ledger>(createLedgerDto);
            var createStatus = await _mainLedgerRepository.CreateLedger(ledger, groupType);
            return new ResponseDto()
            {
                Message = "Ledger Create Successfully",
                Status = true,
                StatusCode = "200",
            };

        }
        public async Task<ResponseDto> EditLedgerService(LedgerDto ledgerDto)
        {
            var ledger = await _mainLedgerRepository.GetLedger(ledgerDto.Id);
            if (ledger != null)
            {
                var editLedger = _mapper.Map<Ledger>(ledgerDto);
                var newLedger = await _mainLedgerRepository.EditLedger(editLedger);
                if (newLedger >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Sucessfully Edited",
                        Status = true,
                        StatusCode = "200"
                    };
                }
                return new ResponseDto()
                {
                    Message = "failed to update the data",
                    Status = false,
                    StatusCode = "500"
                };
            }
            throw new ArgumentNullException($"Ledger with Id {ledgerDto.Id} doesnot exist");
        }

        public async Task<List<LedgerDetailsDto>> GetLedgerByAccountService(int accountTypeId)
        {
            var mappedDetails = await _mainLedgerRepository.GetGroupTypeAndLedgerMapByAccountType(accountTypeId);

            if (mappedDetails != null && mappedDetails.Count >= 1)
            {
                var ledgerDetailsDto = _mapper.Map<List<LedgerDetailsDto>>(mappedDetails);
                return ledgerDetailsDto;
                // foreach (var map in mappedDetails)
                // {
                //     var groupLedgerDto = new GroupLedgerDto()
                //     {
                //         Ledger = _mapper.Map<LedgerDto>(map.Ledger),
                //         GroupType = _mapper.Map<GroupTypeDto>(map.GroupType)
                //     };
                //     groupLedgerDtos.Add(groupLedgerDto);
                // }
                // return groupLedgerDtos;
            }
            else if (mappedDetails.Count < 1) return new List<LedgerDetailsDto>();
            throw new ArgumentNullException("Content Doesnot exist");
        }

        public async Task<List<LedgerDetailsDto>> GetLedgerByGroupService(int groupTypeId)
        {
            var mappedDetails = await _mainLedgerRepository.GetGroupTypeAndLedgerMapByGroupType(groupTypeId);
            if (mappedDetails != null && mappedDetails.Count >= 1)
            {
                var ledgerDetailsDto = _mapper.Map<List<LedgerDetailsDto>>(mappedDetails);
                return ledgerDetailsDto;
                // foreach (var map in mappedDetails)
                // {
                //     var groupLedgerDto = new GroupLedgerDto()
                //     {
                //         Ledger = _mapper.Map<LedgerDto>(map.Ledger),
                //         GroupType = _mapper.Map<GroupTypeDto>(map.GroupType)
                //     };
                //     groupLedgerDtos.Add(groupLedgerDto);
                // }
                // return groupLedgerDtos;
            }
            else if (mappedDetails.Count < 1) return new List<LedgerDetailsDto>();
            throw new ArgumentNullException("Content Doesnot exist");
        }

        public async Task<LedgerDetailsDto> GetLedgerByIdService(int id)
        {
            var mappedDetails = await _mainLedgerRepository.GetLedgerById(id);
            if (mappedDetails != null)
            {
                var ledgerDetailsDto = _mapper.Map<LedgerDetailsDto>(mappedDetails);
                return ledgerDetailsDto;
                // var groupLedgerDto = new GroupLedgerDto()
                // {
                //     Ledger = _mapper.Map<LedgerDto>(mappedDetails.Ledger),
                //     GroupType = _mapper.Map<GroupTypeDto>(mappedDetails.GroupType)
                // };
                // return groupLedgerDto;

            }
            throw new ArgumentNullException("Content Doesnot exist");
        }

        public async Task<List<LedgerDetailsDto>> GetLedgers()
        {
            var mappedDetails = await _mainLedgerRepository.GetLedgers();
            // var groupLedgerDtos = new List<GroupLedgerDto>();
            if (mappedDetails != null && mappedDetails.Count >= 1)
            {
                var ledgerDetailsDto = _mapper.Map<List<LedgerDetailsDto>>(mappedDetails);
                return ledgerDetailsDto;
                // foreach (var map in mappedDetails)
                // {
                //     var groupLedgerDto = new GroupLedgerDto()
                //     {
                //         Ledger = _mapper.Map<LedgerDto>(map.Ledger),
                //         GroupType = _mapper.Map<GroupTypeDto>(map.GroupType)
                //     };
                //     groupLedgerDtos.Add(groupLedgerDto);
                // }
                // return groupLedgerDtos;
            }
            else if (mappedDetails.Count < 1) return new List<LedgerDetailsDto>();
            throw new ArgumentNullException("Content Doesnot exist");
        }
        // END

        // START: SUB-LEDGER
        public async Task<ResponseDto> CreateSubLedgerService(CreateSubLedgerDto createSubLedgerDto)
        {
            var ledger = await _mainLedgerRepository.GetLedger(createSubLedgerDto.LedgerId);
            if (ledger != null && ledger.IsSubLedgerActive)
            {
                var subledger = _mapper.Map<SubLedger>(createSubLedgerDto);
                subledger.Ledger = ledger;
                var createStatus = await _mainLedgerRepository.CreateSubLedger(subledger);
                if (createStatus >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Successfully Created Sub Ledger",
                        Status = true,
                        StatusCode = "200"
                    };
                }
                return new ResponseDto()
                {
                    Message = "failed Create Sub Ledger",
                    Status = false,
                    StatusCode = "500"
                };
            }
            throw new ArgumentNullException($"Ledger Doesnot exist or Subledger for given ledger is not allowed");

        }
        public async Task<ResponseDto> EditSubLedgerService(SubLedgerDto subLedgerDto)
        {
            var subledger = await _mainLedgerRepository.GetSubLedger(subLedgerDto.Id);
            if (subledger != null)
            {
                subledger.Description = subLedgerDto.Description;
                subledger.Name = subLedgerDto.Name;
                var editStatus = await _mainLedgerRepository.EditSubledger(subledger);
                if (editStatus >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Edited Succesfully",
                        Status = true,
                        StatusCode = "200",
                    };
                }
                return new ResponseDto()
                {
                    Message = "Failed to edit",
                    Status = false,
                    StatusCode = "500",
                };
            }
            throw new ArgumentNullException($"SubLedger '{subLedgerDto.Name}' Doesnot exist");
        }

        public async Task<SubLedgerDetailsDto> GetSubLedgerByIdService(int id)
        {
            var subLedger = await _mainLedgerRepository.GetSubLedgerById(id);
            if(subLedger!=null)
            {
                var subLedgerDetailsDto = _mapper.Map<SubLedgerDetailsDto>(subLedger);
                return subLedgerDetailsDto;
                // var ledgerDetails = await _mainLedgerRepository.GetLedgerById(subLedger.LedgerId);
                // if(ledgerDetails!=null)
                // {
                //     var groupSubLedger = new GroupSubLedgerDto()
                //     {
                //         GroupType = _mapper.Map<GroupTypeDto>(ledgerDetails.GroupType),
                //         SubLedger = _mapper.Map<SubLedgerDto>(subLedger)
                //     };
                //     return groupSubLedger;
                // }
            }
            throw new ArgumentNullException("Content Not Found");
        }

        public async Task<List<SubLedgerDetailsDto>> GetSubLedgerByLedgerService(int ledgerId)
        {
            var subLedgerByLedger = await _mainLedgerRepository.GetSubLedgersByLedger(ledgerId);
            if(subLedgerByLedger!=null && subLedgerByLedger.Count>=1)
            {
               var subLedgerDetailsDto = _mapper.Map<List<SubLedgerDetailsDto>>(subLedgerByLedger);
               return subLedgerDetailsDto;
            }
            else if(subLedgerByLedger.Count<1) return new List<SubLedgerDetailsDto>();
            throw new ArgumentNullException("Content Not Found");
        }

        public async Task<List<SubLedgerDetailsDto>> GetSubLedgersService()
        {
            var subLedgers = await _mainLedgerRepository.GetSubLedgers();
            if(subLedgers!=null && subLedgers.Count>=1)
            {
                var subLedgerDetailsDtos = _mapper.Map<List<SubLedgerDetailsDto>>(subLedgers);
                return subLedgerDetailsDtos;
            }
            else if(subLedgers.Count<1) return new List<SubLedgerDetailsDto>();
            throw new ArgumentNullException("Content Not Found");
        }

        // END

        // START: GROUP MAP
        // public Task<GroupLedgerDto> GetAccountGroupLedgerMapsByIdService(int id)
        // {
        //     throw new NotImplementedException();
        // }

        // public Task<List<GroupLedgerDto>> GetAccountGroupLedgerMapsService()
        // {
        //     throw new NotImplementedException();
        // }
        //END
    }
}