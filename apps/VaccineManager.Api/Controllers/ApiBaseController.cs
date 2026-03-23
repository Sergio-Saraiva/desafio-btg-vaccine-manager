using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Common.Responses;

namespace VaccineManager.Api.Controllers;

[ApiController]
public class ApiBaseController : ControllerBase
{
    private readonly ISender _sender;
    protected ApiBaseController(ISender sender)
    {
        _sender = sender;
    }
    
    protected async Task<IActionResult> SendRequest<TResponse>(
        IRequest<Result<TResponse>> request,
        int successStatusCode = StatusCodes.Status200OK)
    {
        var result = await _sender.Send(request);

        if (result.IsSuccess)
            return StatusCode(successStatusCode, ApiResponse.Success(result.Value));

        return HandleError(result);
    }

    protected async Task<IActionResult> SendRequest(
        IRequest<Result> request,
        int successStatusCode = StatusCodes.Status204NoContent)
    {
        var result = await _sender.Send(request);

        if (result.IsSuccess)
            return StatusCode(successStatusCode, ApiResponse.Success());

        return HandleError(result);
    }

    private IActionResult HandleError(ResultBase result)
    {
        var error = result.Errors.FirstOrDefault();

        if (error is ValidationApiError validationError)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, ApiResponse.Failure(
                string.Join("; ", validationError.Failures
                    .SelectMany(f => f.Value.Select(msg => $"{f.Key}: {msg}")))));
        }

        if (error is ApiError apiError)
        {
            return StatusCode((int)apiError.StatusCode, ApiResponse.Failure(apiError.Message));
        }

        return StatusCode(StatusCodes.Status500InternalServerError,
            ApiResponse.Failure("Internal Server Error"));
    }
}