using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Constants;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Vaccines.Commands.DeleteVaccine;

public class DeleteVaccineCommandHandler : ICommandHandler<DeleteVaccineCommand>
{
    private readonly IVaccineRepository _vaccineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVaccineCommandHandler([FromKeyedServices(DbContextKeys.Write)] IVaccineRepository vaccineRepository, IUnitOfWork unitOfWork)
    {
        _vaccineRepository = vaccineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
    {
        var vaccine = await _vaccineRepository.GetByIdAsync(request.Id);
        if (vaccine == null)
        {
            return Result.Fail(ApplicationErrors.Vaccine.NotFound(request.Id));
        }

        _vaccineRepository.Delete(vaccine);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}