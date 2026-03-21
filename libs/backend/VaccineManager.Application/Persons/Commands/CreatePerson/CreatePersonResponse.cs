namespace VaccineManager.Application.Persons.Commands.CreatePerson;

public sealed record CreatePersonResponse(
    Guid Id,
    string Name,
    string DocumentType,
    string DocumentNumber,
    string? Nationality);