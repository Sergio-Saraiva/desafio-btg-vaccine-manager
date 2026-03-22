using System.Reflection;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace VaccineManager.Infrastructure.Persistence.Sieve;

public class AppSieveProcessor : SieveProcessor
{
    public AppSieveProcessor(IOptions<SieveOptions> options) : base(options)
    {
    }

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        var configurations = typeof(AppSieveProcessor).Assembly
            .GetTypes()                                                                                                     
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                        && typeof(ISieveConfiguration).IsAssignableFrom(t))                                                 
            .Select(Activator.CreateInstance)                                                                               
            .Cast<ISieveConfiguration>();                                                                                   
                                                                                                                              
        foreach (var config in configurations)                                                                              
            config.Configure(mapper);                                                                                       
                                                                                                                              
        return mapper;       
    }
}