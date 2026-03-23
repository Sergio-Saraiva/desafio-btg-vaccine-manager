using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Vaccines.Commands.CreateVaccine;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Vaccines.Commands;

public class CreateVaccineCommandHandlerTests
{
    private readonly IVaccineRepository _vaccineRepository = Substitute.For<IVaccineRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateVaccineCommandHandler _sut;

    public CreateVaccineCommandHandlerTests()
    {
        _sut = new CreateVaccineCommandHandler(_vaccineRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_DuplicateCode_ReturnsFailWithConflict()
    {
        // Arrange
        var command = new CreateVaccineCommand("BCG", 1, "VAC-001");
        _vaccineRepository.GetByCodeAsync(command.Code!)
            .Returns(new Vaccine("Existing", 1, "VAC-001"));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Handle_ValidCommandWithCode_CreatesVaccineAndReturnsOk()
    {
        // Arrange
        var command = new CreateVaccineCommand("BCG", 1, "VAC-001");
        _vaccineRepository.GetByCodeAsync(Arg.Any<string>())
            .Returns((Vaccine?)null);
        _vaccineRepository.AddAsync(Arg.Any<Vaccine>())
            .Returns(ci => ci.Arg<Vaccine>());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("BCG");
        result.Value.RequiredDoses.Should().Be(1);
        result.Value.Code.Should().Be("VAC-001");
        await _vaccineRepository.Received(1).AddAsync(Arg.Any<Vaccine>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommandWithoutCode_GeneratesCodeAutomatically()
    {
        // Arrange
        var command = new CreateVaccineCommand("BCG", 1, null);
        _vaccineRepository.AddAsync(Arg.Any<Vaccine>())
            .Returns(ci => ci.Arg<Vaccine>());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("BCG");
        result.Value.Code.Should().StartWith("VAC-");
        await _vaccineRepository.Received(1).AddAsync(Arg.Any<Vaccine>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
