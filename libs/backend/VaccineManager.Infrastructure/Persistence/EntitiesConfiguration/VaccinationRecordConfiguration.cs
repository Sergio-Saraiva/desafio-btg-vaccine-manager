using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.EntitiesConfiguration;

public class VaccinationRecordConfiguration : IEntityTypeConfiguration<VaccinationRecord>
{
    public void Configure(EntityTypeBuilder<VaccinationRecord> builder)
    {
        builder.HasKey(vr => vr.Id);
        builder.Property(vr => vr.ApplicationDate)
            .IsRequired();
        builder.Property(vr => vr.CreatedAt).IsRequired();
        builder.Property(vr => vr.UpdatedAt).IsRequired(false);
        builder.Property(vr => vr.DeletedAt).IsRequired(false);
    }
}