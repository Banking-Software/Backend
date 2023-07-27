using MicroFinance.Dtos;
using MicroFinance.Dtos.Transactions;
using MicroFinance.Services.Transactions;
using MicroFinance.Token;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.Transactions
{
    public class TransactionController : BaseApiController
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IDepositAccountTransactionService _depositAccountTransactionService;
        private readonly ITokenService _tokenService;

        public TransactionController
        (
            ILogger<TransactionController> logger, 
            IDepositAccountTransactionService depositAccountTransactionService,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _depositAccountTransactionService = depositAccountTransactionService;
            _tokenService = tokenService;
        }
        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }

        [HttpPost("makeDeposit")]
        public async Task<ActionResult<ResponseDto>> MakeDeposit([FromForm] MakeDepositTransactionDto makeDepositTransactionDto)
        {
            var decodedToken = GetDecodedToken();
            return Ok(await _depositAccountTransactionService.MakeDepositTransactionService(makeDepositTransactionDto, decodedToken));
        }
    }
}