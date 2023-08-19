using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.ClientSetup;
using MicroFinance.Dtos.DepositSetup;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Enums;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Exceptions;
using MicroFinance.Helper;
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
        private readonly IConfiguration _config;
        private readonly INepaliCalendarFormat _nepaliCalendarFormat;
        private readonly ICommonExpression _commonExpression;

        public DepositSchemeService
        (
        ILogger<DepositSchemeDto> logger,
        IMapper mapper,
        IDepositSchemeRepository depositSchemeRepository,
        IMainLedgerRepository mainLedgerRepository,
        IClientRepository clientRepository,
        ICompanyProfileService companyProfileService,
        IEmployeeService employeeService,
        IConfiguration config,
        INepaliCalendarFormat nepaliCalendarFormat,
        ICommonExpression commonExpression
        )
        {
            _loggger = logger;
            _mapper = mapper;
            _depositSchemeRepository = depositSchemeRepository;
            _mainLedgerRepository = mainLedgerRepository;
            _clientRepo = clientRepository;
            _companyProfileService = companyProfileService;
            _employeeService = employeeService;
            _config = config;
            _nepaliCalendarFormat = nepaliCalendarFormat;
            _commonExpression = commonExpression;
        }

        private List<string> GetSubLedgerNameForDepositScheme(CreateDepositSchemeDto createDepositScheme)
        {
            var depositSubledgerName = createDepositScheme.DepositSubledger ?? createDepositScheme.SchemeName + " " + "Deposit";
            var interestSubledgerName = createDepositScheme.InterestSubledger ?? createDepositScheme.SchemeName + " " + "Interest";
            var taxSublegderName = createDepositScheme.TaxSubledger ?? createDepositScheme.SchemeName + " " + "Tax";

            return new List<string>() { depositSubledgerName, interestSubledgerName, taxSublegderName };
        }

        public async Task<ResponseDto> CreateDepositSchemeService(CreateDepositSchemeDto createDepositScheme, TokenDto decodedToken)
        {
            var depositSchemeWithSameName = await _depositSchemeRepository.GetDepositSchemeByName(createDepositScheme.SchemeName);
            var depositSchemeWithSameSymbol = await _depositSchemeRepository.GetDepositSchemeBySymbol(createDepositScheme.Symbol);
            if (depositSchemeWithSameName != null || depositSchemeWithSameSymbol != null)
                throw new Exception("Deposit Scheme with same name or symbol exist");

            var companyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            var depositScheme = _mapper.Map<DepositScheme>(createDepositScheme);
            List<string> subLedgerNamesForDepositScheme = GetSubLedgerNameForDepositScheme(createDepositScheme);
            // depositScheme.SchemeType = await _mainLedgerRepository.GetLedger((int)createDepositScheme.SchemeType);
            depositScheme.SchemeTypeId = (int)createDepositScheme.SchemeType;
            depositScheme.CreatedBy = decodedToken.UserName;
            depositScheme.CreatorId = decodedToken.UserId;
            depositScheme.BranchCode = decodedToken.BranchCode;
            depositScheme.RealWorldCreationDate = DateTime.Now;
            depositScheme.NepaliCreationDate = await _nepaliCalendarFormat.GetNepaliFormatDate(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            depositScheme.EnglishCreationDate = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(depositScheme.NepaliCreationDate);
            await _depositSchemeRepository.CreateDepositScheme(depositScheme, subLedgerNamesForDepositScheme);
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
            if (existingDepositScheme == null) throw new NotFoundExceptionHandler("No Data Found for requested deposit scheme");
            else if (!existingDepositScheme.IsActive && !updateDepositScheme.IsActive)
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
            existingDepositScheme.NepaliModificationDate = await _nepaliCalendarFormat.GetNepaliFormatDate(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            existingDepositScheme.EnglishModificationDate = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(existingDepositScheme.NepaliModificationDate);
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

        private async Task<DepositAccount> UploadSignatureImages(dynamic depositAccountDto, DepositAccount depositAccount)
        {
            ImageUploadService uploadService = new ImageUploadService(_config);
            List<string> listOfClientSignaturePhotoProperty = new List<string>() { nameof(DepositAccount.SignatureFileData), nameof(DepositAccount.SignatureFileName), nameof(DepositAccount.SignatureFileType) };
            depositAccount = await uploadService.UploadImage(depositAccount, depositAccountDto?.SignaturePhoto, listOfClientSignaturePhotoProperty);
            return depositAccount;
        }

        public async Task<ResponseDto> CreateDepositAccountService(CreateDepositAccountDto createDepositAccountDto, TokenDto decodedToken)
        {

            DepositAccount newDepositAccount = _mapper.Map<DepositAccount>(createDepositAccountDto);
            await VerifyClient(createDepositAccountDto, newDepositAccount, decodedToken);
            var depositScheme = await VerifyDepositScheme(createDepositAccountDto, newDepositAccount);
            await VerfiyPositingAccounts(createDepositAccountDto, newDepositAccount, decodedToken);
            await AddReferredByEmployeeInDepositAccount(createDepositAccountDto, newDepositAccount, decodedToken);
            await AddBasicDetailsInDepositAccount(newDepositAccount, depositScheme ,decodedToken);
            newDepositAccount = await UploadSignatureImages(createDepositAccountDto, newDepositAccount);
            if (createDepositAccountDto.AccountType == AccountTypeEnum.Joint && createDepositAccountDto?.JointClientIds?.Count >= 1)
                await VerifyJointClientId(createDepositAccountDto.JointClientIds, decodedToken);

            var depositAccountId = await _depositSchemeRepository.CreateDepositAccount(newDepositAccount, createDepositAccountDto);
            // if (depositAccountId >= 1)
            // {
            //     if (createDepositAccountDto.AccountType == AccountTypeEnum.Joint)
            //         await CreateJointAccountService(jointClients, newDepositAccount);
            //     return new ResponseDto()
            //     {
            //         Message = $"Successfully created '{newDepositAccount.AccountNumber}' account number",
            //         Status = true,
            //         StatusCode = "200"
            //     };
            // }
            // throw new Exception("Unable to Create Deposit Account");
            return new ResponseDto()
            {
                Message = $"Successfully created '{newDepositAccount.AccountNumber}' account number",
                Status = true,
                StatusCode = "200"
            };
        }

        // private async Task<string> GetUpdatedInterestPostingDate(UpdateDepositAccountDto updateDepositAccountDto, DepositAccount existingDepositAccount)
        // {
        //     string interestPostingDate = existingDepositAccount.NextInterestPostingDate;
        //     if(updateDepositAccountDto.Status!=existingDepositAccount.Status)
        //     {
        //         var activeCompanyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
        //         DateTime activeCompanyCalendarInEnglish = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish($"{activeCompanyCalendar.Year}-{activeCompanyCalendar.Month}-{activeCompanyCalendar.RunningDay}");
        //         DateTime nextInteretPostingDateInEnglish = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(existingDepositAccount.NextInterestPostingDate);
        //         if(nextInteretPostingDateInEnglish >=  )
        //     }
        // }


        public async Task<ResponseDto> UpdateNonClosedDepositAccountService(UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken)
        {
            Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount =
            depositAcc => depositAcc.Id == updateDepositAccountDto.Id && depositAcc.Status != AccountStatusEnum.Close;
            var existingDepositAccount = await _depositSchemeRepository.GetDepositAccount(expressionToQueryDepositAccount);
            if (existingDepositAccount == null)
                throw new Exception("No Non-close deposit account found");

            if (decodedToken.Role != RoleEnum.Officer.ToString() && existingDepositAccount.BranchCode != decodedToken.BranchCode)
                throw new Exception("You are authorized to update other branch details");
            if (updateDepositAccountDto.InterestRate < existingDepositAccount.DepositScheme.MinimumInterestRate && updateDepositAccountDto.InterestRate > existingDepositAccount.DepositScheme.MaximumInterestRate)
                throw new Exception("MinimumInterestRate<=InterestRate<=MaximumInterestRate  constraint doesnot match");
            await UpdateDepositAccount(existingDepositAccount, updateDepositAccountDto, decodedToken);
            var updateStatus = await _depositSchemeRepository.UpdateDepositAccount(existingDepositAccount);
            if (updateStatus >= 1)
            {
                return new ResponseDto()
                {
                    Message = "Successfully updated deposit account",
                    Status = true,
                    StatusCode = "200"
                };
            }
            throw new Exception("Update Failed");
        }
        public async Task<DepositAccountDto> GetDepositAccount(Expression<Func<DepositAccount, bool>> expression)
        {
            var depositAccount = await _depositSchemeRepository.GetDepositAccount(expression);
            if (depositAccount == null)
                throw new Exception("No deposit Account found");
            return _mapper.Map<DepositAccountDto>(depositAccount);
        }
        public async Task<MatureDateDto> GenerateMatureDateOfDepositAccountService(GenerateMatureDateDto generateMatureDateDto)
        {
            DateTime openingDateInEnglish = await VerifyNepaliDateAndConvertToEnglishDate(generateMatureDateDto.OpeningDate);
            DateTime maturePeriodInEnglishFormat;
            if (generateMatureDateDto.PeriodType == PeriodTypeEnum.Year)
                maturePeriodInEnglishFormat = openingDateInEnglish.AddYears(generateMatureDateDto.Period).AddDays(-1);

            else if (generateMatureDateDto.PeriodType == PeriodTypeEnum.Month)
                maturePeriodInEnglishFormat = openingDateInEnglish.AddMonths(generateMatureDateDto.Period).AddDays(-1);

            else
                maturePeriodInEnglishFormat = openingDateInEnglish.AddDays(generateMatureDateDto.Period).AddDays(-1);
            string nepaliMatureDate = await _nepaliCalendarFormat.ConvertEnglishDateToNepali(maturePeriodInEnglishFormat);
            return new MatureDateDto()
            {
                NepaliMatureDate = nepaliMatureDate,
                EnglishMatureDate = maturePeriodInEnglishFormat
            };


            // var openingDate = (generateMatureDateDto.OpeningDate).Split("/");
            // ReceivedCalendarDto receivedCalendarDto = new ReceivedCalendarDto()
            // {
            //     CurrentYear=Int32.Parse(openingDate[0]),
            //     CurrentMonth = Int32.Parse(openingDate[1]),
            //     CurrentDay = Int32.Parse(openingDate[2])
            // };
            // if
            // (
            //     receivedCalendarDto.CurrentYear.ToString().Length<4 
            //     || receivedCalendarDto.CurrentMonth<1 
            //     || receivedCalendarDto.CurrentMonth>12 
            //     || receivedCalendarDto.CurrentDay<1 
            //     || receivedCalendarDto.CurrentDay>32
            // ) throw new Exception("Invalid Opening Date. Please refer format YYYY/MM/DD as 2080/01/01");
            // var matureDate = "";
            // if(generateMatureDateDto.PeriodType == PeriodTypeEnum.Year)
            //     matureDate = await GenerateMatureDateYearWise(generateMatureDateDto, receivedCalendarDto);
            // else if(generateMatureDateDto.PeriodType==PeriodTypeEnum.Month)
            //     matureDate = await GenerateMatureDateMonthWise(generateMatureDateDto, receivedCalendarDto);
            // else
            //     matureDate = await GenerateMatureDateDayWise(generateMatureDateDto, receivedCalendarDto);
            // if(string.IsNullOrEmpty(matureDate)) throw new Exception("Not able to Generate Mature Date");
            // return matureDate;
        }

        public async Task<List<DepositAccountWrapperDto>> GetAllDepositAccountWrapperService(TokenDto decodedToken)
        {
            Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount = depositAcc => depositAcc.Id != null;
            var allNonClosedDepositAccounts = await _depositSchemeRepository.GetAllDepositAccountsWrapper(expressionToQueryDepositAccount);
            if (allNonClosedDepositAccounts == null || allNonClosedDepositAccounts.Count < 1)
                return new List<DepositAccountWrapperDto>();
            List<DepositAccountWrapperDto> allDepositAccountWrapperDto = new List<DepositAccountWrapperDto>();
            foreach (var depositAccountWrapper in allNonClosedDepositAccounts)
            {
                if (decodedToken.Role == RoleEnum.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                {
                    var depositAccountWrapperDto = await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
                    allDepositAccountWrapperDto.Add(depositAccountWrapperDto);
                }
            }
            return allDepositAccountWrapperDto;
        }



        public async Task<DepositAccountWrapperDto> GetDepositAccountWrapperByIdService(int? depositAccountId, Expression<Func<DepositAccount, bool>>? expression, TokenDto decodedToken)
        {
            Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount;
            if (expression == null)
                expressionToQueryDepositAccount = depositAcc => depositAcc.Id == depositAccountId;
            else
                expressionToQueryDepositAccount = expression;
            var depositAccountWrapper = await _depositSchemeRepository.GetDepositAccountWrapper(expressionToQueryDepositAccount);
            if (depositAccountWrapper != null && depositAccountWrapper.DepositAccount != null)
            {
                if (decodedToken.Role == RoleEnum.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                    return await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
            }
            throw new Exception("No data found");
        }

        public async Task<DepositAccountWrapperDto> GetDepositAccountWrapperByAccountNumberService(string? accountNumber, Expression<Func<DepositAccount, bool>>? expression, TokenDto decodedToken)
        {
            Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount;
            if (expression == null)
                expressionToQueryDepositAccount = depositAcc => depositAcc.AccountNumber == accountNumber;
            else
                expressionToQueryDepositAccount = expression;
            //Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount = depositAcc => depositAcc.AccountNumber==accountNumber;
            var depositAccountWrapper = await _depositSchemeRepository.GetDepositAccountWrapper(expressionToQueryDepositAccount);
            if (depositAccountWrapper != null && depositAccountWrapper.DepositAccount != null)
            {
                if (decodedToken.Role == RoleEnum.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                    return await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper);
            }
            throw new Exception("No data found");

        }
        public async Task<List<DepositAccountWrapperDto>> GetDepositAccountWrapperByDepositSchemeService(int depositSchemeId, TokenDto decodedToken)
        {
            Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount = depositAcc => depositAcc.DepositSchemeId == depositSchemeId;
            var depositAccountWrappers = await _depositSchemeRepository.GetAllDepositAccountsWrapper(expressionToQueryDepositAccount);
            List<DepositAccountWrapperDto> depositAccountWrappersDto = new List<DepositAccountWrapperDto>();
            if (depositAccountWrappers != null && depositAccountWrappers.Count >= 1)
            {
                foreach (var depositAccountWrapper in depositAccountWrappers)
                {
                    if (decodedToken.Role == RoleEnum.Officer.ToString() || depositAccountWrapper.DepositAccount.BranchCode == decodedToken.BranchCode)
                    {
                        depositAccountWrappersDto.Add(await MapDepositAccountWrapperToDepositAccountWrapperDto(depositAccountWrapper));
                    }
                }
            }
            return depositAccountWrappersDto;
        }

        private async Task VerifyClient(CreateDepositAccountDto createDepositAccountDto, DepositAccount depositAccount, TokenDto decodedToken)
        {
            var client = await _clientRepo.GetClientById(createDepositAccountDto.ClientId);
            if (client == null || !client.IsActive)
            {
                throw new Exception
                ($"UnAuthorized to proceed. Client '{client?.ClientFirstName}' has active status '{client?.IsActive}'.");
            }
            if (client.BranchCode != decodedToken.BranchCode)
                throw new Exception("Given Client is not found under your branch");
            depositAccount.ClientId = client.Id;
        }

        private async Task<DepositScheme> VerifyDepositScheme(CreateDepositAccountDto createDepositAccountDto, DepositAccount depositAccount)
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
            depositAccount.DepositSchemeId = depositScheme.Id;
            return depositScheme;
        }

        private async Task VerfiyPositingAccounts(CreateDepositAccountDto createDepositAccountDto, DepositAccount newDepositAccount, TokenDto decodedToken)
        {
            if (createDepositAccountDto.InterestPostingAccountId != null)
            {
                Expression<Func<DepositAccount, bool>> expressionQuery
                = da => da.Id == (int)createDepositAccountDto.InterestPostingAccountId && da.Status != AccountStatusEnum.Close &&
                da.Client.IsActive && da.DepositScheme.IsActive && da.BranchCode == decodedToken.BranchCode;

                var interestPostingAccountNumber = await _depositSchemeRepository.GetDepositAccount(expressionQuery);
                if (interestPostingAccountNumber == null)
                    throw new Exception("InterestPostingAccountNumber: Cannot Find Account Number under your branch");
                newDepositAccount.InterestPostingAccountNumberId = interestPostingAccountNumber.Id;
            }
            if (createDepositAccountDto.MatureInterestPostingAccountId != null)
            {
                Expression<Func<DepositAccount, bool>> expressionQuery
                = da => da.Id == (int)createDepositAccountDto.MatureInterestPostingAccountId && da.Status != AccountStatusEnum.Close &&
                da.Client.IsActive && da.DepositScheme.IsActive && da.BranchCode == decodedToken.BranchCode;

                var matureInterestPostingAccountNumber = await _depositSchemeRepository.GetDepositAccount(expressionQuery);
                if (matureInterestPostingAccountNumber == null)
                    throw new Exception("MatureInterestPositingAccountNumber: Cannot Find Account Number under your branch");
                newDepositAccount.MatureInterestPostingAccountNumberId = matureInterestPostingAccountNumber.Id;
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

        private async Task VerifyJointClientId(List<int> jointClientIds, TokenDto decodedToken)
        {
            foreach (var jointClientId in jointClientIds)
            {
                var jointClient = await _clientRepo.GetClientById(jointClientId);
                if (jointClient == null || jointClient.BranchCode != decodedToken.BranchCode)
                    throw new Exception($"Provided Joint Clients are not found under your branch. Id:{jointClientId}");
                if (!jointClient.IsActive)
                    throw new Exception($"Joint Client {jointClient.ClientId} is inactive");
            }
        }
        private async Task<DateTime> VerifyNepaliDateAndConvertToEnglishDate(string nepaliOpeningDate)
        {
            bool isOpeningDateFormatCorrect = await _nepaliCalendarFormat.VerifyNepaliDateFormat(nepaliOpeningDate);
            if (!isOpeningDateFormatCorrect)
                throw new BadRequestExceptionHandler("Invalid Opening Date Format. Correct Format is YYYY-MM-DD");
            return await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(nepaliOpeningDate);
        }
        private async Task AddBasicDetailsInDepositAccount(DepositAccount newDepositAccount, DepositScheme depositScheme ,TokenDto decodedToken)
        {
            var companyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            newDepositAccount.RealWorldCreationDate = DateTime.Now;
            newDepositAccount.NepaliCreationDate = await _nepaliCalendarFormat.GetNepaliFormatDate(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            newDepositAccount.EnglishCreationDate = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(newDepositAccount.NepaliCreationDate);
            newDepositAccount.BranchCode = decodedToken.BranchCode;
            newDepositAccount.CreatedBy = decodedToken.UserName;
            newDepositAccount.CreatorId = decodedToken.UserId;
            string nepaliOpeningDate = await _nepaliCalendarFormat.GetNepaliFormatDate(newDepositAccount.NepaliOpeningDate);
            if (string.IsNullOrEmpty(nepaliOpeningDate))
                throw new BadRequestExceptionHandler("Invalid Opening Date. Format should be YYYY-MM-DD. And also please enter correct date");
            newDepositAccount.NepaliCreationDate = nepaliOpeningDate;
            newDepositAccount.EnglishOpeningDate = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(newDepositAccount.NepaliOpeningDate);
            //newDepositAccount.OpeningDate 
            GenerateMatureDateDto generateMatureDateDto = new GenerateMatureDateDto()
            {
                OpeningDate = newDepositAccount.NepaliOpeningDate,
                Period = newDepositAccount.Period,
                PeriodType = newDepositAccount.PeriodType
            };
            MatureDateDto matureDate = await GenerateMatureDateOfDepositAccountService(generateMatureDateDto);
            newDepositAccount.NepaliMatureDate = matureDate.NepaliMatureDate;
            newDepositAccount.EnglishMatureDate = matureDate.EnglishMatureDate;
            PostingSchemeEnum postingScheme = (PostingSchemeEnum)Enum.ToObject(typeof(PostingSchemeEnum), depositScheme.PostingScheme);
            newDepositAccount.NextInterestPostingDate = await GenerateNextInterestPostingDate(newDepositAccount.EnglishOpeningDate, postingScheme, newDepositAccount.EnglishMatureDate);
        }
        public async Task<DateTime> GenerateNextInterestPostingDate(DateTime englishCurrentDate, PostingSchemeEnum postingScheme, DateTime englishMatureDate)
        {
            DateTime nextPostingDateinEnglish;

            if (postingScheme == PostingSchemeEnum.Yearly)
                nextPostingDateinEnglish = englishCurrentDate.AddYears(1).AddDays(-1);
            else if (postingScheme == PostingSchemeEnum.HalfYearly)
                nextPostingDateinEnglish = englishCurrentDate.AddMonths(6).AddDays(-1);
            else if (postingScheme == PostingSchemeEnum.Quarterly)
                nextPostingDateinEnglish = englishCurrentDate.AddMonths(3).AddDays(-1);
            else if (postingScheme == PostingSchemeEnum.Monthly)
            {
                nextPostingDateinEnglish = englishCurrentDate.AddMonths(1).AddDays(-1);
                string nepaliDateOfPosting = await _nepaliCalendarFormat.ConvertEnglishDateToNepali(nextPostingDateinEnglish);
                var splitedDate = nepaliDateOfPosting.Split("-");
                int activeYear = await _companyProfileService.GetActiveYearService();
                var calendar = await _companyProfileService.GetCalendarByYearAndMonthService(activeYear, int.Parse(splitedDate[1]));
                try
                {
                    nextPostingDateinEnglish = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish($"{splitedDate[0]}-{splitedDate[1]}-{calendar.NumberOfDay - 1}");
                }
                catch (Exception ex)
                {
                    nextPostingDateinEnglish = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish($"{splitedDate[0]}-{splitedDate[1]}-{calendar.NumberOfDay - 2}");
                }
            }

            else
                nextPostingDateinEnglish = englishMatureDate;
            return nextPostingDateinEnglish;
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
            if (receivedCalendarDto.CurrentMonth == 1)
                matureYear -= 1;
            int matureMonth = receivedCalendarDto.CurrentMonth;
            int matureDay = receivedCalendarDto.CurrentDay - 1;
            if (receivedCalendarDto.CurrentDay == 1)
            {
                if (receivedCalendarDto.CurrentMonth == 1)
                    matureMonth = 12;
                else
                    matureMonth -= 1;
                var activeYearCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
                matureDay = (await _companyProfileService.GetCalendarByYearAndMonthService(activeYearCalendar.Year, matureMonth)).NumberOfDay;
            }
            return $"{matureYear}/{matureMonth}/{matureDay}";
        }
        private async Task<string> GenerateMatureDateMonthWise(GenerateMatureDateDto generateMatureDateDto, ReceivedCalendarDto receivedCalendarDto)
        {
            int matureYear = receivedCalendarDto.CurrentYear + (receivedCalendarDto.CurrentMonth + generateMatureDateDto.Period) / 12;
            int matureMonth = (receivedCalendarDto.CurrentMonth + generateMatureDateDto.Period) % 12;
            int matureDay = receivedCalendarDto.CurrentDay - 1;
            if (generateMatureDateDto.Period <= 12 && matureMonth > 12)
            {
                matureYear += 1;
                matureMonth -= 12;
            }
            if (receivedCalendarDto.CurrentDay == 1)
            {
                matureMonth -= 1;
                var activeYearCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
                matureDay = (await _companyProfileService.GetCalendarByYearAndMonthService(activeYearCalendar.Year, matureMonth)).NumberOfDay;
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
            int matureDay = receivedCalendarDto.CurrentDay + extraDay - 1;
            if (matureDay > 30)
            {
                matureMonth += matureDay / 30;
                matureDay %= 30;
            }
            if (matureMonth > 12)
            {
                matureYear += matureMonth / 12;
                matureMonth %= 12;
            }
            var activeYearCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            int totalNumberOfDaysInCurrentMonth = (await _companyProfileService.GetCalendarByYearAndMonthService(activeYearCalendar.Year, matureMonth)).NumberOfDay;
            if (matureDay > totalNumberOfDaysInCurrentMonth)
                matureDay = totalNumberOfDaysInCurrentMonth;

            return $"{matureYear}/{matureMonth}/{matureDay}";
        }


        // UPDATE DEPOSIT ACCOUNT
        private async Task UpdateBasicInformationOfDepositAccount(DepositAccount existingDepositAccount, UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken)
        {
            var currentCompanyActiveCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            existingDepositAccount.ModifierBranchCode = decodedToken.BranchCode;
            existingDepositAccount.ModifiedBy = decodedToken.UserName;
            existingDepositAccount.ModifierId = decodedToken.UserId;
            existingDepositAccount.NepaliModificationDate = await _nepaliCalendarFormat.GetNepaliFormatDate(currentCompanyActiveCalendar.Year, currentCompanyActiveCalendar.Month, currentCompanyActiveCalendar.RunningDay);
            existingDepositAccount.EnglishModificationDate = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(existingDepositAccount.NepaliModificationDate);
            existingDepositAccount.RealWorldModificationDate = DateTime.Now;
        }

        private async Task UpdateDepositAccount(DepositAccount existingDepositAccount, UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken)
        {
            existingDepositAccount.InterestRate = updateDepositAccountDto.InterestRate;
            existingDepositAccount.Description = updateDepositAccountDto.Description;
            existingDepositAccount.NomineeName = updateDepositAccountDto.NomineeName;
            existingDepositAccount.Relation = updateDepositAccountDto.Relation;
            existingDepositAccount.IsSMSServiceActive = updateDepositAccountDto.IsSMSServiceActive;
            existingDepositAccount.ExpectedDailyDepositAmount = updateDepositAccountDto.ExpectedDailyDepositAmount;
            existingDepositAccount.ExpectedTotalDepositAmount = updateDepositAccountDto.ExpectedTotalDepositAmount;
            existingDepositAccount.ExpectedTotalDepositDay = updateDepositAccountDto.ExpectedTotalDepositDay;
            existingDepositAccount.ExpectedTotalInterestAmount = updateDepositAccountDto.ExpectedTotalInterestAmount;
            existingDepositAccount.ExpectedTotalReturnAmount = updateDepositAccountDto.ExpectedTotalReturnAmount;

            await ImageUpdateInDepositAccount(existingDepositAccount, updateDepositAccountDto);
            await InterestPostingAccountUpdateInDepositAccount(existingDepositAccount, updateDepositAccountDto, decodedToken);
            await MatureInterestPostingAccountInDepositAccount(existingDepositAccount, updateDepositAccountDto, decodedToken);
            await UpdateBasicInformationOfDepositAccount(existingDepositAccount, updateDepositAccountDto, decodedToken);
        }

        private async Task ImageUpdateInDepositAccount(DepositAccount existingDepositAccount, UpdateDepositAccountDto updateDepositAccountDto)
        {
            if (updateDepositAccountDto.IsSignatureChanged && updateDepositAccountDto.SignaturePhoto != null)
            {
                existingDepositAccount = await UploadSignatureImages(updateDepositAccountDto, existingDepositAccount);
            }
            else if (updateDepositAccountDto.IsSignatureChanged && updateDepositAccountDto.SignaturePhoto == null)
            {
                existingDepositAccount.SignatureFileData = null;
                existingDepositAccount.SignatureFileName = null;
                existingDepositAccount.SignatureFileType = null;
            }
        }
        private async Task InterestPostingAccountUpdateInDepositAccount(DepositAccount existingDepositAccount, UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken)
        {
            if (updateDepositAccountDto.InterestPostingAccountId != null && !updateDepositAccountDto.InterestPostingAccountId.Equals(existingDepositAccount?.InterestPostingAccountNumberId))
            {
                Expression<Func<DepositAccount, bool>> expression =
                depositAcc => depositAcc.Id == (int)updateDepositAccountDto.InterestPostingAccountId && depositAcc.Status != AccountStatusEnum.Close;
                var interestPostingAccount = await _depositSchemeRepository.GetDepositAccount(expression);
                existingDepositAccount.InterestPostingAccountNumberId = (interestPostingAccount != null && interestPostingAccount.BranchCode == decodedToken.BranchCode) ? interestPostingAccount.Id : throw new Exception("No Account Found for Interest Posting");

            }
            else if (updateDepositAccountDto.InterestPostingAccountId == null)
                existingDepositAccount.InterestPostingAccountNumberId = null;
        }

        private async Task MatureInterestPostingAccountInDepositAccount(DepositAccount existingDepositAccount, UpdateDepositAccountDto updateDepositAccountDto, TokenDto decodedToken)
        {
            if (updateDepositAccountDto.MatureInterestPostingAccountId != null && !updateDepositAccountDto.MatureInterestPostingAccountId.Equals(existingDepositAccount?.MatureInterestPostingAccountNumberId))
            {
                Expression<Func<DepositAccount, bool>> expression =
                depositAcc => depositAcc.Id == (int)updateDepositAccountDto.MatureInterestPostingAccountId && depositAcc.Status != AccountStatusEnum.Close;
                var maturePostingAccount = await _depositSchemeRepository.GetDepositAccount(expression);
                existingDepositAccount.MatureInterestPostingAccountNumberId = (maturePostingAccount != null && maturePostingAccount.BranchCode == decodedToken.BranchCode) ? maturePostingAccount.Id : throw new Exception("No Account Found for mature interest posting");
            }
            else if (updateDepositAccountDto.MatureInterestPostingAccountId == null)
                existingDepositAccount.MatureInterestPostingAccountNumberId = null;
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