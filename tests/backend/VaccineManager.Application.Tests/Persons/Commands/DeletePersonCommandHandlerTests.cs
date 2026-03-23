using FluentAssertions;
using FluentResults;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Persons.Commands.DeletePerson;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Persons.Commands;

public class DeletePersonCommandHandlerTests
{
    private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
      private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
      private readonly DeletePersonCommandHandler _sut;                                                                       
   
      public DeletePersonCommandHandlerTests()                                                                                
      {           
          _sut = new DeletePersonCommandHandler(_personRepository, _unitOfWork);
      }                                                                                                                       
   
      [Fact]                                                                                                                  
      public async Task Handle_PersonNotFound_ReturnsFailWithNotFoundError()
      {             
          // Arrange
          var command = new DeletePersonCommand(Guid.NewGuid());
          _personRepository.GetByIdAsync(command.Id)                                                                          
              .Returns((Person?)null);

          // Act
          var result = await _sut.Handle(command, CancellationToken.None);                                                    
          
          // Assert
          result.IsFailed.Should().BeTrue();                                                                                  
          result.Errors.Should().ContainSingle()
              .Which.Should().BeOfType<ApiError>();
      }                                                                                                                       
   
      [Fact]                                                                                                                  
      public async Task Handle_PersonExists_DeletesAndReturnsOk()
      {
          // Arrange
          var person = new Person("John", DocumentType.Cpf, "12345678901", "BR");
          var command = new DeletePersonCommand(person.Id);                                                                   
          _personRepository.GetByIdAsync(command.Id)
              .Returns(person);                                                                                               
          
          // Act
          var result = await _sut.Handle(command, CancellationToken.None);                                                    
   
          // Assert
          result.IsSuccess.Should().BeTrue();                                                                                 
          _personRepository.Received(1).Delete(person);
          await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
      }                              
}