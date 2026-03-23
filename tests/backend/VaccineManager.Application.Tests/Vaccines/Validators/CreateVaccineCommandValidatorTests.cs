using FluentAssertions;
using VaccineManager.Application.Vaccines.Commands.CreateVaccine;

namespace VaccineManager.Application.Tests.Vaccines.Validators;

public class CreateVaccineCommandValidatorTests
{
    private readonly CreateVaccineCommandValidator _sut = new();
    
    [Fact]
    public async Task Validate_ValidVaccine_ReturnsValid()
    {
        var vaccine = new CreateVaccineCommand("BCG", 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyName_ReturnsError()
    {
        var vaccine = new CreateVaccineCommand("", 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_NameExceeds250_ReturnsError()
    {
        var vaccine = new CreateVaccineCommand(new string('A', 251), 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }
    
    [Fact]
    public async Task Validate_RequiredDosesIsZero_ReturnsError()
    {
        var vaccine = new CreateVaccineCommand("BCG", 1, "VAC-001");
        SetRequiredDoses(vaccine, 0);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "RequiredDoses");
    }
    
    [Fact]
    public async Task Validate_ValidCode_ReturnsValid()
    {
        var vaccine = new CreateVaccineCommand("BCG", 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_NullCode_ReturnsValid()
    {
        var vaccine = new CreateVaccineCommand("BCG", 1, null);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_CodeExceeds20_ReturnsError()
    {
        var vaccine = new CreateVaccineCommand("BCG", 1, new string('A', 21));
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Code");
    }

    private static void SetRequiredDoses(CreateVaccineCommand vaccine, int value)
    {
        typeof(CreateVaccineCommand).GetProperty(nameof(CreateVaccineCommand.RequiredDoses))!
            .SetValue(vaccine, value);
    }
}
