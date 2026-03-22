using FluentResults;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandHandler : ICommandHandler<CreateVaccineCommand, CreateVaccineResponse>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVaccineCommandHandler(IVaccineRepository vaccineRepository, IUnitOfWork unitOfWork)
    {
        _vaccineRepository = vaccineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateVaccineResponse>> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Code))
        {
            var vaccineWithThisCodeExits = await _vaccineRepository.GetByCodeAsync(request.Code);
            if (vaccineWithThisCodeExits != null)
            {
                return Result.Fail(ApplicationErrors.Vaccine.DuplicateCode);
            }
        }

        var vaccine = await _vaccineRepository.AddAsync(new Vaccine(request.Name, request.RequiredDoses, request.Code));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(new CreateVaccineResponse(vaccine.Id, vaccine.Name, vaccine.RequiredDoses, vaccine.Code));
    }
}