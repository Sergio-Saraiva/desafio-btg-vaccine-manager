using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.Vaccines.Commands.UpdateVaccine;

public record UpdateVaccineCommand(
    Guid Id,
    string Name,
    int RequiredDoses
    ) : ICommand<UpdateVaccineResponse>;