using MicroFinance.Dtos;
namespace MicroFinance.Token
{
    public interface ITokenService
    {
        string CreateToken(TokenDto user);
        TokenDto DecodeJWT(string token);
    }
}