using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Persons.Queries.GetPersonVaccinationCard;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Persons.Queries;

public class GetPersonVaccinationCardQueryHandlerTests
{
    private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
    private readonly GetPersonVaccinationCardQueryHandler _sut;

    public GetPersonVaccinationCardQueryHandlerTests()
    {
        _sut = new GetPersonVaccinationCardQueryHandler(_personRepository);
    }

    [Fact]
    public async Task Handle_PersonNotFound_ReturnsNotFound()
    {
        // Arrange
        var query = new GetPersonVaccinationCardQuery(Guid.NewGuid());
        _personRepository.GetByIdAsync(query.Id)
            .Returns((Person?)null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_PersonWithNoRecords_ReturnsEmptyVaccinesList()
    {
        // Arrange
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        var query = new GetPersonVaccinationCardQuery(person.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.PersonId.Should().Be(person.Id);
        result.Value.PersonName.Should().Be("John");
        result.Value.DocumentType.Should().Be("Cpf");
        result.Value.DocumentNumber.Should().Be("12345678901");
        result.Value.Vaccines.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_PersonWithRecords_ReturnsGroupedVaccinationCard()
    {
        // Arrange
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        var vaccine = new Vaccine("BCG", 2, "VAC-001");

        var record1 = new Domain.Entities.VaccinationRecord(person.Id, vaccine.Id, new DateTime(2025, 1, 1));
        SetNavigationProperty(record1, vaccine);
        var record2 = new Domain.Entities.VaccinationRecord(person.Id, vaccine.Id, new DateTime(2025, 6, 1));
        SetNavigationProperty(record2, vaccine);

        person.VaccinationRecords.Add(record1);
        person.VaccinationRecords.Add(record2);

        var query = new GetPersonVaccinationCardQuery(person.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Vaccines.Should().HaveCount(1);

        var entry = result.Value.Vaccines[0];
        entry.VaccineId.Should().Be(vaccine.Id);
        entry.VaccineName.Should().Be("BCG");
        entry.VaccineCode.Should().Be("VAC-001");
        entry.RequiredDoses.Should().Be(2);
        entry.DosesTaken.Should().Be(2);
        entry.IsComplete.Should().BeTrue();
        entry.Doses.Should().HaveCount(2);
        entry.Doses[0].ApplicationDate.Should().Be(new DateTime(2025, 1, 1));
        entry.Doses[1].ApplicationDate.Should().Be(new DateTime(2025, 6, 1));
    }

    [Fact]
    public async Task Handle_IncompleteVaccination_ReturnsIsCompleteFalse()
    {
        // Arrange
        var person = new Person("Jane", DocumentType.Passport, "AB1234", "US");
        var vaccine = new Vaccine("Hepatitis B", 3, "VAC-002");

        var record = new Domain.Entities.VaccinationRecord(person.Id, vaccine.Id, new DateTime(2025, 3, 1));
        SetNavigationProperty(record, vaccine);
        person.VaccinationRecords.Add(record);

        var query = new GetPersonVaccinationCardQuery(person.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var entry = result.Value.Vaccines[0];
        entry.DosesTaken.Should().Be(1);
        entry.RequiredDoses.Should().Be(3);
        entry.IsComplete.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_MultipleVaccines_ReturnsGroupedByVaccine()
    {
        // Arrange
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        var bcg = new Vaccine("BCG", 1, "VAC-001");
        var hepB = new Vaccine("Hepatitis B", 3, "VAC-002");

        var bcgRecord = new Domain.Entities.VaccinationRecord(person.Id, bcg.Id, new DateTime(2025, 1, 1));
        SetNavigationProperty(bcgRecord, bcg);
        person.VaccinationRecords.Add(bcgRecord);

        var hepRecord = new Domain.Entities.VaccinationRecord(person.Id, hepB.Id, new DateTime(2025, 2, 1));
        SetNavigationProperty(hepRecord, hepB);
        person.VaccinationRecords.Add(hepRecord);

        var query = new GetPersonVaccinationCardQuery(person.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Vaccines.Should().HaveCount(2);
        result.Value.Vaccines.Should().Contain(v => v.VaccineName == "BCG" && v.IsComplete);
        result.Value.Vaccines.Should().Contain(v => v.VaccineName == "Hepatitis B" && !v.IsComplete);
    }

    private static void SetNavigationProperty(Domain.Entities.VaccinationRecord record, Vaccine vaccine)
    {
        var prop = typeof(Domain.Entities.VaccinationRecord).GetProperty(nameof(Domain.Entities.VaccinationRecord.Vaccine))!;
        prop.SetValue(record, vaccine);
    }
}
