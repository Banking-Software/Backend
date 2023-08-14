using System.Linq.Expressions;
using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.Share;
using MicroFinance.Exceptions;
using MicroFinance.Helper;
using MicroFinance.Models.Share;
using MicroFinance.Repository.Share;

namespace MicroFinance.Services.Share
{
    public class ShareService : IShareService
    {
        private readonly IShareRepository _shareRepository;
        private readonly ILogger<ShareService> _logger;
        private readonly IMapper _mapper;
        private readonly ICommonExpression _commonExpression;

        public ShareService(IShareRepository shareRepository, ILogger<ShareService> logger, IMapper mapper, ICommonExpression commonExpression)
        {
            _shareRepository = shareRepository;
            _logger = logger;
            _mapper= mapper;
            _commonExpression=commonExpression;
        }

        
        public async Task<List<ShareAccountDto>> GetAllActiveShareAccountsService(TokenDto decodedToken)
        {
            _logger.LogInformation($"{DateTime.Now}: {decodedToken.UserName} is requesting to fetch all share accounts");
            List<ShareAccountDto> shareAccountDtos = new();
            var shareAccounts = await _shareRepository.GetAllActiveShareAccount();
            if(shareAccounts==null||shareAccounts.Count<=0)
                _logger.LogInformation($"{DateTime.Now}: No Data Found for share accounts");
            else
            {
                _logger.LogInformation($"{DateTime.Now}: Sending all share accounts details to {decodedToken.UserName}");
                shareAccountDtos = _mapper.Map<List<ShareAccountDto>>(shareAccounts);
            }
            return shareAccountDtos;

        }
        public async Task<ShareAccountDto> GetShareAccountService(int? shareId, string? clientMemberId, TokenDto decodedToken)
        {
            string requestingId = clientMemberId!=null?"Client":"Share Account";
           _logger.LogInformation($"{DateTime.Now}: {decodedToken.UserName} is requesting for share account of '{requestingId}' Id of {shareId}");
           Expression<Func<ShareAccount, bool>> checksOnData =await _commonExpression.GetExpressionOfShareAccountForTransaction(shareId, clientMemberId);
           var shareAccount =  await _shareRepository.GetShareAccount(checksOnData);
           if(shareAccount!=null)
           {
                _logger.LogInformation($"{DateTime.Now}: Send Share account details of {shareAccount.ClientName} where ShareId and ClientId are {shareAccount.Id} and {shareAccount.ClientId} respec. to {decodedToken.UserName}");
                return _mapper.Map<ShareAccountDto>(shareAccount);
           }
           throw new NotImplementedException("No data found for requested share account");
        }

        public async Task<ResponseDto> CreateShareKittaService(CreateShareKittaDto createShareKitta, TokenDto decodedToken)
        {
            var anyKittaExist = await _shareRepository.GetShareKitta();
            if(anyKittaExist!=null) throw new BadRequestExceptionHandler("Kitta Entry exist");
            ShareKitta shareKitta = new()
            {
                PriceOfOneKitta = createShareKitta.PriceOfOneKitta
            };
            var kittaId = await _shareRepository.CreateShareKitta(shareKitta);
            if(kittaId<1) throw new Exception("Failed to create share kitta");
            return new ResponseDto(){Message="Successfully Created Share Kitta", Status=true, StatusCode="200"};
        }
        public async Task<ResponseDto> UpdateShareKittaService(UpdateShareKittaDto updateShareKittaDto, TokenDto decodedToken)
        {
            ShareKitta shareKitta = new()
            {
                Id = updateShareKittaDto.Id,
                PriceOfOneKitta = updateShareKittaDto.PriceOfOneKitta
            };
            await _shareRepository.UpdateShareKitta(shareKitta);
            return new ResponseDto(){Message="Successfully updated"};
        }

        public async Task<ShareKittaDto> GetActiveShareKittaService(TokenDto decodedToken)
        {
            var shareKitta = await _shareRepository.GetShareKitta();
            if(shareKitta==null) throw new Exception("No Active Share Kitta Exist");
            return new ShareKittaDto()
            {
                Id= shareKitta.Id,
                PriceOfOneKitta= shareKitta.PriceOfOneKitta,
                IsActive = shareKitta.IsActive,
                CurrentKitta= shareKitta.CurrentKitta
            };
        }

       
    }
}