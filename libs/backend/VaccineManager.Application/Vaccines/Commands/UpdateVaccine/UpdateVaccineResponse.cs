namespace VaccineManager.Application.Vaccines.Commands.UpdateVaccine;

public record UpdateVaccineResponse(
    Guid Id,
    string Name,
    int RequiredDoses,
    string Code
    );