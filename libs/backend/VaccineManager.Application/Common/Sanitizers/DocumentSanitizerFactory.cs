using VaccineManager.Domain.Enums;

namespace VaccineManager.Application.Common.Sanitizers;

public static class DocumentSanitizerFactory
{
    private static readonly Dictionary<DocumentType, IDocumentSanitizer> Sanitizers = new()
    {
        { DocumentType.Cpf, new CpfSanitizer() },
        { DocumentType.Passport, new PassportSanitizer() }
    };

    public static IDocumentSanitizer GetSanitizer(DocumentType documentType)
    {
        if (Sanitizers.TryGetValue(documentType, out var sanitizer))
            return sanitizer;

        throw new ArgumentOutOfRangeException(nameof(documentType), $"No sanitizer registered for document type '{documentType}'.");
    }
}
