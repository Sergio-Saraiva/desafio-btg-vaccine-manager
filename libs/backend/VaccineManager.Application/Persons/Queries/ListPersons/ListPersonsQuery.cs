using Sieve.Models;
using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public sealed record ListPersonsQuery(SieveModel SieveModel) : IQuery<List<ListPersonsResponse>>;