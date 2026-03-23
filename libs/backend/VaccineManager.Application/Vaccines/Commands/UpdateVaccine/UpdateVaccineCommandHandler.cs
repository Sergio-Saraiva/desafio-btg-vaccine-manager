using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Constants;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Vaccines.Commands.UpdateVaccine;

public class UpdateVaccineCommandHandler : ICommandHandler<UpdateVaccineCommand, UpdateVaccineResponse>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVaccineCommandHandler([FromKeyedServices(DbContextKeys.Write)] IVaccineRepository vaccineRepository, IUnitOfWork unitOfWork)
    {
        _vaccineRepository = vaccineRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<UpdateVaccineResponse>> Handle(UpdateVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = await _vaccineRepository.GetByIdAsync(request.Id);
        if (vaccine == null)
        {
            return Result.Fail(ApplicationErrors.Vaccine.NotFound(request.Id));
        }
        
        vaccine.Update(request.Name, request.RequiredDoses);
        await _vaccineRepository.UpdateAsync(vaccine);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(new UpdateVaccineResponse(vaccine.Id, vaccine.Name, vaccine.RequiredDoses, vaccine.Code));
    }
}