using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.NSubstitute;
using NSubstitute;
using Sieve.Models;
using Sieve.Services;
using VaccineManager.Application.Vaccines.Queries.ListVaccines;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Vaccines.Queries;

public class ListVaccineQueryHandlerTests
{
    private readonly IVaccineRepository _vaccineRepository = Substitute.For<IVaccineRepository>();
    private readonly ISieveProcessor _sieveProcessor = Substitute.For<ISieveProcessor>();
    private readonly IOptions<SieveOptions> _sieveOptions = Options.Create(new SieveOptions { DefaultPageSize = 10 });
    private readonly ListVaccineQueryHandler _sut;

    public ListVaccineQueryHandlerTests()
    {
        _sut = new ListVaccineQueryHandler(_vaccineRepository, _sieveProcessor, _sieveOptions);
    }

    [Fact]
    public async Task Handle_NoVaccines_ReturnsEmptyPagedResponse()
    {
        // Arrange
        var vaccines = new List<Vaccine>();
        var mockQueryable = vaccines.BuildMockDbSet();

        _vaccineRepository.GetQueryable().Returns(mockQueryable);
        _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Vaccine>>(), applyPagination: false)
            .Returns(mockQueryable);
        _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Vaccine>>())
            .Returns(mockQueryable);

        var query = new ListVaccineQuery(new SieveModel { Page = 1, PageSize = 10 });

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEmpty();
        result.Value.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Handle_WithVaccines_ReturnsMappedPagedResponse()
    {
        // Arrange
        var vaccines = new List<Vaccine>
        {
            new("BCG", 1, "VAC-001"),
            new("Hepatitis B", 3, "VAC-002")
        };
        var mockQueryable = vaccines.BuildMockDbSet();

        _vaccineRepository.GetQueryable().Returns(mockQueryable);
        _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Vaccine>>(), applyPagination: false)
            .Returns(mockQueryable);
        _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Vaccine>>())
            .Returns(mockQueryable);

        var query = new ListVaccineQuery(new SieveModel { Page = 1, PageSize = 10 });

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(2);
        result.Value.TotalCount.Should().Be(2);
        result.Value.CurrentPage.Should().Be(1);
        result.Value.Items[0].Name.Should().Be("BCG");
        result.Value.Items[1].Name.Should().Be("Hepatitis B");
    }

    [Fact]
    public async Task Handle_DefaultsPagination_WhenSieveModelHasNoPageInfo()
    {
        // Arrange
        var vaccines = new List<Vaccine>();
        var mockQueryable = vaccines.BuildMockDbSet();

        _vaccineRepository.GetQueryable().Returns(mockQueryable);
        _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Vaccine>>(), applyPagination: false)
            .Returns(mockQueryable);
        _sieveProcessor.Apply(Arg.Any<SieveModel>(), Arg.Any<IQueryable<Vaccine>>())
            .Returns(mockQueryable);

        var query = new ListVaccineQuery(new SieveModel()); // no page/pageSize set

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CurrentPage.Should().Be(1);
        result.Value.PageSize.Should().Be(10); // from SieveOptions.DefaultPageSize
    }
}
