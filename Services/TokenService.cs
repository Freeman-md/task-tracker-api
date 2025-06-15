using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public TokenResponseDto GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]!));

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new TokenResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires
        };
    }
}