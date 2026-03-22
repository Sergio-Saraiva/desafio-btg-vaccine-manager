namespace VaccineManager.Application.Persons.Queries.ListPersons;

public sealed record ListPersonsResponse (
    Guid Id,
    string Name,
    string DocumentType,
    string DocumentNumber,
    string? Nationality
    );