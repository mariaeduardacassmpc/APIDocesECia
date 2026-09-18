namespace ApiDoces.Responses;

public record ApiResponse<T>(bool Success, string? Message, T? Data);

public static class ApiResponses
{
    public static ApiResponse<T> Success<T>(
        T data,
        string? message = null)
        => new(true, message, data);

    public static ApiResponse<T> Created<T>(
        T data,
        string? message = null)
        => new(true, message, data);

    public static ApiResponse<T> BadRequest<T>(
        string message)
        => new(false, message, default);

    public static ApiResponse<T> Unauthorized<T>(
        string message)
        => new(false, message, default);

    public static ApiResponse<T> Forbidden<T>(
        string message)
        => new(false, message, default);

    public static ApiResponse<T> NotFound<T>(
        string message)
        => new(false, message, default);

    public static ApiResponse<T> Conflict<T>(
        string message)
        => new(false, message, default);

    public static ApiResponse<T> InternalServerError<T>(
        string message)
        => new(false, message, default);
}