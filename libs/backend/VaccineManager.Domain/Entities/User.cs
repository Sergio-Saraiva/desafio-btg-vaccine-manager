namespace VaccineManager.Domain.Entities;

public class User : BaseEntity
{
    public User(string email, string passwordHash)
    {
        Id = Guid.CreateVersion7();
        Email = email;
        PasswordHash = passwordHash;
    }

    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
}