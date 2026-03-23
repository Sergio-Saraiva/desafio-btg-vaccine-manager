using FluentResults;
using VaccineManager.Application.Abstractions.Messaging;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Common.PasswordHasher;
using VaccineManager.Application.Common.Token;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Users.Queries;

public class SignInQueryHandler : IQueryHandler<SignInQuery, SignInQueryResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public SignInQueryHandler(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<SignInQueryResponse>> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.GetUserByEmailAsync(request.Email);
        if (userExists == null)
        {
            return Result.Fail(ApplicationErrors.User.InvalidPasswordOrEmail);
        }

        var result = _passwordHasher.VerifyHash(request.Password, userExists.PasswordHash);
        if (!result)
        {
            return Result.Fail(ApplicationErrors.User.InvalidPasswordOrEmail);
        }

        var jwtToken = _tokenService.GenereteToken(userExists.Email);
        return Result.Ok(new SignInQueryResponse(userExists.Email, jwtToken));
    }
}