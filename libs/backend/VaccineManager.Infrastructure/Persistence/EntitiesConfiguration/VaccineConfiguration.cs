using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.EntitiesConfiguration;

public class VaccineConfiguration : IEntityTypeConfiguration<Vaccine>
{
    public void Configure(EntityTypeBuilder<Vaccine> builder)
    {
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Name).IsRequired().HasMaxLength(250);
        builder.Property(v => v.RequiredDoses).IsRequired();
        
        builder.HasMany(v => v.VaccinationRecords).WithOne(v => v.Vaccine)
            .HasForeignKey(v => v.VaccineId).OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(v => v.Code)
            .IsRequired()                                                                                                           
            .HasMaxLength(20);                                                                                                      
        builder.HasIndex(v => v.Code)                                                                                               
            .IsUnique();
        
        builder.Property(v => v.CreatedAt).IsRequired();
        builder.Property(v => v.UpdatedAt).IsRequired(false);
        builder.Property(v => v.DeletedAt).IsRequired(false);
    }
}