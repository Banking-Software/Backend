using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MicroFinance.Dtos;
using MicroFinance.Role;
using Microsoft.IdentityModel.Tokens;

namespace MicroFinance.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:key"]));

        }
        public string CreateToken(TokenDto tokenDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, tokenDto.UserName),
                new Claim(ClaimTypes.NameIdentifier, tokenDto.UserId),
                new Claim(ClaimTypes.Role, tokenDto.Role),  
                new Claim("IsActive", tokenDto.IsActive),
                new Claim("BranchCode", tokenDto.BranchCode),
                new Claim(ClaimTypes.Email, tokenDto.Email)            
            };            
            var issuer = _config["Token:SuperIssuer"];
            if(tokenDto.Role!=UserRole.SuperAdmin.ToString())
                issuer = _config["Token:Issuer"];

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer=issuer,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public TokenDto DecodeJWT(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(token);
            var user = new TokenDto();
            if(decodedToken.ValidTo > DateTime.UtcNow)
            {
                user.UserId = decodedToken.Claims.First(c=>c.Type=="nameid").Value;
                user.UserName = decodedToken.Claims.First(c=>c.Type=="given_name").Value;
                user.Role = decodedToken.Claims.First(c => c.Type == "role").Value;
                user.IsActive = decodedToken.Claims.First(c=>c.Type=="IsActive").Value;
                user.BranchCode = decodedToken.Claims.First(c=>c.Type=="BranchCode").Value;
                user.Email= decodedToken.Claims.First(c=>c.Type=="email").Value;
            }
            return user;
        }
    }
}