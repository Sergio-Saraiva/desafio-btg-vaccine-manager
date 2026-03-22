using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public class ListPersonsQueryHandler : IQueryHandler<ListPersonsQuery, List<ListPersonsResponse>>
{
    private readonly IPersonRepository _personRepository;
    private readonly ISieveProcessor _sieveProcessor;

    public ListPersonsQueryHandler(IPersonRepository personRepository, ISieveProcessor sieveProcessor)
    {
        _personRepository = personRepository;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Result<List<ListPersonsResponse>>> Handle(ListPersonsQuery request, CancellationToken cancellationToken)
    {
        var query = _personRepository.GetQueryable();
        var filtered = _sieveProcessor.Apply(request.SieveModel, query);
        var persons = await filtered?.ToListAsync(cancellationToken: cancellationToken);
        return Result.Ok(persons.Select(p => new ListPersonsResponse(
            p.Id,
            p.Name,
            p.DocumentType.ToString(),
            p.DocumentNumber,
            p.Nationality
            )).ToList());
    }
}