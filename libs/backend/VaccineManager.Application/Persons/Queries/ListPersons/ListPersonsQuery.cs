using Sieve.Models;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Responses;

namespace VaccineManager.Application.Persons.Queries.ListPersons;

public sealed record ListPersonsQuery(SieveModel SieveModel) : IQuery<PagedResponse<ListPersonsResponse>>;