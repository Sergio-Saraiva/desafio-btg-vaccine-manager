namespace VaccineManager.Domain.Entities;

public class VaccinationRecord : BaseEntity
{
    public Guid PersonId { get; private set; }
    public Guid VaccineId { get; private set; }
    public int DoseNumber { get; private set; } 
    public DateTime ApplicationDate { get; private set; }

    public Person Person { get; private set; } = null!;
    public Vaccine Vaccine { get; private set; } = null!;

    private VaccinationRecord() { }

    public VaccinationRecord(Guid personId, Guid vaccineId, int doseNumber, DateTime? applicationDate = null)
    {
        Id = Guid.CreateVersion7();
        PersonId = personId;
        VaccineId = vaccineId;
        DoseNumber = doseNumber;
        ApplicationDate = applicationDate ?? DateTime.UtcNow;
    }
}