using System.Net;

namespace VaccineManager.Application.Common.Errors;

public class ValidationApiError : ApiError
{
    public IDictionary<string, string[]> Failures { get; }
    public ValidationApiError(IDictionary<string, string[]> failures) 
        : base(HttpStatusCode.UnprocessableEntity, "One or more validation errors occurred.")
    {
        Failures = failures;
    }
}