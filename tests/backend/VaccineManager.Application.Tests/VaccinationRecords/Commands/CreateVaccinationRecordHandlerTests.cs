using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;
using Person = VaccineManager.Domain.Entities.Person;
using Vaccine = VaccineManager.Domain.Entities.Vaccine;

namespace VaccineManager.Application.Tests.VaccinationRecords.Commands;

public class CreateVaccinationRecordHandlerTests
{
    private readonly IVaccineRepository _vaccineRepository = Substitute.For<IVaccineRepository>();
    private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
    private readonly IVaccinationRecordRepository _vaccinationRecordRepository = Substitute.For<IVaccinationRecordRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateVaccinationRecordHandler _sut;

    public CreateVaccinationRecordHandlerTests()
    {
        _sut = new CreateVaccinationRecordHandler(_vaccineRepository, _personRepository, _vaccinationRecordRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_PersonNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = new CreateVaccinationRecordCommand(Guid.NewGuid(), Guid.NewGuid());
        _personRepository.GetByIdAsync(command.PersonId)
            .Returns((Person?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_VaccineNotFound_ReturnsNotFound()
    {
        // Arrange
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        var command = new CreateVaccinationRecordCommand(person.Id, Guid.NewGuid());
        _personRepository.GetByIdAsync(person.Id)
            .Returns(person);
        _vaccineRepository.GetByIdAsync(command.VaccineId)
            .Returns((Vaccine?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_MaxDosesReached_ReturnsConflict()
    {
        // Arrange
        var vaccine = new Vaccine("BCG", 1, "VAC-001");
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        person.VaccinationRecords.Add(new VaccineManager.Domain.Entities.VaccinationRecord(person.Id, vaccine.Id));

        var command = new CreateVaccinationRecordCommand(person.Id, vaccine.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);
        _vaccineRepository.GetByIdAsync(vaccine.Id).Returns(vaccine);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesRecordAndReturnsOk()
    {
        // Arrange
        var vaccine = new Vaccine("BCG", 2, "VAC-001");
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        var command = new CreateVaccinationRecordCommand(person.Id, vaccine.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);
        _vaccineRepository.GetByIdAsync(vaccine.Id).Returns(vaccine);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.PersonName.Should().Be("John");
        result.Value.VaccineName.Should().Be("BCG");
        result.Value.VaccineCode.Should().Be("VAC-001");
        result.Value.DoseNumber.Should().Be(1);
        await _vaccinationRecordRepository.Received(1).AddAsync(Arg.Any<VaccineManager.Domain.Entities.VaccinationRecord>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SecondDose_ReturnsDoseNumber2()
    {
        // Arrange
        var vaccine = new Vaccine("Hepatitis B", 3, "VAC-002");
        var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
        person.VaccinationRecords.Add(new VaccineManager.Domain.Entities.VaccinationRecord(person.Id, vaccine.Id));

        var command = new CreateVaccinationRecordCommand(person.Id, vaccine.Id);
        _personRepository.GetByIdAsync(person.Id).Returns(person);
        _vaccineRepository.GetByIdAsync(vaccine.Id).Returns(vaccine);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.DoseNumber.Should().Be(2);
    }
}
