using System.Text.Json;

namespace NfeExplorer_Api.Application.Exception;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Exceção não tratada na requisição {Method} {Path}: {Message}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                ex.Message);

            if (httpContext.Response.HasStarted)
            {
                throw;
            }

            var (statusCode, message) = ex switch
            {
                ArgumentException => (400, ex.Message),
                KeyNotFoundException => (404, ex.Message),
                UnauthorizedAccessException => (401, ex.Message),
                _ => (500, ex.Message)
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