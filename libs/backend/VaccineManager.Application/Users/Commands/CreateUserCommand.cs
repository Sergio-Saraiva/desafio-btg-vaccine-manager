using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Users.Commands;

public sealed record CreateUserCommand(string Email, string Password, string PasswordConfirmation) : ICommand<CreateUserResponse>;