using VaccineManager.Domain.Entities;

namespace VaccineManager.Domain.Repositories;

public interface IVaccineRepository : IBaseRepository<Vaccine>
{
    Task<Vaccine?> GetByCodeAsync(string code);
}