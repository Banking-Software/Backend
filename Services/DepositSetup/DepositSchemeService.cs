using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Enums;
using MicroFinance.Exceptions;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Repository.AccountSetup.MainLedger;
using MicroFinance.Repository.ClientSetup;
using MicroFinance.Repository.DepositSetup;

namespace MicroFinance.Services.DepositSetup
{
    public class DepositSchemeService : IDepositSchemeService
    {
        private readonly ILogger<DepositSchemeDto> _loggger;
        private readonly IMapper _mapper;
        private readonly IDepositSchemeRepository _depositSchemeRepository;
        private readonly IMainLedgerRepository _mainLedgerRepository;
        private readonly IClientRepository _clientRepo;

        public DepositSchemeService
        (
        ILogger<DepositSchemeDto> logger,
        IMapper mapper,
        IDepositSchemeRepository depositSchemeRepository,
        IMainLedgerRepository mainLedgerRepository,
        IClientRepository clientRepository
        )
        {
            _loggger = logger;
            _mapper = mapper;
            _depositSchemeRepository = depositSchemeRepository;
            _mainLedgerRepository = mainLedgerRepository;
            _clientRepo = clientRepository;
        }

        /// <summary>
        /// Create Deposit Scheme Service.
        /// Validation: 
        /// 1. Minimum Interest Rate Should be less than or Equal to Maximum Interest Rate. 
        /// 2. If LiabilityAccount Name and InterestAccount Name is not provided then with some naming convention create it
        /// 3. LiabilityAccount is Ledger under Account Type 'Liability'  and GroupType 'Saving and Deposit'
        /// 4. InterestAccount is also Ledger under Account Type 'Interest' and GroupType 'Interest Expenses'
        /// 5. If the Ledger exist use that otherwise first create the ledger and then do other task
        /// </summary>
        /// <param name="createDepositScheme"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CreateDepositSchemeService(CreateDepositSchemeDto createDepositScheme, string currentUser)
        {


            var depositLedgerAsLiability = createDepositScheme.LiabilityAccount ?? createDepositScheme.Name + " " + "Liability";
            var depositLedgerAsInterest = createDepositScheme.InterestAccount ?? createDepositScheme.Name + " " + "Interest";

            var depositExist = await _depositSchemeRepository
            .GetDepositSchemeByName(createDepositScheme.Name);

            if (depositExist == null && createDepositScheme.MinimumInterestRate <= createDepositScheme.InterestRate && createDepositScheme.InterestRate <= createDepositScheme.MaximumInterestRate)
            {
                var groupTypeAsLiability = await _mainLedgerRepository.GetGroupByName("Saving and Deposit", "Liability");
                var groupTypeAsInterest = await _mainLedgerRepository.GetGroupByName("Interest Expenses", "Expense");

                Ledger ledgerAsLiability = new Ledger();
                Ledger ledgerAsInterest = new Ledger();

                ledgerAsLiability = await _mainLedgerRepository
                .GetLedgerByGroupTypeAndLedgerName(groupTypeAsLiability, depositLedgerAsLiability);

                ledgerAsInterest = await _mainLedgerRepository
                .GetLedgerByGroupTypeAndLedgerName(groupTypeAsInterest, depositLedgerAsInterest);

                var ledgerAsLiabilityStatus = 1;
                var ledgerAsInterestStatus = 1;

                if (ledgerAsLiability == null || ledgerAsLiability.Id <= 0)
                {
                    ledgerAsLiability.Name = depositLedgerAsLiability;
                    ledgerAsLiability.EntryDate = DateTime.Now;
                    ledgerAsLiability.IsSubLedgerActive = false;
                    ledgerAsLiabilityStatus =
                    await _mainLedgerRepository.CreateLedger(ledgerAsLiability, groupTypeAsLiability);
                }
                if (ledgerAsInterest == null || ledgerAsInterest.Id <= 0)
                {
                    ledgerAsInterest.Name = depositLedgerAsInterest;
                    ledgerAsInterest.EntryDate = DateTime.Now;
                    ledgerAsInterest.IsSubLedgerActive = false;
                    ledgerAsInterestStatus =
                    await _mainLedgerRepository.CreateLedger(ledgerAsInterest, groupTypeAsInterest);
                }
                if (ledgerAsInterestStatus >= 1 && ledgerAsInterestStatus >= 1)
                {
                    var depositScheme = _mapper.Map<DepositScheme>(createDepositScheme);

                    int postingId = (int)createDepositScheme.Posting;
                    var positng = await _depositSchemeRepository
                    .GetPositingScheme(postingId);
                    depositScheme.PostingScheme = positng;

                    depositScheme.LedgerAsInterestAccount = ledgerAsInterest;
                    depositScheme.LedgerAsLiabilityAccount = ledgerAsLiability;
                    depositScheme.DepositType = createDepositScheme.DepositType.ToString();
                    depositScheme.CreatedBy = currentUser;
                    depositScheme.CreatedOn = DateTime.Now;

                    int depositCreationStatus =
                    await _depositSchemeRepository.CreateDepositScheme(depositScheme);

                    if (depositCreationStatus < 1)
                        throw new NotImplementedException("Unable to create Deposit. Try again!!!");
                }
                else
                    throw new NotImplementedException("Unable to Create Ledger. Try again!!!");

                return new ResponseDto
                {
                    Status = true,
                    StatusCode = "200",
                    Message = "Deposit Scheme Created"
                };

            }

            else if (createDepositScheme.MinimumInterestRate > createDepositScheme.InterestRate || createDepositScheme.InterestRate > createDepositScheme.MaximumInterestRate)
                throw new Exception("'Interest Rate' should be between 'minimum interest rate' and 'maximum interest rate'. Follow minimumInterestRate<=interestRate<=maximumInterestRate");
            return new ResponseDto
            {
                Status = false,
                StatusCode = "500",
                Message = "Deposit Scheme Already Existed"
            };
        }

        public async Task<ResponseDto> UpdateDepositSchemeService(UpdateDepositSchemeDto updateDepositScheme, string currentUser)
        {
            UpdateDepositScheme depositScheme = _mapper.Map<UpdateDepositScheme>(updateDepositScheme);
            depositScheme.ModifiedBy = currentUser;
            depositScheme.ModifiedOn = DateTime.Now;
            int modifiedStatus = await _depositSchemeRepository.UpdateDepositScheme(depositScheme);
            if (modifiedStatus >= 1)
            {
                return new ResponseDto
                {
                    Status = true,
                    StatusCode = "200",
                    Message = "Deposit Scheme Updated"
                };
            }
            throw new Exception("Failed to update");

        }

        private DepositSchemeDto GetDepositSchemeDto(ResponseDepositScheme scheme)
        {
            DepositSchemeDto tempDepositSchemeDto = new DepositSchemeDto()
            {
                Id = scheme.Id,
                Name = scheme.Name,
                NameNepali = scheme.NameNepali,
                DepositType = scheme.DepositType,
                Symbol = scheme.Symbol,
                MinimumBalance = scheme.MinimumBalance,
                InterestRateOnMinimumBalance = scheme.InterestRateOnMinimumBalance,
                InterestRate = scheme.InterestRate,
                MinimumInterestRate = scheme.MinimumInterestRate,
                MaximumInterestRate = scheme.MaximumInterestRate,
                Calculation = scheme.Calculation,
                FineAmount = scheme.FineAmount,
                ClosingCharge = scheme.ClosingCharge,
                PostingScheme = scheme.PostingScheme,
                CreatedBy = scheme.CreatedBy,
                ModifiedBy = scheme.ModifiedBy,
                CreatedOn = scheme.CreatedOn,
                ModifiedOn = scheme.ModifiedOn,
                LiabilityAccount = new LedgerDetailsDto()
                {
                    Ledger = new LedgerDto()
                    {
                        Id = scheme.LiabilityAccount.LedgerId,
                        Name = scheme.LiabilityAccount.Ledger.Name,
                        NepaliName = scheme.LiabilityAccount.Ledger.NepaliName,
                        EntryDate = scheme.LiabilityAccount.Ledger.EntryDate,
                        IsSubLedgerActive = scheme.LiabilityAccount.Ledger.IsSubLedgerActive,
                        DepreciationRate = scheme.LiabilityAccount.Ledger.DepreciationRate,
                        HisabNumber = scheme.LiabilityAccount.Ledger.HisabNumber
                    },
                    GroupType = new GroupTypeDto()
                    {
                        Id = scheme.LiabilityAccount.GroupTypeId,
                        Name = scheme.LiabilityAccount.GroupType.Name,
                        NepaliName = scheme.LiabilityAccount.GroupType.NepaliName,
                        EntryDate = scheme.LiabilityAccount.GroupType.EntryDate,
                        Schedule = scheme.LiabilityAccount.GroupType.Schedule
                    },
                    AccountType = new AccountTypeDto()
                    {
                        Id = scheme.LiabilityAccount.GroupType.AccountTypeId,
                        Name = scheme.LiabilityAccount.GroupType.AccountType.Name
                    }
                },
                InterestAccount = new LedgerDetailsDto()
                {
                    Ledger = new LedgerDto()
                    {
                        Id = scheme.InterestAccount.LedgerId,
                        Name = scheme.InterestAccount.Ledger.Name,
                        NepaliName = scheme.InterestAccount.Ledger.NepaliName,
                        EntryDate = scheme.InterestAccount.Ledger.EntryDate,
                        IsSubLedgerActive = scheme.InterestAccount.Ledger.IsSubLedgerActive,
                        DepreciationRate = scheme.InterestAccount.Ledger.DepreciationRate,
                        HisabNumber = scheme.InterestAccount.Ledger.HisabNumber
                    },
                    GroupType = new GroupTypeDto()
                    {
                        Id = scheme.InterestAccount.GroupTypeId,
                        Name = scheme.InterestAccount.GroupType.Name,
                        NepaliName = scheme.InterestAccount.GroupType.NepaliName,
                        EntryDate = scheme.InterestAccount.GroupType.EntryDate,
                        Schedule = scheme.InterestAccount.GroupType.Schedule
                    },
                    AccountType = new AccountTypeDto()
                    {
                        Id = scheme.InterestAccount.GroupType.AccountTypeId,
                        Name = scheme.InterestAccount.GroupType.AccountType.Name
                    }
                }
            };
            return tempDepositSchemeDto;
        }

        public async Task<List<DepositSchemeDto>> GetAllDepositSchemeService()
        {
            var depositScheme = await _depositSchemeRepository.GetAllDepositScheme();
            List<DepositSchemeDto> depositSchemeDtos = new List<DepositSchemeDto>();
            foreach (var scheme in depositScheme)
            {
                depositSchemeDtos.Add(GetDepositSchemeDto(scheme));
            }
            return depositSchemeDtos;
        }

        public async Task<DepositSchemeDto> GetDepositSchemeService(int id)
        {
            var scheme = await _depositSchemeRepository.GetDepositScheme(id);
            ResponseDepositScheme depositSchemeWithLedgerDetails = _mapper.Map<ResponseDepositScheme>(scheme);
            var ledgerAsInterest = await _mainLedgerRepository.GetLedgerById(scheme.LedgerAsInterestAccountId);
            var ledgerAsLiability = await _mainLedgerRepository.GetLedgerById(scheme.LedgerAsLiabilityAccountId);
            //await Task.WhenAll(ledgerAsInterest, ledgerAsLiability);
            depositSchemeWithLedgerDetails.LiabilityAccount = ledgerAsLiability;
            depositSchemeWithLedgerDetails.InterestAccount = ledgerAsInterest;
            var depositSchemeDto = GetDepositSchemeDto(depositSchemeWithLedgerDetails);
            return depositSchemeDto;
        }


        // DEPOSIT ACCOUNT
        /// <summary>
        /// This Returns the Unique account number for the given deposit scheme at the current context
        /// </summary>
        /// <param name="depositSchemeId"></param>
        /// <returns></returns>
        public async Task<AccountNumberDto> GetUniqueAccountNumberService(int depositSchemeId)
        {
            var depositScheme = await _depositSchemeRepository.GetDepositScheme(depositSchemeId);
            if (depositScheme == null) throw new NotFoundExceptionHandler("Deposit Scheme Not Found");
            var depositAccounts = await _depositSchemeRepository.GetDepositAccountByDepositScheme(depositSchemeId);
            int count = depositAccounts.Count >= 1 ? depositAccounts.Count + 1 : 1;
            string accountNumber = depositScheme.Symbol + count.ToString("D5");
            AccountNumberDto accountNumberDto = new AccountNumberDto() { AccountNumber = accountNumber };
            return accountNumberDto;
        }

        /// <summary>
        /// Create Deposit Account under provided deposit scheme.
        /// Validation:
        /// 1. Given Interest Rate should be between Minimum and Maximum Interest Rate defined under deposit scheme.
        /// 2. Only when account type is joint we expect jointClientId
        /// 3. For InterestPositing and MatureInterestPositng, if provided account Number is not equal to current account number than, this account number should exist in database
        /// </summary>
        /// <param name="createDepositAccountDto"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CreateDepositAccountService(CreateDepositAccountDto createDepositAccountDto, string createdBy)
        {
            var depositScheme = await _depositSchemeRepository.GetDepositScheme(createDepositAccountDto.DepositSchemeId);
            var client = await _clientRepo.GetClientById(createDepositAccountDto.ClientId);
            string interestPostingAccountNumber = createDepositAccountDto.InterestPostingAccountNumber;
            string? matureInterestPostingAccountNumber = createDepositAccountDto.MatureInterestPostingAccountNumber;
            var givenInterestRate = createDepositAccountDto.InterestRate;
            var minimumInterestRate = depositScheme.MinimumInterestRate;
            var maximumInterestRate = depositScheme.MaximumInterestRate;


            if (
                depositScheme != null
                && client != null
                && minimumInterestRate <= givenInterestRate
                && givenInterestRate <= maximumInterestRate
                && (((int)createDepositAccountDto.AccountType == 1 && createDepositAccountDto.JointClientId < 1)
                || ((int)createDepositAccountDto.AccountType == 1 && createDepositAccountDto.JointClientId == null)
                || ((int)createDepositAccountDto.AccountType == 2 && createDepositAccountDto.JointClientId >= 1))
                && createDepositAccountDto.ClientId != createDepositAccountDto.JointClientId
                )
            {
                if (interestPostingAccountNumber != createDepositAccountDto.AccountNumber)
                {
                    var checkAccountNumberForInterestPosting = await _depositSchemeRepository.GetDepositAccountByAccountNumber(interestPostingAccountNumber);
                    if (checkAccountNumberForInterestPosting == null) throw new NotFoundExceptionHandler("Interest Posting Account Number Doesnot Exist");
                }
                if (matureInterestPostingAccountNumber != null && matureInterestPostingAccountNumber != createDepositAccountDto.AccountNumber)
                {
                    var checkMatureAccountNumber = await _depositSchemeRepository.GetDepositAccountByAccountNumber(matureInterestPostingAccountNumber);
                    if (checkMatureAccountNumber == null) throw new NotFoundExceptionHandler("Mature Interest Posting Account Number Doesnot Exist");
                }
                Client jointClient = new Client();
                DepositAccount newDepositAccount = new DepositAccount();
                if (createDepositAccountDto.JointClientId >= 1)
                {
                    jointClient = await _clientRepo.GetClientById((int)createDepositAccountDto.JointClientId);
                    if (jointClient == null) throw new NotFoundExceptionHandler("Provided Joint Client is not found");
                    newDepositAccount.JointClient = jointClient;
                }

                newDepositAccount.DepositScheme = depositScheme;
                newDepositAccount.AccountNumber = createDepositAccountDto.AccountNumber;
                newDepositAccount.Client = client;
                newDepositAccount.OpeningDate = createDepositAccountDto.OpeningDate;
                newDepositAccount.Period = createDepositAccountDto?.Period;
                newDepositAccount.PeriodType = (int?)createDepositAccountDto.PeriodType;
                newDepositAccount.AccountType = (int)createDepositAccountDto.AccountType;
                newDepositAccount.MatureDate = createDepositAccountDto.MatureDate;
                newDepositAccount.InterestRate = givenInterestRate;
                newDepositAccount.MinimumBalance = createDepositAccountDto.MinimumBalance;
                newDepositAccount.ReferredBy = createDepositAccountDto.ReferredBy;
                newDepositAccount.InterestPostingAccountNumber = interestPostingAccountNumber;
                newDepositAccount.MatureInterestPostingAccountNumber = matureInterestPostingAccountNumber;
                newDepositAccount.Description = createDepositAccountDto.Description;
                newDepositAccount.Status = (int)createDepositAccountDto.Status;
                newDepositAccount.IsSMSServiceActive = createDepositAccountDto.IsSMSServiceActive;
                newDepositAccount.DailyDepositAmount = createDepositAccountDto?.DailyDepositAmount;
                newDepositAccount.TotalDepositAmount = createDepositAccountDto?.TotalDepositAmount;
                newDepositAccount.TotalReturnAmount = createDepositAccountDto?.TotalReturnAmount;
                newDepositAccount.TotalInterestAmount = createDepositAccountDto?.TotalInterestAmount;
                newDepositAccount.TotalDepositDay = createDepositAccountDto?.TotalDepositDay;
                newDepositAccount.CreatedBy = createdBy;
                newDepositAccount.CreatedOn = DateTime.Now;

                int createStatus = await _depositSchemeRepository.CreateDepositAccount(newDepositAccount);
                if (createStatus >= 1)
                {
                    return new ResponseDto()
                    {
                        Message = "Account Created Successfully",
                        Status = true,
                        StatusCode = "200"
                    };
                }
                return new ResponseDto()
                {
                    Message = "Account Create Failed",
                    Status = false,
                    StatusCode = "500"
                };
            }

            else
            {
                if (depositScheme == null) throw new NotFoundExceptionHandler("Deposit Scheme Not Found");
                else if (client == null) throw new NotFoundExceptionHandler("Client Not Found");
                else if (minimumInterestRate > givenInterestRate) throw new Exception($"Provided Interest Rate should be greater than Minimum Interest Rate.Interest Rate should be more than {minimumInterestRate}");
                else if (givenInterestRate > maximumInterestRate) throw new Exception($"Provided Interest Rate Should be less than Maximum Interest Rate. Interest Rate should be less than {maximumInterestRate}");
                else if (
                ((int)createDepositAccountDto.AccountType == 1 && createDepositAccountDto.JointClientId >= 1)
                || ((int)createDepositAccountDto.AccountType == 2 && createDepositAccountDto.JointClientId < 1)
                || ((int)createDepositAccountDto.AccountType == 2 && createDepositAccountDto.JointClientId == null))
                    throw new Exception("Provide Joint Client Details if you are trying to open joint account otherwise leave the field blank");
                else if (createDepositAccountDto.ClientId == createDepositAccountDto.JointClientId)
                    throw new Exception("Client Details and Joint Client Details should no be same");
                else throw new Exception("Failed to perform the operation");
            }

        }

        public async Task<ResponseDto> UpdateDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, string modifiedBy)
        {
            var status = await _depositSchemeRepository.UpdateDepositAccount(updateDepositAccountDto, modifiedBy);
            if (status >= 1)
                return new ResponseDto()
                {
                    Message = "Updated Successfully",
                    Status = true,
                    StatusCode = "200"
                };

            return new ResponseDto()
            {
                Message = "Update Failed",
                Status = false,
                StatusCode = "500"
            };

        }

        private async Task<DepositAccountDto> GetDepositAccountDto(DepositAccount depositAccount)
        {
            DepositAccountDto depositAccountDto = _mapper.Map<DepositAccountDto>(depositAccount);
            AccountTypeEnum accountTypeEnum = (AccountTypeEnum)depositAccount.AccountType;
            depositAccountDto.AccountType = Enum.GetName(typeof(AccountTypeEnum), accountTypeEnum);
            if (depositAccount.PeriodType >= 1)
            {
                PeriodTypeEnum periodTypeEnum = (PeriodTypeEnum)depositAccount.PeriodType;
                depositAccountDto.PeriodType = Enum.GetName(typeof(PeriodTypeEnum), periodTypeEnum);
            }
            AccountStatusEnum accountStatusEnum = (AccountStatusEnum)depositAccount.Status;
            depositAccountDto.Status = Enum.GetName(typeof(AccountStatusEnum), accountStatusEnum);
            // GET ALL DEPOSIT SCHEME DETAILS
            ResponseDepositScheme depositSchemeWithLedgerDetails = _mapper.Map<ResponseDepositScheme>(depositAccount.DepositScheme);
            var ledgerAsInterest = await _mainLedgerRepository.GetLedgerById(depositAccount.DepositScheme.LedgerAsInterestAccountId);
            var ledgerAsLiability = await _mainLedgerRepository.GetLedgerById(depositAccount.DepositScheme.LedgerAsLiabilityAccountId);
            depositSchemeWithLedgerDetails.LiabilityAccount = ledgerAsLiability;
            depositSchemeWithLedgerDetails.InterestAccount = ledgerAsInterest;
            depositAccountDto.DepositScheme = GetDepositSchemeDto(depositSchemeWithLedgerDetails);
            //////////////////////////////////////////////////
            depositAccountDto.Client = _mapper.Map<ClientDto>(await _clientRepo.GetClient(depositAccount.ClientId));
            if (depositAccount.JointClientId >= 1)
            {
                depositAccountDto.JointClient = _mapper.Map<ClientDto>(await _clientRepo.GetClient((int)depositAccount.JointClientId));
            }

            return depositAccountDto;
        }
        public async Task<DepositAccountDto> GetDepositAccountByIdService(int id)
        {
            var depositAccount = await _depositSchemeRepository.GetDepositAccountById(id);
            return await GetDepositAccountDto(depositAccount);
        }

        public async Task<DepositAccountDto> GetDepositAccountByAccountNumberService(string accountNumber)
        {
            var depositAccount = await _depositSchemeRepository.GetDepositAccountByAccountNumber(accountNumber);
            return await GetDepositAccountDto(depositAccount);
        }

        public async Task<List<DepositAccountDto>> GetAllDepositAccountService()
        {
            List<DepositAccountDto> depositAccountDtos = new List<DepositAccountDto>();
            var depositAccounts = await _depositSchemeRepository.GetDepositAccountListAsync();
            if (depositAccounts.Count >= 1 && depositAccounts != null)
            {
                foreach (var account in depositAccounts)
                {
                    depositAccountDtos.Add(await GetDepositAccountDto(account));
                }
            }
            return depositAccountDtos;
        }

        public async Task<List<DepositAccountDto>> GetAllDepositAccountByDepositSchemeService(int depositSchemeId)
        {
            List<DepositAccountDto> depositAccountDtos = new List<DepositAccountDto>();
            var depositAccounts = await _depositSchemeRepository.GetDepositAccountByDepositScheme(depositSchemeId);
            if (depositAccounts.Count >= 1 && depositAccounts != null)
            {
                foreach (var account in depositAccounts)
                {
                    depositAccountDtos.Add(await GetDepositAccountDto(account));
                }
            }
            return depositAccountDtos;
        }

        // FLEXIBLE INTEREST AND INTEREST CHANGING FUNCTIONS

        public async Task<ResponseDto> UpdateInterestRateAccordingToFlexibleInterestRateService(FlexibleInterestRateSetupDto flexibleInterestRateSetupDto)
        {
            // Validation
            // 1. If Given Interest Rate is between Minimum and Maximum Interest Rate
            var depositScheme = await _depositSchemeRepository.GetDepositScheme(flexibleInterestRateSetupDto.DepositSchemeId);
            if (depositScheme != null && depositScheme.Id >= 1 && depositScheme.MinimumInterestRate <= flexibleInterestRateSetupDto.InterestRate && depositScheme.MaximumInterestRate >= flexibleInterestRateSetupDto.InterestRate)
            {
                FlexibleInterestRate flexibleInterestRate = _mapper.Map<FlexibleInterestRate>(flexibleInterestRateSetupDto);
                var createFlexibleInterestRateStatus = await _depositSchemeRepository.CreateFlexibleInterestRate(flexibleInterestRate);
                if (createFlexibleInterestRateStatus <= 0) throw new Exception("Unable to Create Flexible Interest Rate");
                var updateInterestRateAccordingToFlexibleInterestRateStatus = await _depositSchemeRepository.UpdateInterestRateAccordingToFlexibleInterestRate(flexibleInterestRate);
                if (updateInterestRateAccordingToFlexibleInterestRateStatus <= 0) throw new Exception("Unable to Update the Interest Rate");
                return new ResponseDto()
                {
                    Message = "Update Successfully",
                    Status = true,
                    StatusCode = "500"
                };
            }
            else if(depositScheme==null || depositScheme.Id<=0)
                throw new Exception("Deposit Scheme Not Found");
            else if(depositScheme.MinimumInterestRate > flexibleInterestRateSetupDto.InterestRate)
                throw new Exception($"Interest Rate Should be greater than Minimum Interest Rate. Minimum Interest Rate in your condition is: {depositScheme.MinimumInterestRate}");
            else if(depositScheme.MaximumInterestRate < flexibleInterestRateSetupDto.InterestRate)
                throw new Exception($"Interest Rate Should be less than Maximum Interest Rate. Maximum Interest Rate in your condition is: {depositScheme.MaximumInterestRate}");
            else
                throw new Exception("Something Worng Happend");
        }

        public async Task<ResponseDto> IncrementOrDecrementOfInterestRateService(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto)
        {
            var updateStatus = await _depositSchemeRepository.IncrementOrDecrementOfInterestRate(updateInterestRateByDepositSchemeDto);
            if (updateStatus >= 1)
                return new ResponseDto()
                {
                    Message = "Update Successfully",
                    Status = true,
                    StatusCode = "200"
                };
            return new ResponseDto()
            {
                Message = "Update Failed",
                Status = false,
                StatusCode = "500"
            };
        }

        public async Task<ResponseDto> ChangeInterestRateAccordingToPastInterestRateService(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto)
        {
            // Validation
            // 1. If New Interest Rate is between Minimum and Maximum Interest Rate
            var depositScheme = await _depositSchemeRepository.GetDepositScheme(changeInterestRateByDepositSchemeDto.DepositSchemeId);
            if (
             depositScheme != null && depositScheme.Id >= 1 &&
             depositScheme.MinimumInterestRate <= changeInterestRateByDepositSchemeDto.NewInterestRate
             && depositScheme.MaximumInterestRate >= changeInterestRateByDepositSchemeDto.NewInterestRate)
            {
                var updateStatus = await _depositSchemeRepository.ChangeInterestRateAccordingToPastInterestRate(changeInterestRateByDepositSchemeDto);
                if(updateStatus>=1) return new ResponseDto(){Message="Update Successfull", Status=true, StatusCode="200"};
                return new ResponseDto(){Message="Update Failed", Status=false, StatusCode="500"};

            }
            else if(depositScheme==null || depositScheme.Id<=0)
                throw new Exception("Deposit Scheme Not Found");
            else if(depositScheme.MinimumInterestRate > changeInterestRateByDepositSchemeDto.NewInterestRate)
                throw new Exception($"New Interest Rate Should be greater than Minimum Interest Rate. Minimum Interest Rate in your condition is: {depositScheme.MinimumInterestRate}");
            else if(depositScheme.MaximumInterestRate < changeInterestRateByDepositSchemeDto.NewInterestRate)
                throw new Exception($"New Interest Rate Should be less than Maximum Interest Rate. Maximum Interest Rate in your condition is: {depositScheme.MaximumInterestRate}");
            else
                throw new Exception("Something Worng Happend");
        }
    }
}