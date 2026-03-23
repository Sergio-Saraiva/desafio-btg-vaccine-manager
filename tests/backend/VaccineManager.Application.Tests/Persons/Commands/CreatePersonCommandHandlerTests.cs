using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Persons.Commands.CreatePerson;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Persons.Commands;

public class CreatePersonCommandHandlerTests
{
    private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreatePersonCommandHandler _sut;

    public CreatePersonCommandHandlerTests()
    {
        _sut = new CreatePersonCommandHandler(_personRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_DuplicateDocument_ReturnsFailWithConflict()
    {
        // Arrange
        var command = new CreatePersonCommand("John", DocumentType.Cpf, "12345678901", null);
        _personRepository.GetByDocumentAsync(command.DocumentType, command.DocumentNumber)
            .Returns(new Person("Existing", DocumentType.Cpf, "12345678901", "BR"));

        // Act                                                                                                          
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesPersonAndReturnsOk()
    {
        // Arrange
        var command = new CreatePersonCommand("John", DocumentType.Cpf, "12345678901", "BR");
        _personRepository.GetByDocumentAsync(Arg.Any<DocumentType>(), Arg.Any<string>())
            .Returns((Person?)null);

        // Act                                                                                                              
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("John");
        await _personRepository.Received(1).AddAsync(Arg.Any<Person>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}