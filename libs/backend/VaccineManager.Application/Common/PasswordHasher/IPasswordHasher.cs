namespace VaccineManager.Application.Common.PasswordHasher;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyHash(string password, string storedHashBase64);
}