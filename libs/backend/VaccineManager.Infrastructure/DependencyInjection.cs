using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using VaccineManager.Application.Common.Constants;
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
        services.AddDbContext<WriteDbContext>(options =>
            options.UseSqlite(
                appSettings.SQLiteSettings.ConnectionString,
                b => b.MigrationsAssembly(typeof(WriteDbContext).Assembly.FullName)
            )
        );

        services.AddDbContext<ReadDbContext>(options =>
            options.UseSqlite(appSettings.SQLiteSettings.ReadConnectionString)
        );

        services.Configure<SieveOptions>(options =>
        {
            options.MaxPageSize = appSettings.SieveSettings.MaxPageSize;
            options.DefaultPageSize = appSettings.SieveSettings.DefaultPageSize;
        });
        services.AddScoped<ISieveProcessor, AppSieveProcessor>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Write repositories (backed by WriteDbContext)
        services.AddKeyedScoped<IPersonRepository>(DbContextKeys.Write,
            (sp, _) => new PersonRepository(sp.GetRequiredService<WriteDbContext>()));
        services.AddKeyedScoped<IVaccineRepository>(DbContextKeys.Write,
            (sp, _) => new VaccineRepository(sp.GetRequiredService<WriteDbContext>()));
        services.AddKeyedScoped<IVaccinationRecordRepository>(DbContextKeys.Write,
            (sp, _) => new VaccinationRecordRepository(sp.GetRequiredService<WriteDbContext>()));
        services.AddKeyedScoped<IUserRepository>(DbContextKeys.Write,
            (sp, _) => new UserRepository(sp.GetRequiredService<WriteDbContext>()));

        // Read repositories (backed by ReadDbContext)
        services.AddKeyedScoped<IPersonRepository>(DbContextKeys.Read,
            (sp, _) => new PersonRepository(sp.GetRequiredService<ReadDbContext>()));
        services.AddKeyedScoped<IVaccineRepository>(DbContextKeys.Read,
            (sp, _) => new VaccineRepository(sp.GetRequiredService<ReadDbContext>()));
        services.AddKeyedScoped<IVaccinationRecordRepository>(DbContextKeys.Read,
            (sp, _) => new VaccinationRecordRepository(sp.GetRequiredService<ReadDbContext>()));
        services.AddKeyedScoped<IUserRepository>(DbContextKeys.Read,
            (sp, _) => new UserRepository(sp.GetRequiredService<ReadDbContext>()));

        services.AddScoped<ITokenService, JwtService>();

        return services;
    }
}
