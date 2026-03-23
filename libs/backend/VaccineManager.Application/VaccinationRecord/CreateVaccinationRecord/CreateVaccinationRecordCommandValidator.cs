using FluentValidation;

namespace VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;

public class CreateVaccinationRecordCommandValidator : AbstractValidator<CreateVaccinationRecordCommand>
{
    public CreateVaccinationRecordCommandValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person ID is required.");

        RuleFor(x => x.VaccineId)
            .NotEmpty().WithMessage("Vaccine ID is required.");
    }
}