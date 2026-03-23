using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Responses;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Vaccines.Queries.ListVaccines;

public class ListVaccineQueryHandler : IQueryHandler<ListVaccineQuery, PagedResponse<ListVaccineResponse>>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly ISieveProcessor _sieveProcessor;
    private readonly SieveOptions _sieveOptions;

    public ListVaccineQueryHandler(
        IVaccineRepository vaccineRepository,
        ISieveProcessor sieveProcessor,
        IOptions<SieveOptions> sieveOptions)
    {
        _vaccineRepository = vaccineRepository;
        _sieveProcessor = sieveProcessor;
        _sieveOptions = sieveOptions.Value;
    }

    public async Task<Result<PagedResponse<ListVaccineResponse>>> Handle(ListVaccineQuery request, CancellationToken cancellationToken)
    {
        var queryable = _vaccineRepository.GetQueryable();

        var filteredQuery = _sieveProcessor.Apply(request.SieveModel, queryable, applyPagination: false);
        var totalCount = await filteredQuery.CountAsync(cancellationToken);

        var paginatedQuery = _sieveProcessor.Apply(request.SieveModel, queryable);
        var vaccines = await paginatedQuery.ToListAsync(cancellationToken);

        var page = request.SieveModel.Page ?? 1;
        var pageSize = request.SieveModel.PageSize ?? _sieveOptions.DefaultPageSize;
        var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalCount / (double)pageSize) : 1;

        var items = vaccines
            .Select(v => new ListVaccineResponse(v.Id, v.Name, v.RequiredDoses, v.Code, v.CreatedAt))
            .ToList();

        return Result.Ok(new PagedResponse<ListVaccineResponse>(items, page, pageSize, totalCount, totalPages));
    }
}