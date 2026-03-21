using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class VaccinationRepository : BaseRepository<Vaccine>, IVaccinationRepository
{
    public VaccinationRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}