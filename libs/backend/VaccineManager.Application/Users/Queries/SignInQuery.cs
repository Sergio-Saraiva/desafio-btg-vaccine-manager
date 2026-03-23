using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Users.Queries;

public record SignInQuery(string Email, string Password) : IQuery<SignInQueryResponse>;