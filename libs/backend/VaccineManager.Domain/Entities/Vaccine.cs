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
        RequiredDoses = ValidateDoses(requiredDoses);
    }

    private static int ValidateDoses(int requiredDoses)
    {
        return requiredDoses > 0 ? requiredDoses
            : throw new ArgumentException("RequiredDoses must be at least 1.");
    }

    public void Update(string name, int requiredDoses)
    {
        Name = name;
        RequiredDoses = ValidateDoses(requiredDoses);
    }

    private static string GenerateVaccineHumanReadableCode(Guid vaccineId)
    {
        return $"VAC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..3].ToUpper()}";
    }
}