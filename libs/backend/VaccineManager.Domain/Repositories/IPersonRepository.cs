using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Enums;

namespace VaccineManager.Domain.Repositories;

public interface IPersonRepository : IBaseRepository<Person>
{
    Task<Person?> GetByDocumentAsync(DocumentType documentType, string documentNumber);
}