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
        DocumentNumber = ValidateDocument(documentType, documentNumber);
        Nationality = nationality;
    }
    
    public void Update(string name, DocumentType documentType, string documentNumber, string? nationality)
    {
        Name = name;
        DocumentType = documentType;
        DocumentNumber = ValidateDocument(documentType, documentNumber);
        Nationality = nationality;
    }
    
    private static string ValidateDocument(DocumentType type, string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Document number is required.");

        var cleaned = number.Trim();

        return type switch
        {
            DocumentType.Cpf => ValidateCpf(cleaned),
            DocumentType.Passport => ValidatePassport(cleaned),
            _ => throw new ArgumentException($"Unknown document type: {type}")
        };
    }

    private static string ValidateCpf(string cpf)
    {
        var digits = new string(cpf.Where(char.IsDigit).ToArray());

        return digits.Length != 11 ? throw new ArgumentException("CPF must have exactly 11 digits.") : digits;
    }

    private static string ValidatePassport(string passport)
    {
        var cleaned = passport.Trim().ToUpperInvariant();

        if (cleaned.Length < 6 || cleaned.Length > 15)
            throw new ArgumentException("Passport number must be between 6 and 15 characters.");

        return cleaned;
    }
}