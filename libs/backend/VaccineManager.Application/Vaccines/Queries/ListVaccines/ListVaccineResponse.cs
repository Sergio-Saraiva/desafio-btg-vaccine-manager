namespace VaccineManager.Application.Vaccines.Queries.ListVaccines;

public sealed record ListVaccineResponse(Guid Id, string Name, int RequiredDoses, string Code, DateTime CreatedAt);