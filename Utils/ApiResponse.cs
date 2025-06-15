namespace api.Utils;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }

    public List<ApiError>? Errors { get; set; }

    public static ApiResponse<T> SuccessResponse(
        T data,
        string? message = null
    ) => new()
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static ApiResponse<T> ErrorResponse(
        string message,
        List<ApiError>? errors = null
    ) => new()
    {
        Success = false,
        Message = message,
        Errors = errors
    };
}