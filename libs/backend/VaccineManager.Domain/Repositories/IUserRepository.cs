using VaccineManager.Domain.Entities;

namespace VaccineManager.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
}