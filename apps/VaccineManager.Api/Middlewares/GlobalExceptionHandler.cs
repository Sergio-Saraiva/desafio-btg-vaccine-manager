using Microsoft.AspNetCore.Diagnostics;
using VaccineManager.Application.Common.Responses;

namespace VaccineManager.Api.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _logger = logger;
        _environment = env;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception,
            "Unhandled exception — {ExceptionType}: {Message}",
            exception.GetType().Name,
            exception.Message);

        var errorMessage = _environment.IsDevelopment()
            ? $"{exception.GetType().Name}: {exception.Message}\n{exception.StackTrace}"
            : "Internal Server Error";

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(
            ApiResponse.Failure(errorMessage),
            cancellationToken);

        return true;
    }
}