using FluentResults;

namespace VaccineManager.Application.Common.Validation;

public class CpfValidator : IDocumentValidator
{
    public Result Validate(string documentNumber)
    {
        var digits = new string(documentNumber.Where(char.IsDigit).ToArray());

        if (digits.Length != 11)
            return Result.Fail("CPF must have exactly 11 digits.");

        if (digits.Distinct().Count() == 1)
            return Result.Fail("CPF is invalid.");

        if (!IsCheckDigitValid(digits))
            return Result.Fail("CPF is invalid.");

        return Result.Ok();
    }

    private static bool IsCheckDigitValid(string digits)
    {
        var firstDigit = CalculateCheckDigit(digits, 9);
        var secondDigit = CalculateCheckDigit(digits, 10);

        return digits[9] - '0' == firstDigit && digits[10] - '0' == secondDigit;
    }

    private static int CalculateCheckDigit(string digits, int length)
    {
        var sum = 0;
        for (var i = 0; i < length; i++)
            sum += (digits[i] - '0') * (length + 1 - i);

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
