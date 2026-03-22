using Sieve.Services;

namespace VaccineManager.Infrastructure.Persistence.Sieve;

public interface ISieveConfiguration
{
    void Configure(SievePropertyMapper mapper);
}