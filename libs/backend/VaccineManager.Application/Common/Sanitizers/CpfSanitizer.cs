namespace VaccineManager.Application.Common.Sanitizers;

public class CpfSanitizer : IDocumentSanitizer
{
    public string Sanitize(string documentNumber)
    {
        return new string(documentNumber.Where(char.IsDigit).ToArray());
    }
}
