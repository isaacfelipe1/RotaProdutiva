using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Entities;

namespace RotaProdutiva.Infrastructure.Security
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GerarToken(Usuario usuario)
        {
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Chave));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, usuario.Email),
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.Nome),
                new(ClaimTypes.Role, usuario.Tipo.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Emissor,
                audience: _jwtSettings.Audiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiracaoMinutos),
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
