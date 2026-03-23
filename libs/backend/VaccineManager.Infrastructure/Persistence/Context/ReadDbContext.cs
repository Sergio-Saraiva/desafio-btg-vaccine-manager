using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.Context;

public class 
    ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Person>  Persons { get; set; }
    public DbSet<Vaccine>  Vaccines { get; set; }
    public DbSet<VaccinationRecord>  VaccinationRecords { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DbContextModelConfiguration.ApplySharedConfiguration(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
}
