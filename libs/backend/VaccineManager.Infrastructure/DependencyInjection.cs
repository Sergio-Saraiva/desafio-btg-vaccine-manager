using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using VaccineManager.Application.Common.Settings;
using VaccineManager.Application.Common.Token;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;
using VaccineManager.Infrastructure.Persistence.Repositories;
using VaccineManager.Infrastructure.Persistence.Sieve;

namespace VaccineManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                appSettings.SQLiteSettings.ConnectionString,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            )
        );
        
        services.Configure<SieveOptions>(options =>
        {
            options.MaxPageSize = appSettings.SieveSettings.MaxPageSize;
            options.DefaultPageSize = appSettings.SieveSettings.DefaultPageSize;
        });
        services.AddScoped<ISieveProcessor, AppSieveProcessor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IVaccineRepository, VaccineRepository>();
        services.AddScoped<IVaccinationRecordRepository, VaccinationRecordRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, JwtService>();

        return services;
    }
}