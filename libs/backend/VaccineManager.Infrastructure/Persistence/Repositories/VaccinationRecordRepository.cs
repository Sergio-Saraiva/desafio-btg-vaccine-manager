using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class VaccinationRecordRepository : BaseRepository<VaccinationRecord>, IVaccinationRecordRepository
{
    public VaccinationRecordRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}