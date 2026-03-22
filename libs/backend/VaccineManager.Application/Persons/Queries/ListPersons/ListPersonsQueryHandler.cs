using FluentResults;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public class ListPersonsQueryHandler : IQueryHandler<ListPersonsQuery, List<ListPersonsResponse>>
{
    private readonly IPersonRepository _personRepository;

    public ListPersonsQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Result<List<ListPersonsResponse>>> Handle(ListPersonsQuery request, CancellationToken cancellationToken)
    {
        var persons = await _personRepository.GetAllAsync();
        return Result.Ok(persons.Select(p => new ListPersonsResponse(
            p.Id,
            p.Name,
            p.DocumentType.ToString(),
            p.DocumentNumber,
            p.Nationality
            )).ToList());
    }
}