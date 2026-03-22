using VaccineManager.Application.Abstractions.Messaging;

namespace VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;

public record CreateVaccinationRecordCommand(Guid PersonId, Guid VaccineId) : ICommand<CreateVaccinationRecordResponse>;