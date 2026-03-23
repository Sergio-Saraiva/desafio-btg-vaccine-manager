using FluentAssertions;
using VaccineManager.Application.Vaccines.Commands.CreateVaccine;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Application.Tests.Vaccines.Validators;

public class CreateVaccineCommandValidatorTests
{
    private readonly CreateVaccineCommandValidator _sut = new();
    
    [Fact]
    public async Task Validate_ValidVaccine_ReturnsValid()
    {
        var vaccine = new Vaccine("BCG", 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyName_ReturnsError()
    {
        var vaccine = new Vaccine("", 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_NameExceeds250_ReturnsError()
    {
        var vaccine = new Vaccine(new string('A', 251), 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }
    
    [Fact]
    public async Task Validate_RequiredDosesIsZero_ReturnsError()
    {
        var vaccine = new Vaccine("BCG", 1);
        SetRequiredDoses(vaccine, 0);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "RequiredDoses");
    }
    
    [Fact]
    public async Task Validate_ValidCode_ReturnsValid()
    {
        var vaccine = new Vaccine("BCG", 1, "VAC-001");
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_NullCode_ReturnsValid()
    {
        var vaccine = new Vaccine("BCG", 1, null);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_CodeExceeds20_ReturnsError()
    {
        var vaccine = new Vaccine("BCG", 1, new string('A', 21));
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Code");
    }

    private static void SetRequiredDoses(Vaccine vaccine, int value)
    {
        typeof(Vaccine).GetProperty(nameof(Vaccine.RequiredDoses))!
            .SetValue(vaccine, value);
    }
}
