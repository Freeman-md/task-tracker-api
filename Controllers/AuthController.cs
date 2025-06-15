using api.DTOs;
using api.Interfaces;
using api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    ModelState.ToApiErrors()
                )
            );
        }

        try
        {
            var user = await _authService.RegisterAsync(dto);
            return Ok(
                ApiResponse<object>.SuccessResponse(
                    new
                    {
                        user.Id,
                        user.Email,
                        user.FirstName
                    },
                    "User registered successfully"
                )
            );
        }
        catch (Exception ex)
        {
            return BadRequest(
                ApiResponse<object>.ErrorResponse(ex.Message)
            );
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(
                ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    ModelState.ToApiErrors()
                )
            );
        }

        try
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(
                ApiResponse<object>.SuccessResponse(
                    new { token },
                    "Login Successful"
                )
            );
        }
        catch (System.Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}