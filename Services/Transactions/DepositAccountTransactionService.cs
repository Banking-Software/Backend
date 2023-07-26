using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Enums.Transaction;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using MicroFinance.Repository.Transaction;
using MicroFinance.Services.AccountSetup.MainLedger;
using MicroFinance.Services.ClientSetup;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.DepositSetup;
using MicroFinance.Services.UserManagement;

namespace MicroFinance.Services.Transactions
{
    public class DepositAccountTransactionService : IDepositAccountTransactionService
    {
        private readonly ILogger<DepositAccountTransactionService> _logger;
        private readonly IDepositAccountTransactionRepository _depositAccountTransactionRepo;
        private readonly IDepositSchemeService _depositSchemeService;
        private readonly IClientService _clientService;
        private readonly IMainLedgerService _mainLedgerService;
        private readonly IMapper _mapper;
        private readonly ICompanyProfileService _companyProfile;
        private readonly IEmployeeService _employeeService;

        public DepositAccountTransactionService
        (
            ILogger<DepositAccountTransactionService> logger, 
            IMapper mapper,
            IDepositAccountTransactionRepository depositAccountTransactionRepo,
            IDepositSchemeService depositSchemeService,
            IClientService clientService,
            ICompanyProfileService companyProfile,
            IMainLedgerService mainLedgerService,
            IEmployeeService employeeService
        )
        {
            _logger = logger;
            _mapper=mapper;
            _depositAccountTransactionRepo = depositAccountTransactionRepo;
            _depositSchemeService=depositSchemeService;
            _clientService=clientService;
            _mainLedgerService = mainLedgerService;
            _companyProfile = companyProfile;
            _employeeService = employeeService;
        }
        public async Task<ResponseDto> MakeDepositTransactionService(MakeDepositTransactionDto makeDepositTransactionDto, TokenDto decodedToken)
        {
            var depositAccountWrapper = await _depositSchemeService.GetNonClosedDepositAccountByIdService(makeDepositTransactionDto.DepositAccountId, decodedToken);
            if(depositAccountWrapper.DepositAccount.Status!=AccountStatusEnum.Active)
                throw new Exception("Deposit Account is not active");
            if(makeDepositTransactionDto.CollectedByEmployeeId!=null)
            {
                var employee = await _employeeService.GetEmployeeByIdFromUserBranch((int) makeDepositTransactionDto.CollectedByEmployeeId, decodedToken);
            }

            var companyCalendar = await _companyProfile.GetCurrentActiveCalenderService();
            MakeDepositWrapper depositWrapper = _mapper.Map<MakeDepositWrapper>(makeDepositTransactionDto);
            depositWrapper.DepositSchemeId = depositAccountWrapper.DepositScheme.Id;
            depositWrapper.DepositSchemeSubLedgerId = depositAccountWrapper.DepositScheme.DepositSubledgerId;
            if(depositWrapper.PaymentType==PaymentTypeEnum.Bank)
            {
                var bankdetail = await _mainLedgerService.GetBankSetupByIdService((int) depositWrapper.BankDetailId);
                if(bankdetail.BranchCode!=decodedToken.BranchCode)
                    throw new Exception("Provided Bank doesnot belong to your branch");
                depositWrapper.BankLedgerId = bankdetail.LedgerId;
            }
            depositWrapper.AccountNumber = depositAccountWrapper.DepositAccount.AccountNumber;
            depositWrapper.TransactionType = TransactionTypeEnum.Credit;
            depositWrapper.CreatedBy = decodedToken.UserName;
            depositWrapper.CreatorId = decodedToken.UserId;
            depositWrapper.BranchCode = decodedToken.BranchCode;
            depositWrapper.RealWorldCreationDate = DateTime.Now;
            depositWrapper.CompanyCalendarCreationDate = new DateTime(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            
            string voucherNumber = await _depositAccountTransactionRepo.MakeDeposit(depositWrapper);
            return new ResponseDto()
            {
                Message=$"Transaction Successfull. Generated Voucher Number is {voucherNumber}",
                Status=true,
                StatusCode="200"
            };
        }
    }
}