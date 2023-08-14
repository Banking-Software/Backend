using MicroFinance.Dtos;
using MicroFinance.Dtos.Share;
using MicroFinance.Services.Share;
using MicroFinance.Token;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.Share
{
    public class ShareController : BaseApiController
    {
        private readonly IShareService _shareService;
        private readonly ITokenService _tokenService;

        public ShareController(IShareService shareService, ITokenService  tokenService)
        {
            _shareService = shareService;
            _tokenService = tokenService;
        }
        private TokenDto GetDecodedToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var decodedToken = _tokenService.DecodeJWT(token);
            return decodedToken;
        }
        [HttpGet]
        public async Task<ActionResult<ShareAccountDto>> GetActiveShareAccount([FromQuery] int? shareId, [FromQuery] string? clientMemberId)
        {
            TokenDto decodedToken = GetDecodedToken();
            return Ok(await _shareService.GetShareAccountService(shareId, clientMemberId, decodedToken));
        }
        [HttpGet("allActive")]
        public async Task<ActionResult<List<ShareAccountDto>>> GetAllActiveShareAccount()
        {
            TokenDto decodedToken = GetDecodedToken();
            return Ok(await _shareService.GetAllActiveShareAccountsService(decodedToken));
        }
        
        [HttpPost("shareKitta")]
        public async Task<ActionResult<ResponseDto>> CreateShareKitta(CreateShareKittaDto createShareKittaDto)
        {
            TokenDto decodedToken = GetDecodedToken();
            return Ok(await _shareService.CreateShareKittaService(createShareKittaDto, decodedToken));
        }

        [HttpPut("shareKitta")]
        public async Task<ActionResult<ResponseDto>> UpdateShareKitta(UpdateShareKittaDto updateShareKittaDto)
        {
            TokenDto decodedToken = GetDecodedToken();
            return Ok(await _shareService.UpdateShareKittaService(updateShareKittaDto, decodedToken));
        }

        [HttpGet("shareKitta")]
        public async Task<ActionResult<ResponseDto>> GetShareKitta()
        {
            TokenDto decodedToken = GetDecodedToken();
            return Ok(await _shareService.GetActiveShareKittaService(decodedToken));
        }
    }
}