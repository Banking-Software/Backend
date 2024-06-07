using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.AccountSetup.MainLedger;
using MicroFinance.Dtos.DepositSetup.Account;
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
using MicroFinance.Helpers;
using MicroFinance.Models.DepositSetup;
using System.Linq.Expressions;

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
        private readonly IHelper _helper;
        private readonly ICommonExpression _commonExpression;
        private readonly IBaseTransactionRepository _transactionRepository;

        public DepositAccountTransactionService
        (
            ILogger<DepositAccountTransactionService> logger,
            IMapper mapper,
            IDepositAccountTransactionRepository depositAccountTransactionRepo,
            IDepositSchemeService depositSchemeService,
            IClientService clientService,
            ICompanyProfileService companyProfile,
            IMainLedgerService mainLedgerService,
            IEmployeeService employeeService,
            IHelper helper,
            ICommonExpression commonExpression,
            IBaseTransactionRepository transactionRepository
        )
        {
            _logger = logger;
            _mapper = mapper;
            _depositAccountTransactionRepo = depositAccountTransactionRepo;
            _depositSchemeService = depositSchemeService;
            _clientService = clientService;
            _mainLedgerService = mainLedgerService;
            _companyProfile = companyProfile;
            _employeeService = employeeService;
            _helper=helper;
            _commonExpression = commonExpression;
            _transactionRepository=transactionRepository;
        }
        private async Task<BankSetupDto> GetBankDetails(int bankId, TokenDto decodedToken)
        {
            var bankdetail = await _mainLedgerService.GetBankSetupByIdService(bankId);
            if (bankdetail == null)
                throw new Exception("No data found for provided bank");
            if (bankdetail.BranchCode != decodedToken.BranchCode)
                throw new Exception("Provided Bank doesnot belong to your branch");
            return bankdetail;
        }
        private async Task<DepositAccountWrapperDto> GetDepositAccount(int depositAccountId, TokenDto decodedToken, bool isDeposit)
        {
            Expression<Func<DepositAccount, bool>> expressionToQueryDepositAccount = await _commonExpression.GetExpressionOfDepositAccountForTransaction(depositAccountId, isDeposit);
            var depositAccountWrapper = await _depositSchemeService.GetDepositAccountWrapperByIdService(null, expressionToQueryDepositAccount, decodedToken);
            return depositAccountWrapper;
        }
        private async Task<string> VerifyAmountCollectedEmployee(int employeeId, TokenDto decodedToken)
        {
            var employee = await _employeeService.GetEmployeeByIdFromUserBranch(employeeId, decodedToken);
            return employee.Name;
        }
        private async Task<DepositAccountTransactionWrapper> BaseDepositAccountTransactionService(dynamic transactionDto, TokenDto decodedToken, bool isDeposit)
        {
            string collectedByEmployeeName = "";
            if (!(transactionDto is MakeDepositTransactionDto) && !(transactionDto is MakeWithDrawalTransactionDto))
                throw new Exception("Invalid Model is Passed");

            if (transactionDto.CollectedByEmployeeId != null)
            {
                collectedByEmployeeName = await VerifyAmountCollectedEmployee((int)transactionDto.CollectedByEmployeeId, decodedToken);
            }
            var depositAccountWrapper = await GetDepositAccount((int)transactionDto.DepositAccountId, decodedToken, isDeposit);
            var companyCalendar = await _companyProfile.GetCurrentActiveCalenderService();
            DepositAccountTransactionWrapper transactionData = _mapper.Map<DepositAccountTransactionWrapper>(transactionDto);
            transactionData.DepositSchemeId = depositAccountWrapper.DepositScheme.Id;
            transactionData.DepositSchemeSubLedgerId = depositAccountWrapper.DepositScheme.DepositSubledgerId;
            transactionData.DepositSchemeLedgerId = (await _mainLedgerService.GetSubLedgerByIdService(depositAccountWrapper.DepositScheme.DepositSubledgerId)).LedgerId;
            if (transactionData.PaymentType == PaymentTypeEnum.Bank)
            {
                BankSetupDto bankDetail = await GetBankDetails((int)transactionData.BankDetailId, decodedToken);
                transactionData.BankLedgerId = bankDetail.LedgerId;
            }
            transactionData.AccountNumber = depositAccountWrapper.DepositAccount.AccountNumber;
            transactionData.CreatedBy = decodedToken.UserName;
            transactionData.CreatorId = decodedToken.UserId;
            transactionData.BranchCode = decodedToken.BranchCode;
            transactionData.RealWorldCreationDate = DateTime.Now;
            string nepaliCreationDate = await _helper.GetNepaliFormatDate(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            if(string.IsNullOrEmpty(nepaliCreationDate)) throw new Exception("Unable to assign the creation date");
            transactionData.NepaliCreationDate = nepaliCreationDate;
            transactionData.EnglishCreationDate = await _helper.ConvertNepaliDateToEnglish(transactionData.NepaliCreationDate);
            transactionData.CollectedByEmployeeName = collectedByEmployeeName;
            return transactionData;
            
        }
        public async Task<VoucherDto> MakeDepositTransactionService(MakeDepositTransactionDto makeDepositTransactionDto, TokenDto decodedToken)
        {
            var depositTransactionData = await BaseDepositAccountTransactionService(makeDepositTransactionDto, decodedToken, true);
            depositTransactionData.TransactionType = TransactionTypeEnum.Credit;
            string voucherNumber= await _transactionRepository.DepositAccountTransaction(depositTransactionData);
            VoucherDto vocherDto=new VoucherDto()
            {
                VoucherNumber=voucherNumber,
                TransactionAmount=makeDepositTransactionDto.TransactionAmount,
                AmountInWords= await _helper.HumanizeAmount(makeDepositTransactionDto.TransactionAmount),
                ModeOfPayment = makeDepositTransactionDto.PaymentType.ToString(),
                TransactionDateBS = depositTransactionData.NepaliCreationDate,
                TransactionDateAD = depositTransactionData.EnglishCreationDate,
                RealWorldTransactionDateAD = depositTransactionData.RealWorldCreationDate,
                RealWorldTransactionDateBS = await _helper.ConvertEnglishDateToNepali(depositTransactionData.RealWorldCreationDate),
                CollectedBy = depositTransactionData.CollectedByEmployeeName
            };
            return vocherDto;
        }

        public async Task<VoucherDto> MakeWithDrawalTransactionService(MakeWithDrawalTransactionDto makeWithDrawalTransactionDto, TokenDto decodedToken)
        {
            
            var withDrawalTransactionData = await BaseDepositAccountTransactionService(makeWithDrawalTransactionDto, decodedToken, false);
            withDrawalTransactionData.TransactionType = TransactionTypeEnum.Debit;
            string voucherNumber =  await _transactionRepository.DepositAccountTransaction(withDrawalTransactionData);
            VoucherDto vocherDto=new VoucherDto()
            {
                VoucherNumber=voucherNumber,
                TransactionAmount=makeWithDrawalTransactionDto.TransactionAmount,
                AmountInWords= await _helper.HumanizeAmount(makeWithDrawalTransactionDto.TransactionAmount),
                ModeOfPayment = makeWithDrawalTransactionDto.PaymentType.ToString(),
                TransactionDateBS = withDrawalTransactionData.NepaliCreationDate,
                TransactionDateAD = withDrawalTransactionData.EnglishCreationDate,
                RealWorldTransactionDateAD = withDrawalTransactionData.RealWorldCreationDate,
                RealWorldTransactionDateBS = await _helper.ConvertEnglishDateToNepali(withDrawalTransactionData.RealWorldCreationDate),
                CollectedBy = withDrawalTransactionData.CollectedByEmployeeName
            };
            return vocherDto;
            // return await _depositAccountTransactionRepo.MakeTransaction(withDrawalTransactionData);
        }
    }
}