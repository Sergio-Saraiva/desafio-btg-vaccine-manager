using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Common.Settings;
using VaccineManager.Domain.Repositories;
using VaccineManager.Infrastructure.Persistence.Context;
using VaccineManager.Infrastructure.Persistence.Repositories;

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

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IVaccineRepository, VaccineRepository>();
        services.AddScoped<IVaccinationRecordRepository, VaccinationRecordRepository>();

        return services;
    }
}