using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class VaccineRepository : BaseRepository<Vaccine>, IVaccineRepository
{
    public VaccineRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}