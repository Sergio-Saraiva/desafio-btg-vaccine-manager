using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace VaccineManager.Application.Common.PasswordHasher;

public class Argon2IdPasswordHasher : IPasswordHasher
{
    private const int MemorySize = 19456; 
    private const int Iterations = 2;     
    private const int DegreeOfParallelism = 1; 

    private const int SaltSize = 16; 
    private const int HashSize = 32; 

    public string HashPassword(string password)
    {
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        byte[] hash = argon2.GetBytes(HashSize);
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyHash(string password, string storedHashBase64)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHashBase64);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        byte[] storedHash = new byte[HashSize];
        Array.Copy(hashBytes, SaltSize, storedHash, 0, HashSize);

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        byte[] computedHash = argon2.GetBytes(HashSize);
        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}