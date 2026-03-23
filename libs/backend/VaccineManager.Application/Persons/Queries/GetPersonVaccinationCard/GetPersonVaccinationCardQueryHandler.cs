using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Constants;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Queries.GetPersonVaccinationCard;

public class GetPersonVaccinationCardQueryHandler : IQueryHandler<GetPersonVaccinationCardQuery, GetPersonVaccinationCardResponse>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonVaccinationCardQueryHandler([FromKeyedServices(DbContextKeys.Read)] IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Result<GetPersonVaccinationCardResponse>> Handle(GetPersonVaccinationCardQuery request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id);

        if (person == null)
        {
              return Result.Fail(ApplicationErrors.Person.NotFound(request.Id));
        }                                                                                                 
                                                                                                                              
        var vaccines = person.VaccinationRecords
              .GroupBy(vr => vr.VaccineId)                                                                                    
              .Select(group =>                                                                                                
              {                                                                                                               
                  var vaccine = group.First().Vaccine;                                                                        
                  var doses = group                                                                                           
                      .OrderBy(vr => vr.ApplicationDate)                                                                      
                      .Select((vr, index) => new DoseRecord(
                          vr.Id,
                          vr.ApplicationDate))                                                                                
                      .ToList();                                                                                              
                                                                                                                              
                  return new VaccinationCardEntry(
                      vaccine.Id,
                      vaccine.Name,
                      vaccine.Code,
                      vaccine.RequiredDoses,
                      doses.Count,                                                                                            
                      doses.Count >= vaccine.RequiredDoses,                                                                   
                      doses);                                                                                                 
              })                                                                                                              
              .ToList();

          return Result.Ok(new GetPersonVaccinationCardResponse(
              person.Id,
              person.Name,
              person.DocumentType.ToString(),                                                                                 
              person.DocumentNumber,
              vaccines)); 
    }
}