using MicroFinance.Dto.Transactions;
using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions;
using MicroFinance.Dtos.Transactions.ShareTransaction;
using MicroFinance.Services;
using MicroFinance.Services.Transactions;
using MicroFinance.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.Transactions
{
    [Authorize(AuthenticationSchemes = "UserToken")]
    [TypeFilter(typeof(IsActiveAuthorizationFilter))]
    public class TransactionController : BaseApiController
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IDepositAccountTransactionService _depositAccountTransactionService;
        private readonly ITokenService _tokenService;
        private readonly IShareAccountTransactionService _shareAccountTransactionService;
        private readonly ITransactionService _transactionService;

        public TransactionController
        (
            ILogger<TransactionController> logger, 
            IDepositAccountTransactionService depositAccountTransactionService,
            IShareAccountTransactionService shareAccountTransactionService,
            ITransactionService transactionService,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _depositAccountTransactionService = depositAccountTransactionService;
            _tokenService = tokenService;
            _shareAccountTransactionService=shareAccountTransactionService;
            _transactionService=transactionService;
        }
        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }

        [HttpPost("makeDeposit")]
        public async Task<ActionResult<VoucherDto>> MakeDeposit(MakeDepositTransactionDto makeDepositTransactionDto)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositAccountTransactionService.MakeDepositTransactionService(makeDepositTransactionDto, decodedToken));
        }

        [HttpPost("makeWithDrawal")]
        public async Task<ActionResult<VoucherDto>> MakeWithDrawal(MakeWithDrawalTransactionDto makeWithDrawalTransactionDto)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositAccountTransactionService.MakeWithDrawalTransactionService(makeWithDrawalTransactionDto, decodedToken));
        }

        [HttpPost("share")]
        public async Task<ActionResult<VoucherDto>> MakeShareTransaction(MakeShareTransactionDto shareTransactionDto)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _shareAccountTransactionService.MakeShareTransaction(shareTransactionDto, decodedToken));
        }

        [HttpPost("manualVoucher")]
        public async Task<ActionResult<ResponseDto>> ManualVoucherTransaction(List<ManualVoucherDto> manualVouchers)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _transactionService.ManualVoucherTransactionService(manualVouchers, decodedToken));
        }
    }
}