namespace VaccineManager.Application.Vaccines.Commands.CreateVaccine;

public sealed record CreateVaccineResponse(
    Guid Id,
    string Name,
    int RequiredDoses,
    string Code
    );