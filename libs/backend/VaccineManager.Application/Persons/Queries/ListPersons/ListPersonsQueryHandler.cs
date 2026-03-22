using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public class ListPersonsQueryHandler : IQueryHandler<ListPersonsQuery, PagedResponse<ListPersonsResponse>>
{
    private readonly IPersonRepository _personRepository;
    private readonly ISieveProcessor _sieveProcessor;
    private readonly SieveOptions _sieveOptions;

    public ListPersonsQueryHandler(
        IPersonRepository personRepository,
        ISieveProcessor sieveProcessor,
        IOptions<SieveOptions> sieveOptions)
    {
        _personRepository = personRepository;
        _sieveProcessor = sieveProcessor;
        _sieveOptions = sieveOptions.Value;
    }

    public async Task<Result<PagedResponse<ListPersonsResponse>>> Handle(ListPersonsQuery request, CancellationToken cancellationToken)
    {
        var query = _personRepository.GetQueryable();

        var filteredQuery = _sieveProcessor.Apply(request.SieveModel, query, applyPagination: false);
        var totalCount = await filteredQuery.CountAsync(cancellationToken);

        var paginatedQuery = _sieveProcessor.Apply(request.SieveModel, query);
        var persons = await paginatedQuery.ToListAsync(cancellationToken);

        var page = request.SieveModel.Page ?? 1;
        var pageSize = request.SieveModel.PageSize ?? _sieveOptions.DefaultPageSize;
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 1;

        var items = persons.Select(p => new ListPersonsResponse(
            p.Id,
            p.Name,
            p.DocumentType.ToString(),
            p.DocumentNumber,
            p.Nationality
        )).ToList();

        return Result.Ok(new PagedResponse<ListPersonsResponse>(items, page, pageSize, totalCount, totalPages));
    }
}