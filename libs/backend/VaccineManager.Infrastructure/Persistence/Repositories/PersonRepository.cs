using Microsoft.EntityFrameworkCore;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Person?> GetByDocumentAsync(DocumentType documentType, string documentNumber)
    {
        return await _dbSet.Where(p =>
                p.DocumentType == documentType &&
                p.DocumentNumber == documentNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<Person?> GetByDocumentIncludingDeletedAsync(DocumentType documentType, string documentNumber)
    {
        return await _dbSet.IgnoreQueryFilters()
            .Where(p => p.DocumentType == documentType && p.DocumentNumber == documentNumber)
            .FirstOrDefaultAsync();
    }

    public new async Task<Person?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Where(p => p.Id == id)
            .Include(p => p.VaccinationRecords)
            .ThenInclude(vr => vr.Vaccine)
            .FirstOrDefaultAsync();
    }

    public new void Delete(Person person)
    {
        person.Delete();
        _dbSet.Update(person);
    }
}