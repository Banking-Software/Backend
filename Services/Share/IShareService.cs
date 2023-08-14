using MicroFinance.Dtos;
using MicroFinance.Dtos.Share;
using MicroFinance.Models.Share;

namespace MicroFinance.Services.Share
{
    public interface IShareService
    {
        Task<ShareAccountDto> GetShareAccountService(int? shareId, string clientMemberId, TokenDto decodedToken);
        Task<List<ShareAccountDto>> GetAllActiveShareAccountsService(TokenDto decodedToken);

        Task<ResponseDto> CreateShareKittaService(CreateShareKittaDto createShareKitta, TokenDto decodedToken);
        Task<ResponseDto> UpdateShareKittaService(UpdateShareKittaDto updateShareKittaDto, TokenDto decodedToken);
        Task<ShareKittaDto> GetActiveShareKittaService(TokenDto decodedToken);
    }
}