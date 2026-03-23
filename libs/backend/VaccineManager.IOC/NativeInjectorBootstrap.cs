using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application;
using VaccineManager.Application.Common.Settings;
using VaccineManager.Infrastructure;

namespace VaccineManager.IOC;

public static class NativeInjectorBootstrap
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = new AppSettings();
        configuration.Bind(appSettings);
        services
            .AddApplication(appSettings)
            .AddInfrastructure(appSettings);
        
        return services;
    }
}