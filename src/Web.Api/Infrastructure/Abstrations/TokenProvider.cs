using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Web.Api.Domains;

namespace Web.Api.Infrastructure.Abstrations;

public sealed class TokenProvider(IConfiguration configuration)
{
    public string Create(Member member)
    {
        string? secret = configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JWT secret is not configured.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, member.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, member.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
