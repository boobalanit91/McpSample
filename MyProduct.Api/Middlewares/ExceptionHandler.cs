using Microsoft.AspNetCore.Diagnostics;

namespace MyProduct.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred in MCP.");

        httpContext.Response.StatusCode = 500;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            StatusCode = 500,
            Message = $"Something went wrong in MCP, {exception.Message} , Inner: {exception.InnerException}"
        }, cancellationToken);

        return true;
    }
}