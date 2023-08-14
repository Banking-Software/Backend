using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions.ShareTransaction;
using MicroFinance.Enums.Deposit.Account;
using MicroFinance.Enums.Transaction;
using MicroFinance.Enums.Transaction.ShareTransaction;
using MicroFinance.Exceptions;
using MicroFinance.Helper;
using MicroFinance.Models.DepositSetup;
using MicroFinance.Models.Wrapper.TrasactionWrapper;
using MicroFinance.Repository.Transaction;
using MicroFinance.Services.AccountSetup.MainLedger;
using MicroFinance.Services.ClientSetup;
using MicroFinance.Services.CompanyProfile;
using MicroFinance.Services.DepositSetup;
using MicroFinance.Services.Share;

namespace MicroFinance.Services.Transactions
{
    public class ShareAccountTransactionService : IShareAccountTransactionService
    {
        private readonly IShareAccountTransactionRepository _shareAccountTransactionRepository;
        private readonly IShareService _shareService;
        private readonly IClientService _clientService;
        private readonly IMainLedgerService _mainLedgerService;
        private readonly IDepositSchemeService _depositSchemeService;
        private readonly ICompanyProfileService _companyProfileService;
        private readonly IMapper _mapper;
        private readonly INepaliCalendarFormat _nepaliCalendarFormat;
        private readonly ICommonExpression _commonExpression;

        public ShareAccountTransactionService
        (
            IShareAccountTransactionRepository shareAccountTransactionRepository, 
            IMapper mapper,
            IShareService shareService,
            IClientService clientService,
            IMainLedgerService mainLedgerService,
            IDepositSchemeService depositSchemeService,
            ICompanyProfileService companyProfileService,
            INepaliCalendarFormat nepaliCalendarFormat,
            ICommonExpression commonExpression
        )
        {
            _shareAccountTransactionRepository=shareAccountTransactionRepository;
            _shareService = shareService;
            _clientService = clientService;
            _mainLedgerService = mainLedgerService;
            _depositSchemeService = depositSchemeService;
            _companyProfileService=companyProfileService;
            _mapper=mapper;
            _nepaliCalendarFormat=nepaliCalendarFormat;
            _commonExpression=commonExpression;
        }

        private async Task AddBasicInfo(ShareAccountTransactionWrapper shareAccountTransactionWrapper, TokenDto decodedToken)
        {
            var companyCalendar = await _companyProfileService.GetCurrentActiveCalenderService();
            shareAccountTransactionWrapper.CreatedBy = decodedToken.UserName;
            shareAccountTransactionWrapper.CreatorId = decodedToken.UserId;
            shareAccountTransactionWrapper.BranchCode = decodedToken.BranchCode;
            shareAccountTransactionWrapper.RealWorldCreationDate = DateTime.Now;
            string nepaliCreationDate =  await _nepaliCalendarFormat.GetNepaliFormatDate(companyCalendar.Year, companyCalendar.Month, companyCalendar.RunningDay);
            if(string.IsNullOrEmpty(nepaliCreationDate)) throw new Exception("Unable to assign the creation date");
            shareAccountTransactionWrapper.NepaliCreationDate = nepaliCreationDate;
            shareAccountTransactionWrapper.EnglishCreationDate = await _nepaliCalendarFormat.ConvertNepaliDateToEnglish(nepaliCreationDate);
        }
        public async Task<string> MakeShareTransaction(MakeShareTransactionDto makeShareTransactionDto, TokenDto decodedToken)
        {
            var shareAccountTransactionWrapper = _mapper.Map<ShareAccountTransactionWrapper>(makeShareTransactionDto);
            var shareAccount = await _shareService.GetShareAccountService(shareId: makeShareTransactionDto.ShareAccountId, null ,decodedToken:decodedToken);
            var client = await _clientService.GetClientByIdService(makeShareTransactionDto.ClientId, decodedToken);
            if(!shareAccount.IsActive || !client.IsActive || shareAccount.ClientId!=client.Id)
                throw new Exception("Transaction Failed and possible reason might be: provided share account might not be of provided client or share account and client is inactive");
            var shareKitta = await _shareService.GetActiveShareKittaService(decodedToken);
            shareAccountTransactionWrapper.ShareKittaId = shareKitta.Id;
            if(makeShareTransactionDto.ShareTransactionType==ShareTransactionTypeEnum.Transfer)
            {
                Expression<Func<DepositAccount, bool>> expressionOnTransferAccount = await _commonExpression.GetExpressionOfDepositAccountForTransaction
                (
                    depositAccountId:(int) makeShareTransactionDto.TransferToDepositAccountId,
                    isDeposit:true
                );
                var transferToDepositAccount = await _depositSchemeService.GetDepositAccountWrapperByIdService(null,expressionOnTransferAccount ,decodedToken); 
                shareAccountTransactionWrapper.TransferToDepositSchemeId = transferToDepositAccount.DepositScheme.Id;
                shareAccountTransactionWrapper.TransferToDepositSchemeSubLedgerId = transferToDepositAccount.DepositScheme.DepositSubledgerId;
                shareAccountTransactionWrapper.TransferToDepositSchemeLedgerId = transferToDepositAccount.DepositScheme.SchemeTypeId;
            }
            else if(makeShareTransactionDto.PaymentType == PaymentTypeEnum.Bank)
            {
                var bankDetail = await _mainLedgerService.GetBankSetupByIdService((int) makeShareTransactionDto.BankDetailId);
                shareAccountTransactionWrapper.BankLedgerId = bankDetail.LedgerId;
            }
            else if(makeShareTransactionDto.PaymentType == PaymentTypeEnum.Account)
            {
                Expression<Func<DepositAccount, bool>> expressionOnPaymentType = await _commonExpression.GetExpressionOfDepositAccountForTransaction
                (
                    depositAccountId:(int) makeShareTransactionDto.PaymentDepositAccountId,
                    isDeposit: makeShareTransactionDto.ShareTransactionType==ShareTransactionTypeEnum.Refund?true:false
                );
                var paymentDepositAccountNumber = await _depositSchemeService.GetDepositAccountWrapperByIdService(null, expressionOnPaymentType ,decodedToken);
                
                shareAccountTransactionWrapper.PaymentDepositSchemeId = paymentDepositAccountNumber.DepositScheme.Id;
                shareAccountTransactionWrapper.PaymentDepositSchemeSubLedgerId = paymentDepositAccountNumber.DepositScheme.DepositSubledgerId;
                shareAccountTransactionWrapper.PaymentDepositSchemeLedgerId = paymentDepositAccountNumber.DepositScheme.SchemeTypeId;
            }
            await AddBasicInfo(shareAccountTransactionWrapper, decodedToken);
            string voucherNumber = await _shareAccountTransactionRepository.LockAndMakeShareTransaction(shareAccountTransactionWrapper);
            return voucherNumber;

        }
    }
}