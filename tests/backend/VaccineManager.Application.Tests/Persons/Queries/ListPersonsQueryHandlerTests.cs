using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using NSubstitute;
using Sieve.Models;
using Sieve.Services;
using VaccineManager.Application.Persons.Queries.ListPersons;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Persons.Queries;

public class ListPersonsQueryHandlerTests
{
    private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();
      private readonly ISieveProcessor _sieveProcessor = Substitute.For<ISieveProcessor>();
      private readonly IOptions<SieveOptions> _sieveOptions = Options.Create(new SieveOptions { DefaultPageSize = 10 });      
      private readonly ListPersonsQueryHandler _sut;
                                                                                                                              
      public ListPersonsQueryHandlerTests()
      {                                                                                                                       
          _sut = new ListPersonsQueryHandler(_personRepository, _sieveProcessor, _sieveOptions);
      }

      [Fact]
      public async Task Handle_NoPersons_ReturnsEmptyPagedResponse()
      {                          
          //Arrange
          var persons = new List<Person>();
          var mockQueryable = persons.BuildMockDbSet();                                                       
                                                                                                                              
          _personRepository.GetQueryable().Returns(mockQueryable);
          _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Person>>(), applyPagination: false)                 
              .Returns(mockQueryable);                                                                                        
          _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Person>>())                                         
              .Returns(mockQueryable);                                                                                        
                                                                                                                              
          var query = new ListPersonsQuery(new SieveModel { Page = 1, PageSize = 10 });                                       
  
          // Act
          var result = await _sut.Handle(query, CancellationToken.None);                                                      
                
          // Assert
          result.IsSuccess.Should().BeTrue();
          result.Value.Items.Should().BeEmpty();
          result.Value.TotalCount.Should().Be(0);
      }                                                                                                                       
  
      [Fact]                                                                                                                  
      public async Task Handle_WithPersons_ReturnsMappedPagedResponse()
      {
          // Arrange
          var persons = new List<Person>
          {                                                                                                                   
              new("John", DocumentType.Cpf, "12345678901", "BR"),
              new("Jane", DocumentType.Passport, "AB1234", "US")                                                              
          };
          var mockQueryable = persons.BuildMockDbSet();
                                                                                                                              
          _personRepository.GetQueryable().Returns(mockQueryable);
          _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Person>>(), applyPagination: false)                 
              .Returns(mockQueryable);                                                                                        
          _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Person>>())
              .Returns(mockQueryable);                                                                                        
                                                                                                                              
          var query = new ListPersonsQuery(new SieveModel { Page = 1, PageSize = 10 });
                        
          // Act
          var result = await _sut.Handle(query, CancellationToken.None);

          // Assert
          result.IsSuccess.Should().BeTrue();                                                                                 
          result.Value.Items.Should().HaveCount(2);
          result.Value.TotalCount.Should().Be(2);                                                                             
          result.Value.CurrentPage.Should().Be(1);
          result.Value.Items[0].Name.Should().Be("John");
          result.Value.Items[1].Name.Should().Be("Jane");                                                                     
      }
                                                                                                                              
      [Fact]      
      public async Task Handle_DefaultsPagination_WhenSieveModelHasNoPageInfo()
      {            
          // Arrange
          var persons = new List<Person>();
          var mockQueryable = persons.BuildMockDbSet();                                                          
                                                                                                                              
          _personRepository.GetQueryable().Returns(mockQueryable);
          _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Person>>(), applyPagination: false)                 
              .Returns(mockQueryable);
          _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Person>>())                                         
              .Returns(mockQueryable);
                                                                                                                              
          var query = new ListPersonsQuery(new SieveModel()); // no page/pageSize set                                         
  
          // Act
          var result = await _sut.Handle(query, CancellationToken.None);                                                      
                
          // Assert
          result.IsSuccess.Should().BeTrue();                                                                                 
          result.Value.CurrentPage.Should().Be(1);
          result.Value.PageSize.Should().Be(10); // from SieveOptions.DefaultPageSize                                         
      }
}