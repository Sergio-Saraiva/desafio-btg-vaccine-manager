namespace VaccineManager.Application.Users.Queries;

public sealed record SignInQueryResponse(
    string Email,
    string AccessToken);