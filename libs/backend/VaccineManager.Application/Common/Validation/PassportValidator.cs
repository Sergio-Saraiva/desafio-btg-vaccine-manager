using System.Text.RegularExpressions;
using FluentResults;

namespace VaccineManager.Application.Common.Validation;

public partial class PassportValidator : IDocumentValidator
{
    public Result Validate(string documentNumber)
    {
        var trimmed = documentNumber.Trim();

        if (trimmed.Length < 6 || trimmed.Length > 15)
            return Result.Fail("Passport number must be between 6 and 15 characters.");

        if (!AlphanumericRegex().IsMatch(trimmed))
            return Result.Fail("Passport number must contain only letters and digits.");

        return Result.Ok();
    }

    [GeneratedRegex(@"^[A-Za-z0-9]+$")]
    private static partial Regex AlphanumericRegex();
}
