using System;

namespace api.Interfaces;

public interface ICurrentUserService
{
    Guid? GetUserId();
}
