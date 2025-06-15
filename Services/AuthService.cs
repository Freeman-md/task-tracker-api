using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _appDbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext appDbContext, IPasswordHasher<User> passwordHasher, ILogger<AuthService> logger)
    {
        _appDbContext = appDbContext;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public Task<User> LoginAsync(RegisterUserDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<User> RegisterAsync(RegisterUserDto dto)
    {
        bool userWithEmailExists = await _appDbContext.Users.AnyAsync(user => user.Email == dto.Email.ToLower());

        if (userWithEmailExists) throw new Exception("User with email already exists");

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email.ToLower(),
            PasswordHash = ""
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _appDbContext.Users.Add(user);
        await _appDbContext.SaveChangesAsync();

        return user;
    }
}