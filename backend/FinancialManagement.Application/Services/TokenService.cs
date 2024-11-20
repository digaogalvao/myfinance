using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinancialManagement.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly string key = string.Empty;
        private readonly string issuer = string.Empty;
        private readonly string audience = string.Empty;
        private readonly int duration;

        public TokenService(IConfiguration configuration)
        {
            key = configuration["Jwt:Key"]!;
            issuer = configuration["Jwt:Issuer"];
            audience = configuration["Jwt:Audience"];
            duration = int.Parse(configuration["Jwt:Duration"]!);
        }

        public string GenerateToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));
            var signingKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", usuario.Id.ToString()),
                new Claim("nome", usuario.Nome),
                new Claim("email", usuario.Email),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(this.duration),
                signingKey);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
