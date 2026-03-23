using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.EntitiesConfiguration;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
        
        builder.HasIndex(p => new { p.DocumentType, p.DocumentNumber }).IsUnique();
        
        builder.Property(p => p.DocumentType).IsRequired().HasConversion<int>();
        builder.Property(p => p.DocumentNumber).IsRequired();
        builder.Property(p => p.Nationality).IsRequired(false);
        
        builder.HasMany(p => p.VaccinationRecords).WithOne(p => p.Person)
            .HasForeignKey(p => p.PersonId).OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired(false);
        builder.Property(p => p.DeletedAt).IsRequired(false);
    }
}