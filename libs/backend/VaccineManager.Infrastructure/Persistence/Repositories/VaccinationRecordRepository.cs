using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class VaccinationRecordRepository : BaseRepository<VaccinationRecord>, IVaccinationRecordRepository
{
    public VaccinationRecordRepository(DbContext dbContext) : base(dbContext)
    {
    }
}