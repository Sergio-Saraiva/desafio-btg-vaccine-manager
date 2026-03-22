using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Person?> GetByDocumentAsync(DocumentType documentType, string documentNumber)
    {
        return await _dbSet.Where(p =>
                p.DocumentType == documentType &&
                p.DocumentNumber == documentNumber)
            .FirstOrDefaultAsync();
    }

    public new async Task<Person?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Where(p => p.Id == id)
            .Include(p => p.VaccinationRecords)
            .FirstOrDefaultAsync();
    }
}