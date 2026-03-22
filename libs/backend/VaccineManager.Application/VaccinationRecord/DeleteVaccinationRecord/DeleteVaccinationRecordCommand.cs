using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.VaccinationRecord.DeleteVaccinationRecord;

public record DeleteVaccinationRecordCommand(Guid Id) : ICommand;