using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Vaccines.Commands.UpdateVaccine;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Vaccines.Commands;

public class UpdateVaccineCommandHandlerTests
{
    private readonly IVaccineRepository _vaccineRepository = Substitute.For<IVaccineRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateVaccineCommandHandler _sut;

    public UpdateVaccineCommandHandlerTests()
    {
        _sut = new UpdateVaccineCommandHandler(_vaccineRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_VaccineNotFound_ReturnsNotFound()
    {
        // Arrange
        var vaccineId = Guid.NewGuid();
        var command = new UpdateVaccineCommand(vaccineId, "BCG", 2);
        _vaccineRepository.GetByIdAsync(vaccineId)
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
    public async Task Handle_VaccineFound_UpdatesAndReturnsSuccess()
    {
        // Arrange
        var vaccine = new Vaccine("BCG", 1, "VAC-001");
        var command = new UpdateVaccineCommand(vaccine.Id, "BCG Updated", 3);
        _vaccineRepository.GetByIdAsync(vaccine.Id)
            .Returns(vaccine);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("BCG Updated");
        result.Value.RequiredDoses.Should().Be(3);
        result.Value.Code.Should().Be("VAC-001");
        await _vaccineRepository.Received(1).UpdateAsync(vaccine);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
