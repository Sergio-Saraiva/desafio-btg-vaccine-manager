using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.VaccinationRecord.DeleteVaccinationRecord;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.VaccinationRecords.Commands;

public class DeleteVaccinationRecordCommandHandlerTests
{
    private readonly IVaccinationRecordRepository _vaccinationRecordRepository = Substitute.For<IVaccinationRecordRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteVaccinationRecordCommandHandler _sut;

    public DeleteVaccinationRecordCommandHandlerTests()
    {
        _sut = new DeleteVaccinationRecordCommandHandler(_vaccinationRecordRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_RecordNotFound_ReturnsFailWithNotFoundError()
    {
        // Arrange
        var command = new DeleteVaccinationRecordCommand(Guid.NewGuid());
        _vaccinationRecordRepository.GetByIdAsync(command.Id)
            .Returns((VaccineManager.Domain.Entities.VaccinationRecord?)null);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handle_RecordExists_DeletesAndReturnsOk()
    {
        // Arrange
        var record = new VaccineManager.Domain.Entities.VaccinationRecord(Guid.NewGuid(), Guid.NewGuid());
        var command = new DeleteVaccinationRecordCommand(record.Id);
        _vaccinationRecordRepository.GetByIdAsync(command.Id)
            .Returns(record);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _vaccinationRecordRepository.Received(1).Delete(record);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
