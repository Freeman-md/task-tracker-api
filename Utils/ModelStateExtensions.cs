using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Utils;

public static class ModelStateExtensions
{
    public static List<ApiError> ToApiErrors(this ModelStateDictionary modelState)
    {
        return modelState
                .Where(error => error.Value?.Errors.Count > 0)
                .SelectMany(e => e.Value!.Errors.Select(error => new ApiError(e.Key, error.ErrorMessage)))
                .ToList();
    }
}