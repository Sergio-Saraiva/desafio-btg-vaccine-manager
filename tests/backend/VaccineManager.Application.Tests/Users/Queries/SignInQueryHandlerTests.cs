using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Common.PasswordHasher;
using VaccineManager.Application.Common.Token;
using VaccineManager.Application.Users.Queries;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Users.Queries;

public class SignInQueryHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly SignInQueryHandler _sut;

    public SignInQueryHandlerTests()
    {
        _sut = new SignInQueryHandler(_userRepository, _tokenService, _passwordHasher);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsFailWithUnauthorized()
    {
        // Arrange
        var query = new SignInQuery("john@email.com", "Password123");
        _userRepository.GetUserByEmailAsync(query.Email)
            .Returns((User?)null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ReturnsFailWithUnauthorized()
    {
        // Arrange
        var query = new SignInQuery("john@email.com", "WrongPassword");
        var user = new User("john@email.com", "hashedpassword");
        _userRepository.GetUserByEmailAsync(query.Email)
            .Returns(user);
        _passwordHasher.VerifyHash(query.Password, user.PasswordHash)
            .Returns(false);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsTokenResponse()
    {
        // Arrange
        var query = new SignInQuery("john@email.com", "Password123");
        var user = new User("john@email.com", "hashedpassword");
        _userRepository.GetUserByEmailAsync(query.Email)
            .Returns(user);
        _passwordHasher.VerifyHash(query.Password, user.PasswordHash)
            .Returns(true);
        _tokenService.GenerateToken(user.Email)
            .Returns("jwt-token-value");

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("john@email.com");
        result.Value.AccessToken.Should().Be("jwt-token-value");
    }

    [Fact]
    public async Task Handle_UserNotFound_DoesNotCallPasswordHasherOrTokenService()
    {
        // Arrange
        var query = new SignInQuery("john@email.com", "Password123");
        _userRepository.GetUserByEmailAsync(query.Email)
            .Returns((User?)null);

        // Act
        await _sut.Handle(query, CancellationToken.None);

        // Assert
        _passwordHasher.DidNotReceive().VerifyHash(Arg.Any<string>(), Arg.Any<string>());
        _tokenService.DidNotReceive().GenerateToken(Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_InvalidPassword_DoesNotCallTokenService()
    {
        // Arrange
        var query = new SignInQuery("john@email.com", "WrongPassword");
        var user = new User("john@email.com", "hashedpassword");
        _userRepository.GetUserByEmailAsync(query.Email)
            .Returns(user);
        _passwordHasher.VerifyHash(query.Password, user.PasswordHash)
            .Returns(false);

        // Act
        await _sut.Handle(query, CancellationToken.None);

        // Assert
        _tokenService.DidNotReceive().GenerateToken(Arg.Any<string>());
    }
}
