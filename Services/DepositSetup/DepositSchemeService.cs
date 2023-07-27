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
using MicroFinance.Role;
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
            depositScheme.CompanyCalendarCreationDate = $"{companyCalendar.Year}/{companyCalendar.Month}/{companyCalendar.RunningDay}";
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
            existingDepositScheme.CompanyCalendarModificationDate = $"{companyCalendar.Year}/{companyCalendar.Month}/{companyCalendar.RunningDay}";
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

        // DEPOSIT ACCOUNT

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

       
        
        public async Task<ResponseDto> UpdateNonClosedDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken)
        {
            var existingDepositAccount = await _depositSchemeRepository.GetNonClosedDepositAccountById(updateDepositAccountDto.Id);
            if(existingDepositAccount==null)
                throw new Exception("No Data Found");
            if(decodedToken.Role!=UserRole.Officer.ToString() && existingDepositAccount.BranchCode!=decodedToken.BranchCode)
                throw new Exception("You are authorized to update other branch details");
            if(existingDepositAccount.Status==AccountStatusEnum.Close)
                throw new Exception("You are not authorized to update the closed account");
            if(updateDepositAccountDto.InterestRate<existingDepositAccount.DepositScheme.MinimumInterestRate && updateDepositAccountDto.InterestRate>existingDepositAccount.DepositScheme.MaximumInterestRate)
                throw new Exception("MinimumInterestRate<=InterestRate<=MaximumInterestRate  constraint doesnot match");
            existingDepositAccount.InterestRate = updateDepositAccountDto.InterestRate;
            existingDepositAccount.Status = updateDepositAccountDto.Status;
            existingDepositAccount.Description = updateDepositAccountDto.Description;
            existingDepositAccount.NomineeName = updateDepositAccountDto.NomineeName;
            existingDepositAccount.Relation = updateDepositAccountDto.Relation;
            existingDepositAccount.IsSMSServiceActive = updateDepositAccountDto.IsSMSServiceActive;
            existingDepositAccount.ExpectedDailyDepositAmount = updateDepositAccountDto.ExpectedDailyDepositAmount;
            existingDepositAccount.ExpectedTotalDepositAmount = updateDepositAccountDto.ExpectedTotalDepositAmount;
            existingDepositAccount.ExpectedTotalDepositDay = updateDepositAccountDto.ExpectedTotalDepositDay;
            existingDepositAccount.ExpectedTotalInterestAmount = updateDepositAccountDto.ExpectedTotalInterestAmount;
            existingDepositAccount.ExpectedTotalReturnAmount = updateDepositAccountDto.ExpectedTotalReturnAmount;
            if(updateDepositAccountDto.InterestPostingAccountId!=null && !updateDepositAccountDto.InterestPostingAccountId.Equals(existingDepositAccount?.InterestPostingAccountNumberId))
            {
                var interestPostingAccount = await _depositSchemeRepository.GetNonClosedDepositAccountById((int) updateDepositAccountDto.InterestPostingAccountId);
                existingDepositAccount.InterestPostingAccountNumber = (interestPostingAccount!=null && interestPostingAccount.BranchCode==decodedToken.BranchCode)?interestPostingAccount:throw new Exception("No Account Found for Interest Posting");
            }
            else if(updateDepositAccountDto.InterestPostingAccountId==null)
                existingDepositAccount.InterestPostingAccountNumber = null;
            if(updateDepositAccountDto.MatureInterestPostingAccountId!=null && !updateDepositAccountDto.MatureInterestPostingAccountId.Equals(existingDepositAccount?.MatureInterestPostingAccountNumberId))
            {
                var maturePostingAccount = await _depositSchemeRepository.GetNonClosedDepositAccountById((int) updateDepositAccountDto.MatureInterestPostingAccountId);
                existingDepositAccount.MatureInterestPostingAccountNumber=(maturePostingAccount!=null && maturePostingAccount.BranchCode==decodedToken.BranchCode)?maturePostingAccount:throw new Exception("No Account Found for mature interest posting");
            }
            else if(updateDepositAccountDto.MatureInterestPostingAccountId==null)
                existingDepositAccount.MatureInterestPostingAccountNumber=null;
            var updateStatus = await _depositSchemeRepository.UpdateDepositAccount(existingDepositAccount);
            if(updateStatus>=1)
            {
                return new ResponseDto()
                {
                    Message="Successfully updated deposit account",
                    Status=true,
                    StatusCode="200"
                };
            }
            throw new Exception("Update Failed");
        }

        public async Task<string> GenerateMatureDateOfDepositAccountService(GenerateMatureDateDto generateMatureDateDto)
        {
            var openingDate = (generateMatureDateDto.OpeningDate).Split("/");
            ReceivedCalendarDto receivedCalendarDto = new ReceivedCalendarDto()
            {
                CurrentYear=Int32.Parse(openingDate[0]),
                CurrentMonth = Int32.Parse(openingDate[1]),
                CurrentDay = Int32.Parse(openingDate[2])
            };
            if
            (
                receivedCalendarDto.CurrentYear.ToString().Length<4 
                || receivedCalendarDto.CurrentMonth<1 
                || receivedCalendarDto.CurrentMonth>12 
                || receivedCalendarDto.CurrentDay<1 
                || receivedCalendarDto.CurrentDay>32
            ) throw new Exception("Invalid Opening Date. Please refer formar YYYY/MM/DD as 2080/01/01");
            var matureDate = "";
            if(generateMatureDateDto.PeriodType == PeriodTypeEnum.Year)
                matureDate = await GenerateMatureDateYearWise(generateMatureDateDto, receivedCalendarDto);
            else if(generateMatureDateDto.PeriodType==PeriodTypeEnum.Month)
                matureDate = await GenerateMatureDateMonthWise(generateMatureDateDto, receivedCalendarDto);
            else
                matureDate = await GenerateMatureDateDayWise(generateMatureDateDto, receivedCalendarDto);
            if(string.IsNullOrEmpty(matureDate)) throw new Exception("Not able to Generate Mature Date");
            return matureDate;
        }

        public async Task<List<DepositAccountWrapperDto>> GetAllNonClosedDepositAccountService(TokenDto decodedToken)
        {
            var allNonClosedDepositAccounts = await _depositSchemeRepository.GetAllNonClosedDepositAccounts();
            if (allNonClosedDepositAccounts == null || allNonClosedDepositAccounts.Count < 1)
                return new List<DepositAccountWrapperDto>();
            List<DepositAccountWrapperDto> allDepositAccountWrapperDto = new List<DepositAccountWrapperDto>();
            foreach (var depositAccountWrapper in allNonClosedDepositAccounts)
            {
                if (decodedToken.Role==UserRole.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                {
                    var depositAccountWrapperDto = await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
                    allDepositAccountWrapperDto.Add(depositAccountWrapperDto);
                }
            }
            return allDepositAccountWrapperDto;
        }

        public async Task<DepositAccountWrapperDto> GetNonClosedDepositAccountByIdService(int depositAccountId, TokenDto decodedToken)
        {
            var depositAccountWrapper = await _depositSchemeRepository.GetNonClosedDepositAccount(depositAccountId);
            if (depositAccountWrapper != null && depositAccountWrapper.DepositAccount != null)
            {
                if (decodedToken.Role==UserRole.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                    return await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
            }

            throw new Exception("No data found");
        }

        public async Task<DepositAccountWrapperDto> GetNonClosedDepositAccountByAccountNumberService(string accountNumber, TokenDto decodedToken)
        {
            var depositAccountWrapper = await _depositSchemeRepository.GetNonClosedDepositAccountByAccountNumber(accountNumber);
            if (depositAccountWrapper != null && depositAccountWrapper.DepositAccount != null)
            {
                if (decodedToken.Role==UserRole.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                    return await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
            }
            throw new Exception("No data found");
            
        }
        public async Task<List<DepositAccountWrapperDto>> GetNonClosedDepositAccountByDepositSchemeService(int depositSchemeId, TokenDto decodedToken)
        {
            var depositAccountWrappers = await _depositSchemeRepository.GetNonClosedDepositAccountByDepositScheme(depositSchemeId);
            List<DepositAccountWrapperDto> depositAccountWrappersDto = new List<DepositAccountWrapperDto>();
            if(depositAccountWrappers!=null && depositAccountWrappers.Count>=1)
            {
                foreach (var depositAccountWrapper in depositAccountWrappers)
                {
                    if(decodedToken.Role==UserRole.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode==decodedToken.BranchCode)
                    {
                        depositAccountWrappersDto.Add(await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper));
                    }
                }
            }
            return depositAccountWrappersDto;
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
                var interestPostingAccountNumber = await _depositSchemeRepository.GetNonClosedDepositAccountById((int)createDepositAccountDto.InterestPostingAccountId);
                if (interestPostingAccountNumber == null || interestPostingAccountNumber.BranchCode != decodedToken.BranchCode)
                    throw new Exception("InterestPostingAccountNumber: Cannot Find Account Number under your branch");
                newDepositAccount.InterestPostingAccountNumber = interestPostingAccountNumber;
            }
            if (createDepositAccountDto.MatureInterestPostingAccountId != null)
            {
                var matureInterestPostingAccountNumber = await _depositSchemeRepository.GetNonClosedDepositAccountById((int)createDepositAccountDto.MatureInterestPostingAccountId);
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
            newDepositAccount.CompanyCalendarCreationDate =$"{companyCalendar.Year}/{companyCalendar.Month}/{companyCalendar.RunningDay}";
            newDepositAccount.BranchCode = decodedToken.BranchCode;
            newDepositAccount.CreatedBy = decodedToken.UserName;
            newDepositAccount.CreatorId = decodedToken.UserId;
            //newDepositAccount.OpeningDate 
            GenerateMatureDateDto generateMatureDateDto = new GenerateMatureDateDto()
            {
                OpeningDate = newDepositAccount.OpeningDate,
                Period = newDepositAccount.Period,
                PeriodType = newDepositAccount.PeriodType
            };
            newDepositAccount.MatureDate = await GenerateMatureDateOfDepositAccountService(generateMatureDateDto);
        }

        private async Task CreateJointAccountService(List<Client> jointClients, DepositAccount depositAccount)
        {
            List<JointAccount> jointAccounts = new List<JointAccount>();
            DateTime RealWorldStartDate = DateTime.Now;
            string CompanyCalendarStartDate = depositAccount.CompanyCalendarCreationDate;
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

         private async Task<string> GenerateMatureDateYearWise(GenerateMatureDateDto generateMatureDateDto, ReceivedCalendarDto receivedCalendarDto)
        {
            int matureYear = receivedCalendarDto.CurrentYear + generateMatureDateDto.Period;
            if(receivedCalendarDto.CurrentMonth==1)
                matureYear-=1;
            int matureMonth = receivedCalendarDto.CurrentMonth;
            int matureDay = receivedCalendarDto.CurrentDay-1;
            if(receivedCalendarDto.CurrentDay==1)
            {
                if(receivedCalendarDto.CurrentMonth==1)
                    matureMonth = 12;
                else
                    matureMonth-=1;
                matureDay = (await _companyProfileService.GetCalendarByYearAndMonthService(receivedCalendarDto.CurrentYear, matureMonth)).NumberOfDay;
            }
            return $"{matureYear}/{matureMonth}/{matureDay}";
        }
        private async Task<string> GenerateMatureDateMonthWise(GenerateMatureDateDto generateMatureDateDto, ReceivedCalendarDto receivedCalendarDto)
        {
            int matureYear = receivedCalendarDto.CurrentYear + (receivedCalendarDto.CurrentMonth + generateMatureDateDto.Period) / 12;
            int matureMonth = (receivedCalendarDto.CurrentMonth + generateMatureDateDto.Period) % 12;
            int matureDay = receivedCalendarDto.CurrentDay -1;
            if(generateMatureDateDto.Period<=12 && matureMonth>12)
            {
                matureYear+=1;
                matureMonth-=12;
            }
            if(receivedCalendarDto.CurrentDay==1)
            {
                matureMonth-=1;
                matureDay = (await _companyProfileService.GetCalendarByYearAndMonthService(receivedCalendarDto.CurrentYear, matureMonth)).NumberOfDay;
            }
            return $"{matureYear}/{matureMonth}/{matureDay}";
        }
        private async Task<string> GenerateMatureDateDayWise(GenerateMatureDateDto generateMatureDateDto, ReceivedCalendarDto receivedCalendarDto)
        {
            int extraYear = generateMatureDateDto.Period / 365;
            int remainingDays = generateMatureDateDto.Period % 365;
            int extraMonth = remainingDays / 30;
            int extraDay = remainingDays % 30;
            int matureYear = receivedCalendarDto.CurrentYear + extraYear;
            int matureMonth = receivedCalendarDto.CurrentMonth + extraMonth;
            int matureDay = receivedCalendarDto.CurrentDay + extraDay -1;
            if(matureDay>30)
            {
                matureMonth+=matureDay / 30;
                matureDay %= 30;
            }
            if(matureMonth > 12)
            {
                matureYear +=matureMonth / 12;
                matureMonth %= 12;
            }
            int totalNumberOfDaysInCurrentMonth = (await _companyProfileService.GetCalendarByYearAndMonthService(receivedCalendarDto.CurrentYear, matureMonth)).NumberOfDay;
            if(matureDay>totalNumberOfDaysInCurrentMonth)
                matureDay = totalNumberOfDaysInCurrentMonth;
            
            return $"{matureYear}/{matureMonth}/{matureDay}";
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