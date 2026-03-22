using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Domain.Enums;

namespace VaccineManager.Application.Persons.Commands.UpdatePerson;

public sealed record UpdatePersonCommand(
    Guid Id,
    string Name,
    DocumentType DocumentType, 
    string DocumentNumber, 
    string? Nationality
    ) : ICommand<UpdatePersonResponse>;