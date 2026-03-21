namespace VaccineManager.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; internal set; }
    public bool IsDeleted => DeletedAt != null;
    public DateTime CreatedAt { get; internal set; }
    public DateTime? UpdatedAt { get; internal set; }
    public DateTime? DeletedAt { get; internal set; }

    public void Delete()
    {
        DeletedAt = DateTime.UtcNow;
    }
}