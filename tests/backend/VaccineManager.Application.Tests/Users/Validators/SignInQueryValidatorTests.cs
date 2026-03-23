using FluentAssertions;
using VaccineManager.Application.Users.Queries;

namespace VaccineManager.Application.Tests.Users.Validators;

public class SignInQueryValidatorTests
{
    private readonly SignInQueryValidator _sut = new();

    [Fact]
    public async Task Validate_ValidQuery_ReturnsValid()
    {
        var query = new SignInQuery("john@email.com", "Password123");
        var result = await _sut.ValidateAsync(query);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyEmail_ReturnsError()
    {
        var query = new SignInQuery("", "Password123");
        var result = await _sut.ValidateAsync(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Validate_InvalidEmailFormat_ReturnsError()
    {
        var query = new SignInQuery("not-an-email", "Password123");
        var result = await _sut.ValidateAsync(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Validate_EmptyPassword_ReturnsError()
    {
        var query = new SignInQuery("john@email.com", "");
        var result = await _sut.ValidateAsync(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task Validate_BothFieldsEmpty_ReturnsMultipleErrors()
    {
        var query = new SignInQuery("", "");
        var result = await _sut.ValidateAsync(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }
}
