using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.Context;

public class WriteDbContext : DbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options)
    {
    }

    public DbSet<Person>  Persons { get; set; }
    public DbSet<Vaccine>  Vaccines { get; set; }
    public DbSet<VaccinationRecord>  VaccinationRecords { get; set; }
    public DbSet<User> Users { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DbContextModelConfiguration.ApplySharedConfiguration(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
