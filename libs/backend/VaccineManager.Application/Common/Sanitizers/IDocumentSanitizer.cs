namespace VaccineManager.Application.Common.Sanitizers;

public interface IDocumentSanitizer
{
    string Sanitize(string documentNumber);
}
