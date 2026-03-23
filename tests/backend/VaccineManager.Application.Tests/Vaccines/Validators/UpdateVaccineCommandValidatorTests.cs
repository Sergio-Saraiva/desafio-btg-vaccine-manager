using FluentAssertions;
using VaccineManager.Application.Vaccines.Commands.UpdateVaccine;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Application.Tests.Vaccines.Validators;

public class UpdateVaccineCommandValidatorTests
{
    private readonly UpdateVaccineCommandValidator _sut = new();
    
    [Fact]
    public async Task Validate_ValidVaccine_ReturnsValid()
    {
        var vaccine = new UpdateVaccineCommand(Guid.NewGuid(),"BCG", 1);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyName_ReturnsError()
    {
        var vaccine = new UpdateVaccineCommand(Guid.NewGuid(),string.Empty, 1);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public async Task Validate_NameExceeds250_ReturnsError()
    {
        var vaccine = new UpdateVaccineCommand(Guid.NewGuid(),new string('A', 251), 1);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }
    
    [Fact]
    public async Task Validate_RequiredDosesIsZero_ReturnsError()
    {
     
        var vaccine = new UpdateVaccineCommand(Guid.NewGuid(),"BCG", 1);
        SetRequiredDoses(vaccine, 0);
        var result = await _sut.ValidateAsync(vaccine);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "RequiredDoses");
    }

    private static void SetRequiredDoses(UpdateVaccineCommand vaccine, int value)
    {
        typeof(UpdateVaccineCommand).GetProperty(nameof(UpdateVaccineCommand.RequiredDoses))!
            .SetValue(vaccine, value);
    }
}
