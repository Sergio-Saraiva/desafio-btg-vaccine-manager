using System.Diagnostics;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace VaccineManager.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling {RequestName} — {@Request}", requestName, request);

        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        if (response.IsFailed)
        {
            _logger.LogWarning(
                "Completed {RequestName} with failure — {Errors} ({ElapsedMs}ms)",
                requestName,
                string.Join("; ", response.Errors.Select(e => e.Message)),
                stopwatch.ElapsedMilliseconds);
        }
        else
        {
            _logger.LogInformation(
                "Completed {RequestName} successfully ({ElapsedMs}ms)",
                requestName,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}