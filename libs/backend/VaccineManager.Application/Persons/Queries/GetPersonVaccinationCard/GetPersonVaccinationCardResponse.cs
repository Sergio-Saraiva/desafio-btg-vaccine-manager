namespace VaccineManager.Application.Persons.Queries.GetPersonVaccinationCard;

public sealed record GetPersonVaccinationCardResponse(
    Guid PersonId,                                                                                                          
    string PersonName,
    string DocumentType,
    string DocumentNumber,
    List<VaccinationCardEntry> Vaccines);
                                                                                                                              
public sealed record VaccinationCardEntry(                                                                                  
    Guid VaccineId,                                                                                                         
    string VaccineName,                                                                                                     
    string VaccineCode,
    int RequiredDoses,
    int DosesTaken,
    bool IsComplete,
    List<DoseRecord> Doses);                                                                                                
                                                                                                                              
public sealed record DoseRecord(                                                                                            
    Guid RecordId,                                                                                                          
    DateTime ApplicationDate);