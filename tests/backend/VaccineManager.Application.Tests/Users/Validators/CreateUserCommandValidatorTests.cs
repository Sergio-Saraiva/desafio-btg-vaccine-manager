using FluentAssertions;
using VaccineManager.Application.Users.Commands;

namespace VaccineManager.Application.Tests.Users.Validators;

public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _sut = new();

    [Fact]
    public async Task Validate_ValidCommand_ReturnsValid()
    {
        var command = new CreateUserCommand("john@email.com", "Password123", "Password123");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyEmail_ReturnsError()
    {
        var command = new CreateUserCommand("", "Password123", "Password123");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Validate_InvalidEmailFormat_ReturnsError()
    {
        var command = new CreateUserCommand("not-an-email", "Password123", "Password123");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Validate_EmptyPassword_ReturnsError()
    {
        var command = new CreateUserCommand("john@email.com", "", "");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task Validate_PasswordTooShort_ReturnsError()
    {
        var command = new CreateUserCommand("john@email.com", "Short1", "Short1");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task Validate_PasswordConfirmationDoesNotMatch_ReturnsError()
    {
        var command = new CreateUserCommand("john@email.com", "Password123", "DifferentPassword");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PasswordConfirmation");
    }
}
