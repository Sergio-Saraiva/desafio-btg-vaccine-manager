using System.Net;
using FluentResults;

namespace VaccineManager.Application.Common.Errors;

public class ApiError : Error
{
    public HttpStatusCode StatusCode { get; set; }

    public ApiError(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
        Metadata.Add("StatusCode", (int)statusCode);
    }
}