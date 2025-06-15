using api.DTOs;
using api.Models;

namespace api.Interfaces;

public interface ITokenService
{
    TokenResponseDto GenerateToken(User user);
}