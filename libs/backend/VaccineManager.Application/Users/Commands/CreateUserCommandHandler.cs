using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Constants;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Common.PasswordHasher;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Users.Commands;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler([FromKeyedServices(DbContextKeys.Write)] IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.GetUserByEmailAsync(request.Email);
        if (userExists != null)
        {
            return Result.Fail(ApplicationErrors.User.DuplicateEmail);
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        var user = await _userRepository.AddAsync(new User(
            request.Email,
            hashedPassword
        ));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(new CreateUserResponse(
            user.Id,
            user.Email
        ));
    }
}