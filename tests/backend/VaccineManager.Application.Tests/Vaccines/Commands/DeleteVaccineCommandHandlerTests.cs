using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Vaccines.Commands.DeleteVaccine;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Vaccines.Commands;

public class DeleteVaccineCommandHandlerTests
{
    private readonly IVaccineRepository _vaccineRepository = Substitute.For<IVaccineRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteVaccineCommandHandler _sut;

    public DeleteVaccineCommandHandlerTests()
    {
        _sut = new DeleteVaccineCommandHandler(_vaccineRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_VaccineNotFound_ReturnsFailWithNotFoundError()
    {
        // Arrange
        var command = new DeleteVaccineCommand(Guid.NewGuid());
        _vaccineRepository.GetByIdAsync(command.Id)
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
    public async Task Handle_VaccineExists_DeletesAndReturnsOk()
    {
        // Arrange
        var vaccine = new Vaccine("BCG", 1, "VAC-001");
        var command = new DeleteVaccineCommand(vaccine.Id);
        _vaccineRepository.GetByIdAsync(command.Id)
            .Returns(vaccine);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _vaccineRepository.Received(1).Delete(vaccine);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
