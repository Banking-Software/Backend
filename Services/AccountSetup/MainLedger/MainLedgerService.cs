using System.Reflection;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Repository.AccountSetup.MainLedger;
using MicroFinance.Repository.CompanyProfile;

namespace MicroFinance.Services.AccountSetup.MainLedger
{
    public class MainLedgerService : IMainLedgerService
    {
        private readonly IMainLedgerRepository _mainLedgerRepository;
        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MainLedgerService> _logger;

        public MainLedgerService(
            IMainLedgerRepository mainLedgerRepository,
            ICompanyProfileRepository companyProfileRepository,
            IMapper mapper, ILogger<MainLedgerService> logger)
        {
            _mainLedgerRepository = mainLedgerRepository;
            _companyProfileRepository = companyProfileRepository;
            _mapper = mapper;
            _logger = logger;
        }
        // START: ACCOUNT TYPE

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
            throw new ArgumentNullException("Account Type");

        }

        //END

        // START: GROUP TYPE
        public async Task<ResponseDto> CreateGroupTypeService(CreateGroupTypeDto createGroupTypeDto)
        {
            // Check if account type exist or not
            var accountType = await _mainLedgerRepository.GetAccountType(createGroupTypeDto.AccountTypeId);
            if (accountType != null && !(await _mainLedgerRepository.CheckIfGroupNameExist(createGroupTypeDto.AccountTypeId, createGroupTypeDto.Name)))
            {
                var groupType = _mapper.Map<GroupType>(createGroupTypeDto);
                // groupType.AccountType = accountType;
                //groupType.DebitOrCredit = await _mainLedgerRepository.GetDebitOrCreditById(createGroupTypeDto.DebitOrCreditId);
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

        public async Task<ResponseDto> UpdateGroupTypeService(UpdateGroupTypeDto updateGroupTypeDto)
        {
            var updateStatus = await _mainLedgerRepository.UpdateGroupType(updateGroupTypeDto);
            if (updateStatus >= 1) return new ResponseDto() { Message = "Update Successfull", Status = true, StatusCode = "200" };
            throw new Exception("Update Failed");
        }

        public async Task<GroupTypeDto> GetGroupTypeByIdService(int id)
        {
            var groupType = await _mainLedgerRepository.GetGroupTypeById(id);
            if (groupType != null)
            {
                var groupTypeDto = _mapper.Map<GroupTypeDto>(groupType);
                return groupTypeDto;
            }

            throw new ArgumentNullException($"Group Type with id {id}");

        }



        public async Task<List<GroupTypeDto>> GetGroupTypesByAccountService(int accountTypeId)
        {
            //var accountType = await _mainLedgerRepository.GetAccountType(accountTypeId);
            //if (accountType != null)
            //{
            var groupTypes = await _mainLedgerRepository.GetGroupTypesByAccountType(accountTypeId);
            if (groupTypes != null && groupTypes.Count >= 1)
            {
                var groupTypeDtos = _mapper.Map<List<GroupTypeDto>>(groupTypes);
                return groupTypeDtos;
            }
            return new List<GroupTypeDto>();
            //}
            throw new ArgumentNullException($"Account Type with Id: {accountTypeId}");
        }

        public async Task<List<GroupTypeDto>> GetGroupTypesService()
        {
            var groupTypes = await _mainLedgerRepository.GetGroupTypes();
            if (groupTypes != null && groupTypes.Count >= 1)
                return _mapper.Map<List<GroupTypeDto>>(groupTypes);
            else if (groupTypes.Count < 1)
                return new List<GroupTypeDto>();
            throw new ArgumentNullException($"Group Type");
        }

        // END



        // START: LEDGER
        /// <summary>
        /// Create a new Ledger
        /// </summary>
        /// <param name="createLedgerDto"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CreateLedgerService(CreateLedgerDto createLedgerDto)
        {
            var groupType = await _mainLedgerRepository.GetGroupTypeById(createLedgerDto.GroupTypeId);
            if (groupType == null || (await _mainLedgerRepository.CheckIfLedgerNameExist(createLedgerDto.GroupTypeId, createLedgerDto.Name)))
                throw new ArgumentNullException("No Group found for the ledger or Ledger already exist");

            var ledger = _mapper.Map<Ledger>(createLedgerDto);
            // ledger.GroupType = groupType;
            ledger.IsBank = false;
            var createStatus = await _mainLedgerRepository.CreateLedger(ledger);
            if (createStatus >= 1)
                return new ResponseDto()
                {
                    Message = "Ledger Create Successfully",
                    Status = true,
                    StatusCode = "200",
                };
            throw new BadHttpRequestException("Failed to Create Ledger");

        }

        public async Task<ResponseDto> EditLedgerService(UpdateLedgerDto ledgerDto)
        {
            // var ledger = await _mainLedgerRepository.GetLedger(ledgerDto.Id);
            // if (ledger != null)
            // {
            //     var editLedger = _mapper.Map<Ledger>(ledgerDto);
            var newLedger = await _mainLedgerRepository.EditLedger(ledgerDto);
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
            //}
            throw new ArgumentNullException($"Ledger with Id {ledgerDto.Id} doesnot exist");
        }

        public async Task<List<LedgerDto>> GetLedgerByAccountService(int accountTypeId)
        {
            var ledgers = await _mainLedgerRepository.GetLedgersByAccountType(accountTypeId);

            if (ledgers != null && ledgers.Count >= 1)
            {
                var ledgerDetailsDto = _mapper.Map<List<LedgerDto>>(ledgers);
                return ledgerDetailsDto;
            }
            else if (ledgers.Count < 1) return new List<LedgerDto>();
            throw new ArgumentNullException("Content Doesnot exist");
        }

        public async Task<List<LedgerDto>> GetLedgerByGroupService(int groupTypeId)
        {
            var ledgers = await _mainLedgerRepository.GetLedgerByGroupType(groupTypeId);
            if (ledgers != null && ledgers.Count >= 1)
            {
                var ledgerDetailsDto = _mapper.Map<List<LedgerDto>>(ledgers);
                return ledgerDetailsDto;
            }
            else if (ledgers.Count < 1) return new List<LedgerDto>();
            throw new ArgumentNullException("Content Doesnot exist");
        }

        public async Task<LedgerDto> GetLedgerByIdService(int id)
        {
            var ledger = await _mainLedgerRepository.GetLedger(id);
            if (ledger != null)
            {
                var ledgerDetailsDto = _mapper.Map<LedgerDto>(ledger);
                return ledgerDetailsDto;
            }
            throw new ArgumentNullException("Content Doesnot exist");
        }

        public async Task<List<LedgerDto>> GetLedgers()
        {
            var ledgers = await _mainLedgerRepository.GetLedgers();
            if (ledgers != null && ledgers.Count >= 1)
            {
                var ledgerDetailsDto = _mapper.Map<List<LedgerDto>>(ledgers);
                return ledgerDetailsDto;
            }
            else if (ledgers.Count < 1) return new List<LedgerDto>();
            throw new ArgumentNullException("Content Doesnot exist");
        }
        // END


        // START: BANK SETUP
        public async Task<ResponseDto> CreateBankSetupService(CreateBankSetupDto createBankSetupDto)
        {
            var bankType = await _mainLedgerRepository.GetBankTypeById(createBankSetupDto.BankTypeId);
            var branchCode = await _companyProfileRepository.GetBranchByBranchCode(createBankSetupDto.BranchCode);
            if (bankType == null || branchCode == null)
                throw new Exception("Please check branch code or bank type and try again");

            var bankSetup = _mapper.Map<BankSetup>(createBankSetupDto);
            // bankSetup.BankType = bankType;
            var createStatus = await _mainLedgerRepository.CreateBankSetup(bankSetup);
            if (createStatus >= 1)
            {
                return new ResponseDto()
                {
                    Message = "Bank Create Successfully",
                    Status = true,
                    StatusCode = "200"
                };
            }
            return new ResponseDto()
            {
                Message = "Failed to Create Bank",
                Status = false,
                StatusCode = "500"
            };
        }
        public async Task<ResponseDto> EditBankSetupService(UpdateBankSetup bankSetupDto)
        {
            var editStatus = await _mainLedgerRepository.EditBankSetup(bankSetupDto);
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

            //}
            throw new ArgumentNullException("Content does not Exist");
        }
        public async Task<List<BankSetupDto>> GetBankSetupByLedgerService(int ledgerId)
        {
            var bankSetup = await _mainLedgerRepository.GetBankSetupByLedger(ledgerId);
            if (bankSetup != null && bankSetup.Count >= 1)
                return _mapper.Map<List<BankSetupDto>>(bankSetup);

            else if (bankSetup.Count < 1)
                return new List<BankSetupDto>();

            throw new ArgumentNullException("Content Does not Exist");
        }

        public async Task<BankSetupDto> GetBankSetupByIdService(int id)
        {
            var bankSetup = await _mainLedgerRepository.GetBankSetupById(id);
            if (bankSetup != null)
                return _mapper.Map<BankSetupDto>(bankSetup);

            throw new ArgumentNullException("Bank Detail");
        }

        public async Task<List<BankSetupDto>> GetBankSetupService()
        {
            var bankSetup = await _mainLedgerRepository.GetBankSetup();
            if (bankSetup != null)
                return _mapper.Map<List<BankSetupDto>>(bankSetup);
            throw new ArgumentNullException("Content Does not Exist");
        }

        public async Task<List<BankTypeDto>> GetAllBankTypeService()
        {
            var bankTypes = await _mainLedgerRepository.GetAllBankType();
            if (bankTypes == null || bankTypes.Count <= 0) throw new Exception("No Data Found");
            return _mapper.Map<List<BankTypeDto>>(bankTypes);
        }
        public async Task<BankTypeDto> GetBankTypeByIdService(int id)
        {
            var bankType = await _mainLedgerRepository.GetBankTypeById(id);
            if (bankType == null) throw new BadHttpRequestException("Bad Request: ID");
            return _mapper.Map<BankTypeDto>(bankType);
        }

        // END

        // START: SUB-LEDGER
        public async Task<ResponseDto> CreateSubLedgerService(CreateSubLedgerDto createSubLedgerDto)
        {
            var ledger = await _mainLedgerRepository.GetLedger(createSubLedgerDto.LedgerId);
            if (ledger != null && ledger.IsSubLedgerActive)
            {
                var subledger = _mapper.Map<SubLedger>(createSubLedgerDto);
                await _mainLedgerRepository.CreateSubLedger(subledger);
                return new ResponseDto()
                {
                    Message = "Successfully Created Sub Ledger",
                    Status = true,
                    StatusCode = "200"
                };

            }
            throw new Exception("Not Allowed to create Subledger for give ledger");
        }
        public async Task<ResponseDto> EditSubLedgerService(UpdateSubLedgerDto subLedgerDto)
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

        public async Task<SubLedgerDto> GetSubLedgerByIdService(int id)
        {
            var subLedger = await _mainLedgerRepository.GetSubLedgerById(id);
            if (subLedger != null)
            {
                var subLedgerDetailsDto = _mapper.Map<SubLedgerDto>(subLedger);
                return subLedgerDetailsDto;
            }
            throw new ArgumentNullException("Requested SubLedger Not Found");
        }

        public async Task<List<SubLedgerDto>> GetSubLedgerByLedgerService(int ledgerId)
        {
            var subLedgerByLedger = await _mainLedgerRepository.GetSubLedgersByLedger(ledgerId);
            if (subLedgerByLedger != null && subLedgerByLedger.Count >= 1)
            {
                var subLedgerDetailsDto = _mapper.Map<List<SubLedgerDto>>(subLedgerByLedger);
                return subLedgerDetailsDto;
            }
            else if (subLedgerByLedger.Count < 1) return new List<SubLedgerDto>();
            throw new ArgumentNullException("Content Not Found");
        }

        public async Task<List<SubLedgerDto>> GetSubLedgersService()
        {
            var subLedgers = await _mainLedgerRepository.GetSubLedgers();
            if (subLedgers != null && subLedgers.Count >= 1)
            {
                var subLedgerDetailsDtos = _mapper.Map<List<SubLedgerDto>>(subLedgers);
                return subLedgerDetailsDtos;
            }
            else if (subLedgers.Count < 1) return new List<SubLedgerDto>();
            throw new ArgumentNullException("Content Not Found");
        }
    }
}