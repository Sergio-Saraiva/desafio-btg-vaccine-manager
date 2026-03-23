namespace VaccineManager.Application.Common.Sanitizers;

public class PassportSanitizer : IDocumentSanitizer
{
    public string Sanitize(string documentNumber)
    {
        return documentNumber.Trim().ToUpperInvariant();
    }
}
