using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Domain.Enums;

namespace VaccineManager.Application.Persons.Commands.CreatePerson;

public sealed record CreatePersonCommand(
    string Name, 
    DocumentType DocumentType, 
    string DocumentNumber, 
    string? Nationality) : ICommand<CreatePersonResponse>;