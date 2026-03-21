using FluentValidation;
using VaccineManager.Domain.Enums;

namespace VaccineManager.Application.Persons.Commands.CreatePerson;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(250).WithMessage("Name must not exceed 250 characters.");

        RuleFor(x => x.DocumentType)
            .IsInEnum().WithMessage("Invalid document type.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .MaximumLength(20).WithMessage("Document number must not exceed 20 characters.");

        RuleFor(x => x.DocumentNumber)
            .Must(cpf => cpf.Where(char.IsDigit).Count() == 11)
            .When(x => x.DocumentType == DocumentType.Cpf)
            .WithMessage("CPF must have exactly 11 digits.");

        RuleFor(x => x.DocumentNumber)
            .Length(6, 15)
            .When(x => x.DocumentType == DocumentType.Passport)
            .WithMessage("Passport number must be between 6 and 15 characters.");

        RuleFor(x => x.Nationality)
            .MaximumLength(100).WithMessage("Nationality must not exceed 100 characters.")
            .When(x => x.Nationality is not null);
    }
}