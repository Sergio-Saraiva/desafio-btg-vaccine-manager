namespace VaccineManager.Domain.Entities;

public class Vaccine : BaseEntity
{
    public string Name { get; private set; }
    public int RequiredDoses { get; private set; }
    public ICollection<VaccinationRecord> VaccinationRecords { get; private set; } = new List<VaccinationRecord>();
    
    public Vaccine(string name, int requiredDoses)
    {
        Id = Guid.NewGuid();
        Name = name;
        RequiredDoses = requiredDoses > 0 ? requiredDoses
            : throw new ArgumentException("RequiredDoses must be at least 1.");
    }
}