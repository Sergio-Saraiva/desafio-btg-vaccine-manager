using Sieve.Services;
using VaccineManager.Domain.Entities;

namespace VaccineManager.Infrastructure.Persistence.Sieve;

public class PersonSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Person>(p => p.Name)
            .CanFilter().CanSort();                                                                                         
                                                                                                                              
        mapper.Property<Person>(p => p.DocumentType)                                                                        
            .CanFilter();                                                                                                   
                                                                                                                              
        mapper.Property<Person>(p => p.DocumentNumber)                                                                      
            .CanFilter();                                                                                                   
                                                                                                                              
        mapper.Property<Person>(p => p.Nationality)                                                                         
            .CanFilter().CanSort();                                                                                         
                                                                                                                              
        mapper.Property<Person>(p => p.CreatedAt)                                                                           
            .CanSort();       
    }
}