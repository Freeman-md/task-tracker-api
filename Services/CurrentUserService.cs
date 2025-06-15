using api.Interfaces;
using api.Models;

namespace api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _http;

    public CurrentUserService(IHttpContextAccessor http)
    {
        _http = http;
    }

    public Guid? GetUserId()
    {
        var userId = _http.HttpContext?.User?.FindFirst(JwtClaims.UserId)?.Value;
        return Guid.TryParse(userId, out var id) ? id : null;
    }
}
