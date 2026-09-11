namespace ApiDoces.Responses;

public static class ApiResponse
{
    public static object Success<T>(T data, string? message = null)
    {
        return new
        {
            success = true,
            message,
            data
        };
    }

    public static object Created<T>(T data, string? message = null)
    {
        return new
        {
            success = true,
            message,
            data
        };
    }

    public static object BadRequest(string message)
    {
        return new
        {
            success = false,
            message,
            data = (object?)null
        };
    }

    public static object Unauthorized(string message = "Não autorizado.")
    {
        return new
        {
            success = false,
            message,
            data = (object?)null
        };
    }

    public static object Forbidden(string message = "Acesso negado.")
    {
        return new
        {
            success = false,
            message,
            data = (object?)null
        };
    }

    public static object NotFound(string message)
    {
        return new
        {
            success = false,
            message,
            data = (object?)null
        };
    }

    public static object Conflict(string message)
    {
        return new
        {
            success = false,
            message,
            data = (object?)null
        };
    }

    public static object InternalServerError(
        string message = "Ocorreu um erro inesperado.")
    {
        return new
        {
            success = false,
            message,
            data = (object?)null
        };
    }
}