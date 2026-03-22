using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Persons.Commands.DeletePerson;

public sealed record DeletePersonCommand(Guid Id) : ICommand;