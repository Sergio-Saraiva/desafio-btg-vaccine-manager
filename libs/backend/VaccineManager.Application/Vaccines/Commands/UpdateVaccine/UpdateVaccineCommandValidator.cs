using FluentValidation;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Application.Vaccines.Commands.UpdateVaccine;

public class UpdateVaccineCommandValidator : AbstractValidator<UpdateVaccineCommand>
{
    public UpdateVaccineCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(250).WithMessage("Name must not exceed 250 characters.");

        RuleFor(v => v.RequiredDoses)
            .GreaterThan(0).WithMessage("Required doses must be greater than 0.");
    }
}