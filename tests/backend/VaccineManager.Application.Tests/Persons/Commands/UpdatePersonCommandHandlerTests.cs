using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Persons.Commands.CreatePerson;
using VaccineManager.Application.Persons.Commands.UpdatePerson;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Persons.Commands;

public class UpdatePersonCommandHandlerTests
{
    private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdatePersonCommandHandler _sut;

    public UpdatePersonCommandHandlerTests()
    {
        _sut = new UpdatePersonCommandHandler(_personRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_PersonNotFound_ReturnsNotFound()
    {
        // Arrange
        var personGuid = Guid.NewGuid();
        var command = new UpdatePersonCommand(personGuid, "John", DocumentType.Cpf, "12345678901", "BR");
        _personRepository.GetByIdAsync(personGuid)
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
    public async Task Handle_PersonFoundSameDocument_ReturnsConflict()
    {
        // Arrange
        var person = new Person("Foo", DocumentType.Cpf, "12345678901", "BR");
        var command = new UpdatePersonCommand(person.Id, person.Name, person.DocumentType, person.DocumentNumber, person.Nationality);
        _personRepository.GetByIdAsync(person.Id)
            .Returns(person);
        _personRepository.GetByDocumentAsync(command.DocumentType, command.DocumentNumber)
            .Returns(new Person("Bar", command.DocumentType, command.DocumentNumber, "BR"));
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Handle_PersonFoundDifferentDocument_ReturnsSuccess()
    {
        // Arrange
        var person = new Person("Foo", DocumentType.Cpf, "12345678901", "BR");
        var command = new UpdatePersonCommand(person.Id, person.Name, person.DocumentType, person.DocumentNumber, person.Nationality);
        _personRepository.GetByIdAsync(person.Id)
            .Returns(person);
        _personRepository.GetByDocumentAsync(command.DocumentType, command.DocumentNumber)
            .Returns((Person?)null);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Foo");
        await _personRepository.Received(1).UpdateAsync(Arg.Any<Person>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}