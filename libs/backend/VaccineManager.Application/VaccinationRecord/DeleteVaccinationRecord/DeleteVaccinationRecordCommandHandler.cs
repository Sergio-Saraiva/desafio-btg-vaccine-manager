using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Constants;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.VaccinationRecord.DeleteVaccinationRecord;

public class DeleteVaccinationRecordCommandHandler : ICommandHandler<DeleteVaccinationRecordCommand>
{
    private readonly IVaccinationRecordRepository _vaccinationRecordRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVaccinationRecordCommandHandler([FromKeyedServices(DbContextKeys.Write)] IVaccinationRecordRepository vaccinationRecordRepository, IUnitOfWork unitOfWork)
    {
        _vaccinationRecordRepository = vaccinationRecordRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteVaccinationRecordCommand request, CancellationToken cancellationToken)
    {
        var vaccinationRecord = await _vaccinationRecordRepository.GetByIdAsync(request.Id);
        if (vaccinationRecord == null)
        {
            return Result.Fail(ApplicationErrors.VaccinationRecord.NotFound(request.Id));
        }
        
        _vaccinationRecordRepository.Delete(vaccinationRecord);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}