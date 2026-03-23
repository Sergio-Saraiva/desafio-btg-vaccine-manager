using FluentValidation;
using VaccineManager.Application.Common.Validation;

namespace VaccineManager.Application.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(250).WithMessage("Name must not exceed 250 characters.");

        RuleFor(x => x.DocumentType)
            .IsInEnum().WithMessage("Invalid document type.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .MaximumLength(20).WithMessage("Document number must not exceed 20 characters.");

        RuleFor(x => x)
            .Must(x => DocumentValidatorFactory.GetValidator(x.DocumentType).Validate(x.DocumentNumber).IsSuccess)
            .When(x => !string.IsNullOrEmpty(x.DocumentNumber))
            .WithMessage((x, _) => DocumentValidatorFactory.GetValidator(x.DocumentType).Validate(x.DocumentNumber).Errors.FirstOrDefault()?.Message ?? "Invalid document number.")
            .OverridePropertyName("DocumentNumber");

        RuleFor(x => x.Nationality)
            .MaximumLength(100).WithMessage("Nationality must not exceed 100 characters.")
            .When(x => x.Nationality is not null);
    }
}
