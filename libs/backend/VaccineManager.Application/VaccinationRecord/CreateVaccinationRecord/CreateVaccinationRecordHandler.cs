using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Constants;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;

public class CreateVaccinationRecordHandler : ICommandHandler<CreateVaccinationRecordCommand, CreateVaccinationRecordResponse>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IVaccinationRecordRepository _vaccinationRecordRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVaccinationRecordHandler(
        [FromKeyedServices(DbContextKeys.Write)] IVaccineRepository vaccineRepository,
        [FromKeyedServices(DbContextKeys.Write)] IPersonRepository personRepository,
        [FromKeyedServices(DbContextKeys.Write)] IVaccinationRecordRepository vaccinationRecordRepository,
        IUnitOfWork unitOfWork)
    {
        _vaccineRepository = vaccineRepository;
        _personRepository = personRepository;
        _vaccinationRecordRepository = vaccinationRecordRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateVaccinationRecordResponse>> Handle(CreateVaccinationRecordCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId);
        if (person == null)
        {
            return Result.Fail(ApplicationErrors.VaccinationRecord.PersonNotFound(request.PersonId));
        }

        var vaccine = await _vaccineRepository.GetByIdAsync(request.VaccineId);
        if (vaccine == null)
        {
            return Result.Fail(ApplicationErrors.VaccinationRecord.VaccineNotFound(request.VaccineId));
        }
        
        var amountOfDosesTakenByVaccine = person.VaccinationRecords.Count(x => x.VaccineId == request.VaccineId);
        if (amountOfDosesTakenByVaccine >= vaccine.RequiredDoses)
        {
            return Result.Fail(ApplicationErrors.VaccinationRecord.MaxDosesReached);
        }

        var vaccinationRecord = new Domain.Entities.VaccinationRecord(request.PersonId, request.VaccineId);
        await _vaccinationRecordRepository.AddAsync(vaccinationRecord);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(new CreateVaccinationRecordResponse(
            vaccinationRecord.Id,
            person.Name, 
            person.DocumentType.ToString(), 
            person.DocumentNumber, 
            vaccine.Id, 
            vaccine.Name, 
            amountOfDosesTakenByVaccine + 1, 
            vaccine.Code, 
            vaccinationRecord.ApplicationDate));
    }
}