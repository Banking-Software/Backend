using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.DepositSetup.Account;
using MicroFinance.Dtos.LoanSetup;
using MicroFinance.Helpers;
using MicroFinance.Models.LoanSetup;
using MicroFinance.Repository.LoanSetup;
using MicroFinance.Services.CompanyProfile;

namespace MicroFinance.Services.LoanSetup
{
    public class LoanSetupServices : ILoanSetupServices
    {
        private ILoanSetupRepository _loanSetupRepository;
        private readonly IHelper _helper;
        private readonly IMapper _mapper;
        private readonly ICompanyProfileService _companyProfileService;
        private readonly IConfiguration _config;

        public LoanSetupServices
        (
            ILoanSetupRepository loanSetupRepository,
            ICompanyProfileService companyProfileService, 
            IHelper helper, 
            IMapper mapper,
            IConfiguration config
        )
        {
            _loanSetupRepository=loanSetupRepository;
            _helper = helper;
            _mapper = mapper;
            _companyProfileService=companyProfileService;
            _config = config;
        }

        private async Task AddBasicInfoForLoanScheme(LoanScheme loanScheme, TokenDto decodedToken)
        {
            var activeCompanyDate = await _companyProfileService.GetCurrentActiveCalenderService();
            loanScheme.CreatedBy = decodedToken.UserName;
            loanScheme.CreatorId = decodedToken.UserId;
            loanScheme.BranchCode = decodedToken.BranchCode;
            loanScheme.EnglishCreationDate = await _helper.GetCompanyActiveCalendarInAD(activeCompanyDate);
            loanScheme.NepaliCreationDate = await _helper.ConvertEnglishDateToNepali(loanScheme.EnglishCreationDate);
        }
        private async Task AddBasicInfoForLoanAccount(LoanAccount loanAccount, TokenDto decodedToken)
        {
            var activeCompanyDate = await _companyProfileService.GetCurrentActiveCalenderService();
            loanAccount.CreatedBy = decodedToken.UserName;
            loanAccount.CreatorId = decodedToken.UserId;
            loanAccount.BranchCode = decodedToken.BranchCode;
            loanAccount.EnglishCreationDate = await _helper.GetCompanyActiveCalendarInAD(activeCompanyDate);
            loanAccount.NepaliCreationDate = await _helper.ConvertEnglishDateToNepali(loanAccount.EnglishCreationDate);
        }
        public async Task<ResponseDto> CreateLoanSchemeService(CreateLoanSchemeDto createLoanScheme, TokenDto decodedToken)
        {
            bool isInterestValid = createLoanScheme.InterestRate <= createLoanScheme.MaximumInterestRate && createLoanScheme.InterestRate>=createLoanScheme.MinimumInterestRate;
            if(!isInterestValid)
                throw new Exception("Interest Rate not accepted. Interest Rate should lie in between Minimum and Maximum interest Rate");
            
            bool validateAliasCode = await _loanSetupRepository.ValidateAliasCode(createLoanScheme.AliasCode);
            if(!validateAliasCode)
                throw new Exception("Alias Code already exist");
            LoanScheme loanScheme = _mapper.Map<LoanScheme>(createLoanScheme);
            await AddBasicInfoForLoanScheme(loanScheme, decodedToken);
            string assetsAccountLedgerName = string.IsNullOrEmpty(createLoanScheme.AssetsAccountLedgerName) ? createLoanScheme.Name : createLoanScheme.AssetsAccountLedgerName;
            string interestAccountLedgerName = string.IsNullOrEmpty(createLoanScheme.InterestAccountLedgerName) ? $"Interest from {createLoanScheme.Name}" : createLoanScheme.InterestAccountLedgerName;
            await _loanSetupRepository.CreateLoanScheme(loanScheme, assetsAccountLedgerName, interestAccountLedgerName);
            return new ResponseDto(){Message = "Loan Scheme Added Successfully", Status=true, StatusCode="200"};
        }

        private async Task<LoanAccount> UploadLoanDocument(CreateLoanAccountDto createLoanAccountDto, LoanAccount loanAccount)
        {
            ImageUploadService uploadService = new ImageUploadService(_config);
            List<string> listOfDocumentProperty= new List<string>() { nameof(LoanAccount.UploadedDocument), nameof(LoanAccount.UploadedDocumentFileName), nameof(LoanAccount.UploadedDocumentType) };          
            loanAccount = await uploadService.UploadImage(loanAccount, createLoanAccountDto?.Documents, listOfDocumentProperty);
            return loanAccount;
        }

        public async Task<ResponseDto> CreateLoanAccountService(CreateLoanAccountDto createLoanAccount, TokenDto decodedToken)
        {
            LoanAccount loanAccount = _mapper.Map<LoanAccount>(createLoanAccount);
            await AddBasicInfoForLoanAccount(loanAccount, decodedToken);
            GenerateMatureDateDto generateMatureDate = new GenerateMatureDateDto()
            {
                OpeningDateEnglish=loanAccount.EnglishCreationDate,
                Period = loanAccount.Period,
                PeriodType = loanAccount.PeriodType
            };
            MatureDateDto matureDate = await _helper.GenerateMatureDateOfAccount(generateMatureDate);
            loanAccount.MatureDate = matureDate.EnglishMatureDate;
            if(createLoanAccount.Documents!=null)
                loanAccount= await UploadLoanDocument(createLoanAccount, loanAccount);
            var numberOfRowAffected = await _loanSetupRepository.CreateLoanAccount(loanAccount);
            if(numberOfRowAffected<=0) throw new Exception("Failed to create an account");
            return new ResponseDto(){Message="Successfully created an account", Status=true, StatusCode="200"};
        }

        public async Task<List<LoanSchemeDto>> GetLoanSchemeService(int? loanSchemeId)
        {
            Expression<Func<LoanScheme, bool>> expression = ls =>ls.IsActive;
            if(loanSchemeId!=null)
                expression=ls=>ls.IsActive && ls.Id==loanSchemeId;
            List<LoanScheme> loanSchemes = await _loanSetupRepository.GetSchemes(expression);
            List<LoanSchemeDto> loanSchemeDtos = _mapper.Map<List<LoanSchemeDto>>(loanSchemes);
            return loanSchemeDtos;
        }

        public async Task<List<LoanAccountDto>> GetLoanAccountService(int? loanAccountId)
        {
            List<LoanAccount> loanAccounts = await _loanSetupRepository.GetLoanAccounts(loanAccountId);
            List<LoanAccountDto> loanAccountDtos = _mapper.Map<List<LoanAccountDto>>(loanAccounts);
            return loanAccountDtos;
        }

        public async Task<LoanScheduleDtos> GenerateScheduleService(GenerateLoanScheduleDto generateLoanSchedule)
        {
            return await _loanSetupRepository.GenerateLoanSchedule(generateLoanSchedule);
        }
    }
}