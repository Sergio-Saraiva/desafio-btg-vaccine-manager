using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;

namespace VaccineManager.Infrastructure.Persistence.Repositories;

public class PersonRepository : BaseRepository<Person>, IPersonRepository
{
    public PersonRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}