using VaccineManager.Domain.Enums;

namespace VaccineManager.Domain.Entities;

public class Person : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public DocumentType DocumentType { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty;
    public string? Nationality { get; private set; }
    public ICollection<VaccinationRecord> VaccinationRecords { get; private set; } = new List<VaccinationRecord>();

    private Person() { }

    public Person(string name, DocumentType documentType, string documentNumber, string nationality)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        DocumentType = documentType;
        DocumentNumber = documentNumber;
        Nationality = nationality;
    }
    
    public void Update(string name, DocumentType documentType, string documentNumber, string? nationality)
    {
        Name = name;
        DocumentType = documentType;
        DocumentNumber = documentNumber;
        Nationality = nationality;
    }
}