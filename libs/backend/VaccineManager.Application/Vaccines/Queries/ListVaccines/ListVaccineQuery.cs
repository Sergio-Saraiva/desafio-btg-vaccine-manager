using Sieve.Models;
using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Vaccines.Queries.ListVaccines;

public sealed record ListVaccineQuery(SieveModel SieveModel) : IQuery<List<ListVaccineResponse>>;