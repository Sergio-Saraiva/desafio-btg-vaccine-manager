using FluentAssertions;
using VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;

namespace VaccineManager.Application.Tests.VaccinationRecords.Validators;

public class CreateVaccinationRecordCommandValidatorTests
{
    private readonly CreateVaccinationRecordCommandValidator _sut = new();

    [Fact]
    public async Task Validate_ValidRecord_ReturnsValid()
    {
        // Arrange
        var record = CreateRecord(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(record);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_EmptyPersonId_ReturnsError()
    {
        // Arrange
        var record = CreateRecord(Guid.Empty, Guid.NewGuid());

        // Act
        var result = await _sut.ValidateAsync(record);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PersonId");
    }

    [Fact]
    public async Task Validate_EmptyVaccineId_ReturnsError()
    {
        // Arrange
        var record = CreateRecord(Guid.NewGuid(), Guid.Empty);

        // Act
        var result = await _sut.ValidateAsync(record);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "VaccineId");
    }
    
    [Fact]
    public async Task Validate_BothIdsEmpty_ReturnsMultipleErrors()
    {
        // Arrange
        var record = CreateRecord(Guid.Empty, Guid.Empty);

        // Act
        var result = await _sut.ValidateAsync(record);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().Contain(e => e.PropertyName == "PersonId");
        result.Errors.Should().Contain(e => e.PropertyName == "VaccineId");
    }

    private static Domain.Entities.VaccinationRecord CreateRecord(Guid personId, Guid vaccineId)
    {
        return new Domain.Entities.VaccinationRecord
        (
            personId,
            vaccineId
        );
    }
}