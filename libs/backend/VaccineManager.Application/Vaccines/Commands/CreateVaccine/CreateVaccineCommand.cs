using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Vaccines.Commands.CreateVaccine;

public sealed record CreateVaccineCommand(
    string Name,
    int RequiredDoses,
    string? Code) : ICommand<CreateVaccineResponse>;