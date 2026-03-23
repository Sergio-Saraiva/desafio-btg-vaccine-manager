using FluentResults;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Commands.DeletePerson;

public class DeletePersonCommandHandler : ICommandHandler<DeletePersonCommand>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id);
        if (person == null)
        {
            return Result.Fail(ApplicationErrors.Person.NotFound(request.Id));
        }
        
        _personRepository.Delete(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}