using Sieve.Services;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.Sieve;

public class VaccineSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Vaccine>(v => v.Name)
            .CanFilter().CanSort();
        mapper.Property<Vaccine>(v => v.Code)
            .CanFilter();
        mapper.Property<Vaccine>(v => v.RequiredDoses)
            .CanFilter().CanSort();
        mapper.Property<Vaccine>(v => v.CreatedAt)
            .CanFilter().CanSort();
    }
}