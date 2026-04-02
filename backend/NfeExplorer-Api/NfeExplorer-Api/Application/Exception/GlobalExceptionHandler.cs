using System.Text.Json;

namespace NfeExplorer_Api.Application.Exception;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (System.Exception ex)
        {
            var (statusCode, message) = ex switch
            {
                ArgumentException => (400, ex.Message),
                KeyNotFoundException => (404, ex.Message),
                UnauthorizedAccessException => (401, ex.Message),
                _ => (500, "Erro interno no servidor.")
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                code = statusCode,
                message
            }));
        }
    }
}