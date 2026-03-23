using FluentResults;

namespace VaccineManager.Application.Common.Validation;

public interface IDocumentValidator
{
    Result Validate(string documentNumber);
}
