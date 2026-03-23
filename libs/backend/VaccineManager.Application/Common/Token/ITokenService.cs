namespace VaccineManager.Application.Common.Token;

public interface ITokenService
{
    string GenerateToken(string email);
}