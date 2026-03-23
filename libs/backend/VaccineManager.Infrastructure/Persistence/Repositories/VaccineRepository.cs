using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class VaccineRepository : BaseRepository<Vaccine>, IVaccineRepository
{
    public VaccineRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Vaccine?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(vaccine => vaccine.Code == code);
    }
}