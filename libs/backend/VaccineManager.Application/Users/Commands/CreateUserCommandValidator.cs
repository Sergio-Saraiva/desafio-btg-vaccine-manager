using FluentValidation;

namespace VaccineManager.Application.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email format is required.");

        // 2. Validate the Password (minimum 8 characters)
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        // 3. Validate the Password Confirmation (must match Password)
        RuleFor(x => x.PasswordConfirmation)
            .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
    }
}