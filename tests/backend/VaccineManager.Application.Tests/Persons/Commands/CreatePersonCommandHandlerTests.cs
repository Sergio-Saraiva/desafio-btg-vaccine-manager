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
    public async Task Handle_DuplicateActiveDocument_ReturnsFailWithConflict()
    {
        // Arrange
        var command = new CreatePersonCommand("John", DocumentType.Cpf, "12345678901", null);
        var existingActivePerson = new Person("Existing", DocumentType.Cpf, "12345678901", "BR");
        
        _personRepository.GetByDocumentIncludingDeletedAsync(command.DocumentType, Arg.Any<string>())
            .Returns(existingActivePerson);

        // Act                                                                                                          
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
            
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesPersonAndReturnsOk()
    {
        // Arrange
        var command = new CreatePersonCommand("John", DocumentType.Cpf, "12345678901", "BR");
        
        _personRepository.GetByDocumentIncludingDeletedAsync(Arg.Any<DocumentType>(), Arg.Any<string>())
            .Returns((Person?)null);

        // Act                                                                                                              
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Name.Should().Be("John");
        result.Value.DocumentNumber.Should().Be("12345678901");
        
        await _personRepository.Received(1).AddAsync(Arg.Any<Person>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DeletedPersonExists_ReactivatesAndReturnsOk()
    {
        // Arrange
        var command = new CreatePersonCommand("John", DocumentType.Cpf, "12345678901", "BR");
        
        var softDeletedPerson = new Person("Old Name", DocumentType.Cpf, "12345678901", "BR");
        softDeletedPerson.Delete(); 
        
        _personRepository.GetByDocumentIncludingDeletedAsync(command.DocumentType, Arg.Any<string>())
            .Returns(softDeletedPerson);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        
        softDeletedPerson.IsDeleted.Should().BeFalse();
        softDeletedPerson.DeletedAt.Should().BeNull();

        await _personRepository.Received(1).UpdateAsync(softDeletedPerson);
        await _personRepository.DidNotReceive().AddAsync(Arg.Any<Person>());
        
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}