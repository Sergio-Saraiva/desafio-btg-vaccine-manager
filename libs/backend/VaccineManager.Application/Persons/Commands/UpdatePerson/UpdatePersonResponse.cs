namespace VaccineManager.Application.Persons.Commands.UpdatePerson;

public sealed record UpdatePersonResponse(
    Guid Id,
    string Name,
    string DocumentType,
    string DocumentNumber,
    string? Nationality
    );