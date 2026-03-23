using FluentValidation;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Application.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandValidator : AbstractValidator<CreateVaccineCommand>
{
    public CreateVaccineCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(250).WithMessage("Name must not exceed 250 characters.");

        RuleFor(v => v.RequiredDoses)
            .GreaterThan(0).WithMessage("Required doses must be greater than 0.");

        RuleFor(v => v.Code)
            .MaximumLength(20).WithMessage("Code must not exceed 20 characters.")
            .When(v => !string.IsNullOrWhiteSpace(v.Code));
    }
    
}