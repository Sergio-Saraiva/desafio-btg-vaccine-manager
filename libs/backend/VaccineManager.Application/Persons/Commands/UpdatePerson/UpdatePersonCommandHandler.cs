using FluentResults;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandHandler : ICommandHandler<UpdatePersonCommand, UpdatePersonResponse>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdatePersonResponse>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id);
        if (person == null)
        {
            return Result.Fail(ApplicationErrors.Person.NotFound(request.Id));
        }

        var personWithSameDocument = await _personRepository.GetByDocumentAsync(request.DocumentType, request.DocumentNumber);
        if (personWithSameDocument != null)
        {
            if (personWithSameDocument.Id != request.Id)
            {
                return Result.Fail(ApplicationErrors.Person.DuplicateDocument);
            }
        }
        
        person.Update(request.Name, request.DocumentType, request.DocumentNumber, request.Nationality);
        await _personRepository.UpdateAsync(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Ok(new UpdatePersonResponse(
            person.Id,
            person.Name,
            person.DocumentType.ToString(),
            person.DocumentNumber,
            person.Nationality));
    }
}