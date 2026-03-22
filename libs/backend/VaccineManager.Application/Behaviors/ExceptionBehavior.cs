using System.Net;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VaccineManager.Application.Common.Errors;

namespace VaccineManager.Application.Behaviors;

public class ExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly ILogger<ExceptionBehavior<TRequest, TResponse>> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionBehavior(
        ILogger<ExceptionBehavior<TRequest, TResponse>> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex,
                "Unhandled exception in {RequestName} — {ExceptionType}: {Message}",
                requestName, ex.GetType().Name, ex.Message);

            var errorMessage = _environment.IsDevelopment()
                ? $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}"
                : "Internal Server Error";

            var response = new TResponse();
            response.Reasons.Add(new ApiError(HttpStatusCode.InternalServerError, errorMessage));
            return response;
        }
    }
}