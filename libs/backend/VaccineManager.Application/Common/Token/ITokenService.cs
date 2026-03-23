namespace VaccineManager.Application.Common.Token;

public interface ITokenService
{
    string GenereteToken(string email);
}