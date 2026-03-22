using Sieve.Models;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common;

namespace VaccineManager.Application.Vaccines.Queries.ListVaccines;

public sealed record ListVaccineQuery(SieveModel SieveModel) : IQuery<PagedResponse<ListVaccineResponse>>;