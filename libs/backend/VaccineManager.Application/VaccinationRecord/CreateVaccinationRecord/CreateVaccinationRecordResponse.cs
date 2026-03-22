namespace VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;

public record CreateVaccinationRecordResponse(
    Guid PersonId, 
    string PersonName, 
    string DocumentType, 
    string DocumentNumber, 
    Guid VaccineId, 
    string VaccineName, 
    int DoseNumber, 
    string VaccineCode, 
    DateTime ApplicationDate
    );