using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccineManager.Application.Common.Errors;

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
        {
            return StatusCode(successStatusCode, result.Value);
        }

        var error = result.Errors.FirstOrDefault();

        if (error is ValidationApiError validationError)
        {
            return StatusCode(StatusCodes.Status422UnprocessableEntity, new ProblemDetails
            {
                Title = "Validation Error",
                Status = StatusCodes.Status422UnprocessableEntity,
                Extensions = { { "errors", validationError.Failures } }
            });
        }

        if (error is ApiError apiError)
        {
            return StatusCode((int)apiError.StatusCode, new ProblemDetails
            {
                Title = apiError.Message,
                Status = (int)apiError.StatusCode,
                Detail = apiError.Message
            });
        }

        return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
        {
            Title = "Internal Server Error",
            Status = StatusCodes.Status500InternalServerError
        });
    }
}