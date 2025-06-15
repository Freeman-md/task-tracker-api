using System.Security.Claims;
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
    var userId = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return Guid.TryParse(userId, out var id) ? id : null;
}

}
