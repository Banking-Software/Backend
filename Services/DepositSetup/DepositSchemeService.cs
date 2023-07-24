using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Models.AccountSetup;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Wrapper;
using MicroFinance.Repository.AccountSetup.MainLedger;
using MicroFinance.Repository.ClientSetup;
using MicroFinance.Repository.DepositSetup;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.UserManagement;

namespace MicroFinance.Services.DepositSetup
{
    public class DepositSchemeService : IDepositSchemeService
    {
        private readonly ILogger<DepositSchemeDto> _loggger;
        private readonly IMapper _mapper;
        private readonly IDepositSchemeRepository _depositSchemeRepository;
        private readonly IMainLedgerRepository _mainLedgerRepository;
        private readonly IClientRepository _clientRepo;
        private readonly ICompanyProfileService _companyProfileService;
        private readonly IEmployeeService _employeeService;

        public DepositSchemeService
        (
        ILogger<DepositSchemeDto> logger,
        IMapper mapper,
        IDepositSchemeRepository depositSchemeRepository,
        IMainLedgerRepository mainLedgerRepository,
        IClientRepository clientRepository,
        ICompanyProfileService companyProfileService,
        IEmployeeService employeeService
        )
        {
            _loggger = logger;
            _mapper = mapper;
            _depositSchemeRepository = depositSchemeRepository;
            _mainLedgerRepository = mainLedgerRepository;
            _clientRepo = clientRepository;
            _companyProfileService = companyProfileService;
            _employeeService = employeeService;
        }


        private async Task<List<SubLedger>> CreateSubLedgerForDepositScheme(CreateDepositSchemeDto createDepositScheme)
        {
            var depositSubledgerName = createDepositScheme.DepositSubledger ?? createDepositScheme.SchemeName + " " + "Deposit";
            var interestSubledgerName = createDepositScheme.InterestSubledger ?? createDepositScheme.SchemeName + " " + "Interest";
            var taxSublegderName = createDepositScheme.TaxSubledger ?? createDepositScheme.SchemeName + " " + "Tax";
            // Deposit SubLedger
            int depositLedgerId = (int)createDepositScheme.SchemeType;
            Ledger depsoitLedgerSchemeType = await _mainLedgerRepository.GetLedger(depositLedgerId);
            SubLedger depositSubledger = new SubLedger() { Name = depositSubledgerName, Ledger = depsoitLedgerSchemeType, Description = $"Subledger created while creating Deposit Scheme {createDepositScheme.SchemeName}" };
            // Interest SubLedger
            Ledger interestLedger = await _mainLedgerRepository.GetLedger(68); // Interest Expenses
            SubLedger interestSubledger = new SubLedger() { Name = interestSubledgerName, Ledger = interestLedger, Description = $"Subledger created while creating Deposit Scheme {createDepositScheme.SchemeName}" };
            // Tax SubLedger
            Ledger taxLedger = await _mainLedgerRepository.GetLedger(29); // Tax Payable
            SubLedger taxSubledger = new SubLedger() { Name = taxSublegderName, Ledger = taxLedger, Description = $"Subledger created while creating Deposit Scheme {createDepositScheme.SchemeName}" };
            List<SubLedger> allSublegders = new List<SubLedger>() { depositSubledger, interestSubledger, taxSubledger };
            int subledgerCreateStatus = await _mainLedgerRepository.CreateMultipleSubLedger(allSublegders);
            if (subledgerCreateStatus < 1) throw new Exception("Unable to create the Scheme. Subledger creation failed");
            return new List<SubLedger>() { depositSubledger, interestSubledger, taxSubledger };

        }
        public async Task<ResponseDto> CreateDepositSchemeService(CreateDepositSchemeDto createDepositScheme, TokenDto decodedToken)
        {
            var depositSchemeWithSameName = await _depositSchemeRepository.GetDepositSchemeByName(createDepositScheme.SchemeName);
            var depositSchemeWithSameSymbol = await _depositSchemeRepository.GetDepositSchemeBySymbol(createDepositScheme.Symbol);
            if (depositSchemeWithSameName != null || depositSchemeWithSameSymbol != null)
                throw new Exception("Deposit Scheme with same name or symbol exist");

            var companyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            var depositScheme = _mapper.Map<DepositScheme>(createDepositScheme);
            List<SubLedger> subLedgersForDepositScheme = await CreateSubLedgerForDepositScheme(createDepositScheme);
            depositScheme.SchemeType = await _mainLedgerRepository.GetLedger((int)createDepositScheme.SchemeType);
            depositScheme.DepositSubLedger = subLedgersForDepositScheme[0];
            depositScheme.InterestSubledger = subLedgersForDepositScheme[1];
            depositScheme.TaxSubledger = subLedgersForDepositScheme[2];
            depositScheme.CreatedBy = decodedToken.UserName;
            depositScheme.CreatorId = decodedToken.UserId;
            depositScheme.BranchCode = decodedToken.BranchCode;
            depositScheme.RealWorldCreationDate = DateTime.Now;
            depositScheme.CompanyCalendarCreationDate = new DateTime(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            var depositSchemeId = await _depositSchemeRepository.CreateDepositScheme(depositScheme);
            return new ResponseDto()
            {
                Message = $"'{depositScheme.SchemeName}' created successfully",
                Status = true,
                StatusCode = "200"
            };

        }

        public async Task<ResponseDto> UpdateDepositSchemeService(UpdateDepositSchemeDto updateDepositScheme, TokenDto decodedToken)
        {
            var existingDepositScheme = await _depositSchemeRepository.GetDepositSchemeById(updateDepositScheme.Id);
            var companyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            if (!existingDepositScheme.IsActive && !updateDepositScheme.IsActive)
            {
                _loggger.LogError($"{DateTime.Now}: Employee {decodedToken.UserName} tried to edit the inactive deposit scheme '{existingDepositScheme.SchemeName}'");
                throw new Exception("Deposit Scheme you want update is In-Active. Please activate it and then update the infornation");
            }
            else if (!existingDepositScheme.IsActive && updateDepositScheme.IsActive)
            {
                _loggger.LogInformation($"{DateTime.Now}: Employee {decodedToken.UserName} sends request to activate '{existingDepositScheme.SchemeName}' depsoit scheme");
            }
            else if (existingDepositScheme.IsActive && !updateDepositScheme.IsActive)
            {
                _loggger.LogInformation($"{DateTime.Now}: Employee {decodedToken.UserName} sends request to deactivate '{existingDepositScheme.SchemeName}' depsoit scheme");
            }
            existingDepositScheme.InterestRate = updateDepositScheme.InterestRate;
            existingDepositScheme.InterestRateOnMinimumBalance = updateDepositScheme.InterestRateOnMinimumBalance;
            existingDepositScheme.MaximumInterestRate = updateDepositScheme.MaximumInterestRate;
            existingDepositScheme.MinimumInterestRate = updateDepositScheme.MinimumInterestRate;
            existingDepositScheme.IsActive = updateDepositScheme.IsActive;
            existingDepositScheme.ModifiedBy = decodedToken.UserName;
            existingDepositScheme.ModifierId = decodedToken.UserId;
            existingDepositScheme.ModifierBranchCode = decodedToken.BranchCode;
            existingDepositScheme.RealWorldModificationDate = DateTime.Now;
            existingDepositScheme.CompanyCalendarModificationDate = new DateTime(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            var updateStatus = await _depositSchemeRepository.UpdateDepositScheme(existingDepositScheme);
            if (updateStatus < 1)
                throw new Exception("Failed to update the Deposit Scheme");
            return new ResponseDto()
            {
                Message = "Successfully updated the Scheme",
                Status = true,
                StatusCode = "200"
            };
        }


        public async Task<List<DepositSchemeDto>> GetAllDepositSchemeService()
        {
            var allDepositScheme = await _depositSchemeRepository.GetAllDepositScheme();
            if (allDepositScheme != null)
            {
                return _mapper.Map<List<DepositSchemeDto>>(allDepositScheme);
            }
            throw new Exception("Null Deposit Scheme");
        }

        public async Task<DepositSchemeDto> GetDepositSchemeByIdService(int id)
        {
            var depositScheme = await _depositSchemeRepository.GetDepositSchemeById(id);
            if (depositScheme != null)
                return _mapper.Map<DepositSchemeDto>(depositScheme);
            throw new Exception("No Deposit Scheme Found");
        }


        private async Task AddClientInDepositAccount(CreateDepositAccountDto createDepositAccountDto, DepositAccount newDepositAccount, TokenDto decodedToken)
        {
            var client = await _clientRepo.GetClientById(createDepositAccountDto.ClientId);
            if (client == null || !client.IsActive)
            {
                throw new Exception
                ($"UnAuthorized to proceed. Client '{client?.ClientFirstName}' has active status '{client?.IsActive}'.");
            }
            if (client.BranchCode != decodedToken.BranchCode)
                throw new Exception("Given Client is not found under your branch");
            newDepositAccount.Client = client;
        }

        private async Task AddDepositSchemeInDepositAccount(CreateDepositAccountDto createDepositAccountDto, DepositAccount newDepositAccount)
        {
            var depositScheme = await _depositSchemeRepository.GetDepositSchemeById(createDepositAccountDto.DepositSchemeId);
            if (depositScheme == null || !depositScheme.IsActive)
            {
                throw new Exception($"Provided Deposit Scheme '{depositScheme?.SchemeName}' active status is '{depositScheme?.IsActive}'");
            }
            if (createDepositAccountDto.InterestRate < depositScheme.MinimumInterestRate || createDepositAccountDto.InterestRate > depositScheme.MaximumInterestRate)
            {
                throw new Exception($"MinimumInterestRate<=InterestRate<=MaximumInterestRate constraint doesnot match. Available minimum Interest Rate is {depositScheme.MinimumInterestRate} and maximum interest rate is {depositScheme.MaximumInterestRate}");
            }
            newDepositAccount.DepositScheme = depositScheme;
        }

        private async Task AddInterestAndMaturePostingAccountInDepositAccount(CreateDepositAccountDto createDepositAccountDto, DepositAccount newDepositAccount, TokenDto decodedToken)
        {
            if (createDepositAccountDto.InterestPostingAccountId != null)
            {
                var interestPostingAccountNumber = await _depositSchemeRepository.GetDepositAccountById((int)createDepositAccountDto.InterestPostingAccountId);
                if (interestPostingAccountNumber == null || interestPostingAccountNumber.BranchCode != decodedToken.BranchCode)
                    throw new Exception("InterestPostingAccountNumber: Cannot Find Account Number under your branch");
                newDepositAccount.InterestPostingAccountNumber = interestPostingAccountNumber;
            }
            if (createDepositAccountDto.MatureInterestPostingAccountId != null)
            {
                var matureInterestPostingAccountNumber = await _depositSchemeRepository.GetDepositAccountById((int)createDepositAccountDto.MatureInterestPostingAccountId);
                if (matureInterestPostingAccountNumber == null || matureInterestPostingAccountNumber.BranchCode != decodedToken.BranchCode)
                    throw new Exception("MatureInterestPositingAccountNumber: Cannot Find Account Number under your branch");
                newDepositAccount.MatureInterestPostingAccountNumber = matureInterestPostingAccountNumber;
            }
        }
        private async Task AddReferredByEmployeeInDepositAccount(CreateDepositAccountDto createDepositAccountDto, DepositAccount newDepositAccount, TokenDto decodedToken)
        {
            if (createDepositAccountDto.ReferredByEmployeeId != null)
            {
                var referredByEmployee = await _employeeService.GetEmployeeById((int)createDepositAccountDto.ReferredByEmployeeId);
                if (referredByEmployee == null || referredByEmployee.BranchCode != decodedToken.BranchCode)
                    throw new Exception("ReferredByEmployeeId: Provided Employee Details not found under your branch");
                newDepositAccount.ReferredByEmployeeId = (int)createDepositAccountDto.ReferredByEmployeeId;
            }
        }

        private async Task<List<Client>> GetAllJointClientDetails(List<int> jointClientIds, TokenDto decodedToken)
        {
            List<Client> listOfAllJointClients = new List<Client>();
            foreach (var jointClientId in jointClientIds)
            {
                var jointClient = await _clientRepo.GetClientById(jointClientId);
                if (jointClient == null || jointClient.BranchCode != decodedToken.BranchCode)
                    throw new Exception($"Provided Joint Clients are not found under your branch. Id:{jointClientId}");
                if (!jointClient.IsActive)
                    throw new Exception($"Joint Client {jointClient.ClientId} is inactive");
                listOfAllJointClients.Add(jointClient);
            }
            return listOfAllJointClients;
        }
        private async Task AddBasicDetailsInDepositAccount(DepositAccount newDepositAccount, TokenDto decodedToken)
        {
            var companyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            newDepositAccount.RealWorldCreationDate = DateTime.Now;
            newDepositAccount.CompanyCalendarCreationDate = new DateTime(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            newDepositAccount.BranchCode = decodedToken.BranchCode;
            newDepositAccount.CreatedBy = decodedToken.UserName;
            newDepositAccount.CreatorId = decodedToken.UserId;
            newDepositAccount.OpeningDate = new DateTime(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            if (newDepositAccount.PeriodType == PeriodTypeEnum.Year)
                newDepositAccount.MatureDate = (newDepositAccount.OpeningDate).AddYears(newDepositAccount.Period).AddDays(-1);
            else if (newDepositAccount.PeriodType == PeriodTypeEnum.Month)
                newDepositAccount.MatureDate = (newDepositAccount.OpeningDate).AddMonths(newDepositAccount.Period).AddDays(-1);
            else
                newDepositAccount.MatureDate = (newDepositAccount.OpeningDate).AddDays(newDepositAccount.Period - 1);
        }

        public async Task<ResponseDto> CreateDepositAccountService(CreateDepositAccountDto createDepositAccountDto, TokenDto decodedToken)
        {

            DepositAccount newDepositAccount = _mapper.Map<DepositAccount>(createDepositAccountDto);
            await AddClientInDepositAccount(createDepositAccountDto, newDepositAccount, decodedToken);
            await AddDepositSchemeInDepositAccount(createDepositAccountDto, newDepositAccount);
            await AddInterestAndMaturePostingAccountInDepositAccount(createDepositAccountDto, newDepositAccount, decodedToken);
            await AddReferredByEmployeeInDepositAccount(createDepositAccountDto, newDepositAccount, decodedToken);
            await AddBasicDetailsInDepositAccount(newDepositAccount, decodedToken);
            List<Client> jointClients =
            createDepositAccountDto.AccountType == AccountTypeEnum.Joint
            ?
            await GetAllJointClientDetails(createDepositAccountDto.JointClientIds, decodedToken)
            :
            null;
            var depositAccountId = await _depositSchemeRepository.CreateDepositAccount(newDepositAccount);
            if (depositAccountId >= 1)
            {
                if (createDepositAccountDto.AccountType == AccountTypeEnum.Joint)
                    await CreateJointAccountService(jointClients, newDepositAccount);
                return new ResponseDto()
                {
                    Message = $"Successfully created '{newDepositAccount.AccountNumber}' account number",
                    Status = true,
                    StatusCode = "200"
                };
            }
            throw new Exception("Unable to Create Deposit Account");
        }
        private async Task CreateJointAccountService(List<Client> jointClients, DepositAccount depositAccount)
        {
            List<JointAccount> jointAccounts = new List<JointAccount>();
            DateTime RealWorldStartDate = DateTime.Now;
            DateTime CompanyCalendarStartDate = depositAccount.CompanyCalendarCreationDate;
            foreach (var jointClient in jointClients)
            {
                var jointAccount = new JointAccount()
                {
                    JointClient = jointClient,
                    DepositAccount = depositAccount,
                    RealWorldStartDate = RealWorldStartDate,
                    CompanyCalendarStartDate = CompanyCalendarStartDate
                };
                jointAccounts.Add(jointAccount);
            }
            await _depositSchemeRepository.CreateJointAccount(jointAccounts, depositAccount);
        }

        private Task<DepositAccountWrapperDto> MapDepositAccountWrapperToDepositAccountWrapperDto(DepositAccountWrapper depositAccountWrapper)
        {
            DepositAccountWrapperDto depositAccountWrapperDto = new DepositAccountWrapperDto();
            var depositAccount = depositAccountWrapper.DepositAccount;
            var jointAccounts = depositAccountWrapper?.JointAccount;
            var client = depositAccountWrapper.DepositAccount.Client;
            var depositScheme = depositAccountWrapper.DepositAccount.DepositScheme;
            depositAccountWrapperDto.DepositAccount = _mapper.Map<DepositAccountDto>(depositAccount);
            depositAccountWrapperDto.Client = _mapper.Map<ClientDto>(client);
            depositAccountWrapperDto.DepositScheme = _mapper.Map<DepositSchemeDto>(depositScheme);

            if (jointAccounts != null && jointAccounts.Count >= 1)
                depositAccountWrapperDto.JointClients = _mapper.Map<List<JointAccountDto>>(jointAccounts);
            depositAccountWrapperDto.DepositAccount.InterestPostingAccountNumber = depositAccount.InterestPostingAccountNumber?.AccountNumber;
            depositAccountWrapperDto.DepositAccount.MatureInterestPostingAccountNumber = depositAccount.MatureInterestPostingAccountNumber?.AccountNumber;
            return Task.FromResult(depositAccountWrapperDto);
        }

        public async Task<List<DepositAccountWrapperDto>> GetAllNonClosedDepositAccountService()
        {
            var allNonClosedDepositAccounts = await _depositSchemeRepository.GetAllNonClosedDepositAccounts();
            if (allNonClosedDepositAccounts == null || allNonClosedDepositAccounts.Count < 1)
                return new List<DepositAccountWrapperDto>();
            List<DepositAccountWrapperDto> allDepositAccountWrapperDto = new List<DepositAccountWrapperDto>();
            foreach (var depositAccountWrapper in allNonClosedDepositAccounts)
            {
                var depositAccountWrapperDto = await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
                allDepositAccountWrapperDto.Add(depositAccountWrapperDto);
            }
            return allDepositAccountWrapperDto;
        }

        public async Task<DepositAccountWrapperDto> GetNonClosedDepositAccountById(int depositAccountId)
        {
            var depositAccountWrapper = await _depositSchemeRepository.GetNonCloseDepositAccountById(depositAccountId);
            if(depositAccountWrapper!=null && depositAccountWrapper.DepositAccount!=null)
                return await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
            throw new Exception("No data found");
        }


        // public async Task<ResponseDto> UpdateDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, string modifiedBy)
        // {
        //     var status = await _depositSchemeRepository.UpdateDepositAccount(updateDepositAccountDto, modifiedBy);
        //     if (status >= 1)
        //         return new ResponseDto()
        //         {
        //             Message = "Updated Successfully",
        //             Status = true,
        //             StatusCode = "200"
        //         };

        //     return new ResponseDto()
        //     {
        //         Message = "Update Failed",
        //         Status = false,
        //         StatusCode = "500"
        //     };

        // }

        // private async Task<DepositAccountDto> GetDepositAccountDto(DepositAccount depositAccount)
        // {
        //     DepositAccountDto depositAccountDto = _mapper.Map<DepositAccountDto>(depositAccount);
        //     AccountTypeEnum accountTypeEnum = (AccountTypeEnum)depositAccount.AccountType;
        //     depositAccountDto.AccountType = Enum.GetName(typeof(AccountTypeEnum), accountTypeEnum);
        //     if (depositAccount.PeriodType >= 1)
        //     {
        //         PeriodTypeEnum periodTypeEnum = (PeriodTypeEnum)depositAccount.PeriodType;
        //         depositAccountDto.PeriodType = Enum.GetName(typeof(PeriodTypeEnum), periodTypeEnum);
        //     }
        //     AccountStatusEnum accountStatusEnum = (AccountStatusEnum)depositAccount.Status;
        //     depositAccountDto.Status = Enum.GetName(typeof(AccountStatusEnum), accountStatusEnum);
        //     // GET ALL DEPOSIT SCHEME DETAILS
        //     ResponseDepositScheme depositSchemeWithLedgerDetails = _mapper.Map<ResponseDepositScheme>(depositAccount.DepositScheme);
        //     var ledgerAsInterest = await _mainLedgerRepository.GetLedger(depositAccount.DepositScheme.LedgerAsInterestAccountId);
        //     var ledgerAsLiability = await _mainLedgerRepository.GetLedger(depositAccount.DepositScheme.LedgerAsLiabilityAccountId);
        //     depositSchemeWithLedgerDetails.LiabilityAccount = ledgerAsLiability;
        //     depositSchemeWithLedgerDetails.InterestAccount = ledgerAsInterest;
        //     depositAccountDto.DepositScheme = GetDepositSchemeDto(depositSchemeWithLedgerDetails);
        //     //////////////////////////////////////////////////
        //     depositAccountDto.Client = _mapper.Map<ClientDto>(await _clientRepo.GetClientById(depositAccount.ClientId));
        //     if (depositAccount.JointClientId >= 1)
        //     {
        //         depositAccountDto.JointClient = _mapper.Map<ClientDto>(await _clientRepo.GetClientById((int)depositAccount.JointClientId));
        //     }

        //     return depositAccountDto;
        // }
        // public async Task<DepositAccountDto> GetDepositAccountByIdService(int id)
        // {
        //     var depositAccount = await _depositSchemeRepository.GetDepositAccountById(id);
        //     return await GetDepositAccountDto(depositAccount);
        // }

        // public async Task<DepositAccountDto> GetDepositAccountByAccountNumberService(string accountNumber)
        // {
        //     var depositAccount = await _depositSchemeRepository.GetDepositAccountByAccountNumber(accountNumber);
        //     return await GetDepositAccountDto(depositAccount);
        // }

        // public async Task<List<DepositAccountDto>> GetAllDepositAccountService()
        // {
        //     List<DepositAccountDto> depositAccountDtos = new List<DepositAccountDto>();
        //     var depositAccounts = await _depositSchemeRepository.GetDepositAccountListAsync();
        //     if (depositAccounts.Count >= 1 && depositAccounts != null)
        //     {
        //         foreach (var account in depositAccounts)
        //         {
        //             depositAccountDtos.Add(await GetDepositAccountDto(account));
        //         }
        //     }
        //     return depositAccountDtos;
        // }

        // public async Task<List<DepositAccountDto>> GetAllDepositAccountByDepositSchemeService(int depositSchemeId)
        // {
        //     List<DepositAccountDto> depositAccountDtos = new List<DepositAccountDto>();
        //     var depositAccounts = await _depositSchemeRepository.GetDepositAccountByDepositScheme(depositSchemeId);
        //     if (depositAccounts.Count >= 1 && depositAccounts != null)
        //     {
        //         foreach (var account in depositAccounts)
        //         {
        //             depositAccountDtos.Add(await GetDepositAccountDto(account));
        //         }
        //     }
        //     return depositAccountDtos;
        // }

        // // FLEXIBLE INTEREST AND INTEREST CHANGING FUNCTIONS

        // public async Task<ResponseDto> UpdateInterestRateAccordingToFlexibleInterestRateService(FlexibleInterestRateSetupDto flexibleInterestRateSetupDto)
        // {
        //     // Validation
        //     // 1. If Given Interest Rate is between Minimum and Maximum Interest Rate
        //     var depositScheme = await _depositSchemeRepository.GetDepositScheme(flexibleInterestRateSetupDto.DepositSchemeId);
        //     if (depositScheme != null && depositScheme.Id >= 1 && depositScheme.MinimumInterestRate <= flexibleInterestRateSetupDto.InterestRate && depositScheme.MaximumInterestRate >= flexibleInterestRateSetupDto.InterestRate)
        //     {
        //         FlexibleInterestRate flexibleInterestRate = _mapper.Map<FlexibleInterestRate>(flexibleInterestRateSetupDto);
        //         var createFlexibleInterestRateStatus = await _depositSchemeRepository.CreateFlexibleInterestRate(flexibleInterestRate);
        //         if (createFlexibleInterestRateStatus <= 0) throw new Exception("Unable to Create Flexible Interest Rate");
        //         var updateInterestRateAccordingToFlexibleInterestRateStatus = await _depositSchemeRepository.UpdateInterestRateAccordingToFlexibleInterestRate(flexibleInterestRate);
        //         if (updateInterestRateAccordingToFlexibleInterestRateStatus <= 0) throw new Exception("Unable to Update the Interest Rate");
        //         return new ResponseDto()
        //         {
        //             Message = "Update Successfully",
        //             Status = true,
        //             StatusCode = "500"
        //         };
        //     }
        //     else if(depositScheme==null || depositScheme.Id<=0)
        //         throw new Exception("Deposit Scheme Not Found");
        //     else if(depositScheme.MinimumInterestRate > flexibleInterestRateSetupDto.InterestRate)
        //         throw new Exception($"Interest Rate Should be greater than Minimum Interest Rate. Minimum Interest Rate in your condition is: {depositScheme.MinimumInterestRate}");
        //     else if(depositScheme.MaximumInterestRate < flexibleInterestRateSetupDto.InterestRate)
        //         throw new Exception($"Interest Rate Should be less than Maximum Interest Rate. Maximum Interest Rate in your condition is: {depositScheme.MaximumInterestRate}");
        //     else
        //         throw new Exception("Something Worng Happend");
        // }

        // public async Task<ResponseDto> IncrementOrDecrementOfInterestRateService(UpdateInterestRateByDepositSchemeDto updateInterestRateByDepositSchemeDto)
        // {
        //     var updateStatus = await _depositSchemeRepository.IncrementOrDecrementOfInterestRate(updateInterestRateByDepositSchemeDto);
        //     if (updateStatus >= 1)
        //         return new ResponseDto()
        //         {
        //             Message = "Update Successfully",
        //             Status = true,
        //             StatusCode = "200"
        //         };
        //     return new ResponseDto()
        //     {
        //         Message = "Update Failed",
        //         Status = false,
        //         StatusCode = "500"
        //     };
        // }

        // public async Task<ResponseDto> ChangeInterestRateAccordingToPastInterestRateService(ChangeInterestRateByDepositSchemeDto changeInterestRateByDepositSchemeDto)
        // {
        //     // Validation
        //     // 1. If New Interest Rate is between Minimum and Maximum Interest Rate
        //     var depositScheme = await _depositSchemeRepository.GetDepositScheme(changeInterestRateByDepositSchemeDto.DepositSchemeId);
        //     if (
        //      depositScheme != null && depositScheme.Id >= 1 &&
        //      depositScheme.MinimumInterestRate <= changeInterestRateByDepositSchemeDto.NewInterestRate
        //      && depositScheme.MaximumInterestRate >= changeInterestRateByDepositSchemeDto.NewInterestRate)
        //     {
        //         var updateStatus = await _depositSchemeRepository.ChangeInterestRateAccordingToPastInterestRate(changeInterestRateByDepositSchemeDto);
        //         if(updateStatus>=1) return new ResponseDto(){Message="Update Successfull", Status=true, StatusCode="200"};
        //         return new ResponseDto(){Message="Update Failed", Status=false, StatusCode="500"};

        //     }
        //     else if(depositScheme==null || depositScheme.Id<=0)
        //         throw new Exception("Deposit Scheme Not Found");
        //     else if(depositScheme.MinimumInterestRate > changeInterestRateByDepositSchemeDto.NewInterestRate)
        //         throw new Exception($"New Interest Rate Should be greater than Minimum Interest Rate. Minimum Interest Rate in your condition is: {depositScheme.MinimumInterestRate}");
        //     else if(depositScheme.MaximumInterestRate < changeInterestRateByDepositSchemeDto.NewInterestRate)
        //         throw new Exception($"New Interest Rate Should be less than Maximum Interest Rate. Maximum Interest Rate in your condition is: {depositScheme.MaximumInterestRate}");
        //     else
        //         throw new Exception("Something Worng Happend");
        // }
    }
}