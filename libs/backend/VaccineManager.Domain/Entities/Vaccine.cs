namespace VaccineManager.Domain.Entities;

public class Vaccine : BaseEntity
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public int RequiredDoses { get; private set; }
    public ICollection<VaccinationRecord> VaccinationRecords { get; private set; } = new List<VaccinationRecord>();
    
    public Vaccine(string name, int requiredDoses, string? code = null)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Code = string.IsNullOrEmpty(code) ? GenerateVaccineHumanReadableCode(this.Id) : code;
        RequiredDoses = requiredDoses;
    }
    
    public void Update(string name, int requiredDoses)
    {
        Name = name;
        RequiredDoses = requiredDoses;
    }

    private static string GenerateVaccineHumanReadableCode(Guid vaccineId)
    {
        return $"VAC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..3].ToUpper()}";
    }
}