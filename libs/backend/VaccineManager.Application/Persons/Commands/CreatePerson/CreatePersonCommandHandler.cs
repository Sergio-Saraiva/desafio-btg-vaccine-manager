using FluentResults;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Common.Sanitizers;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Persons.Commands.CreatePerson;

public class CreatePersonCommandHandler : ICommandHandler<CreatePersonCommand, CreatePersonResponse>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreatePersonResponse>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var sanitizedDocument = DocumentSanitizerFactory.GetSanitizer(request.DocumentType).Sanitize(request.DocumentNumber);
        var existingPerson = await _personRepository.GetByDocumentAsync(request.DocumentType, sanitizedDocument);

        if (existingPerson != null)
        {
            return Result.Fail<CreatePersonResponse>(ApplicationErrors.Person.DuplicateDocument);
        }

        var person = new Person(request.Name, request.DocumentType, request.DocumentNumber, request.Nationality);
        
        await _personRepository.AddAsync(person);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(new CreatePersonResponse(
            person.Id,
            person.Name,
            person.DocumentType.ToString(),
            person.DocumentNumber,
            person.Nationality)
        );
    }
}