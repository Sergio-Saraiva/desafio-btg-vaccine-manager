using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public sealed record ListPersonsQuery() : IQuery<List<ListPersonsResponse>>;