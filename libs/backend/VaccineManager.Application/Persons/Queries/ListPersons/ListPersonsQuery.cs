using Sieve.Models;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public sealed record ListPersonsQuery(SieveModel SieveModel) : IQuery<PagedResponse<ListPersonsResponse>>;