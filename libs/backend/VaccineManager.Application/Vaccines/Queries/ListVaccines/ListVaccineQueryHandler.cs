using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Vaccines.Queries.ListVaccines;

public class ListVaccineQueryHandler : IQueryHandler<ListVaccineQuery, List<ListVaccineResponse>>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly ISieveProcessor _sieveProcessor;

    public ListVaccineQueryHandler(IVaccineRepository vaccineRepository, ISieveProcessor sieveProcessor)
    {
        _vaccineRepository = vaccineRepository;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Result<List<ListVaccineResponse>>> Handle(ListVaccineQuery request, CancellationToken cancellationToken)
    {
        var queryable = _vaccineRepository.GetQueryable();
        var filtered = _sieveProcessor.Apply(request.SieveModel, queryable);
        var vaccines = await filtered?.ToListAsync(cancellationToken);

        return Result.Ok(vaccines
            .Select(v => new ListVaccineResponse(v.Id, v.Name, v.RequiredDoses, v.Code, v.CreatedAt)).ToList());
    }
}