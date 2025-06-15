using api.DTOs;
using api.Models;

namespace api.Interfaces;

public interface IAuthService
{
    public Task<User> RegisterAsync(RegisterUserDto dto);
    public Task<string> LoginAsync(LoginDto dto);
}