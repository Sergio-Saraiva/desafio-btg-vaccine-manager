using VaccineManager.Domain.Enums;

namespace VaccineManager.Application.Common.Validation;

public static class DocumentValidatorFactory
{
    private static readonly Dictionary<DocumentType, IDocumentValidator> Validators = new()
    {
        { DocumentType.Cpf, new CpfValidator() },
        { DocumentType.Passport, new PassportValidator() }
    };

    public static IDocumentValidator GetValidator(DocumentType documentType)
    {
        if (Validators.TryGetValue(documentType, out var validator))
            return validator;

        throw new ArgumentOutOfRangeException(nameof(documentType), $"No validator registered for document type '{documentType}'.");
    }
}
