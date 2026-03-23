using FluentAssertions;
using VaccineManager.Application.Persons.Commands.UpdatePerson;
using VaccineManager.Domain.Enums;

namespace VaccineManager.Application.Tests.Persons.Validators;

public class UpdatePersonCommandValidatorTests
{
    private readonly UpdatePersonCommandValidator _sut = new();
    
    [Fact]
    public async Task Validate_EmptyName_ReturnsError()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "", DocumentType.Cpf, "52998224725", "BR");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }
    
    [Fact]
    public async Task Validate_ValidCpf_ReturnsValid()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "John", DocumentType.Cpf, "52998224725", "BR");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_CpfWithWrongLength_ReturnsError()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "John", DocumentType.Cpf, "1234567", "BR");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DocumentNumber");
    }

    [Fact]
    public async Task Validate_CpfAllSameDigits_ReturnsError()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "John", DocumentType.Cpf, "11111111111", "BR");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DocumentNumber");
    }

    [Fact]
    public async Task Validate_CpfInvalidCheckDigit_ReturnsError()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "John", DocumentType.Cpf, "52998224720", "BR");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DocumentNumber");
    }
    
    [Fact]
    public async Task Validate_ValidPassport_ReturnsValid()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "Jane", DocumentType.Passport, "AB123456", "US");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_PassportTooShort_ReturnsError()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "Jane", DocumentType.Passport, "AB12", "US");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DocumentNumber");
    }

    [Fact]
    public async Task Validate_PassportWithSpecialChars_ReturnsError()
    {
        var command = new UpdatePersonCommand(Guid.NewGuid(), "Jane", DocumentType.Passport, "AB-1234!", "US");
        var result = await _sut.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DocumentNumber");
    }
}
