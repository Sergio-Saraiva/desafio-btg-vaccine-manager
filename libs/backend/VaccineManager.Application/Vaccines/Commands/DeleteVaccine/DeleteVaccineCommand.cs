using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Vaccines.Commands.DeleteVaccine;

public sealed record DeleteVaccineCommand(Guid Id) : ICommand;