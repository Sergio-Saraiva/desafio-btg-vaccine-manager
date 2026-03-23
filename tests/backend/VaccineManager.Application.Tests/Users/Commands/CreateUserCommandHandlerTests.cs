using System.Net;
using FluentAssertions;
using NSubstitute;
using VaccineManager.Application.Common.Errors;
using VaccineManager.Application.Common.PasswordHasher;
using VaccineManager.Application.Users.Commands;
using VaccineManager.Domain.Entities;
using VaccineManager.Domain.Repositories;

namespace VaccineManager.Application.Tests.Users.Commands;

public class CreateUserCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly CreateUserCommandHandler _sut;

    public CreateUserCommandHandlerTests()
    {
        _sut = new CreateUserCommandHandler(_userRepository, _unitOfWork, _passwordHasher);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ReturnsFailWithConflict()
    {
        // Arrange
        var command = new CreateUserCommand("john@email.com", "Password123", "Password123");
        _userRepository.GetUserByEmailAsync(command.Email)
            .Returns(new User("john@email.com", "hashedpassword"));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Should().BeOfType<ApiError>()
            .Which.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesUserAndReturnsOk()
    {
        // Arrange
        var command = new CreateUserCommand("john@email.com", "Password123", "Password123");
        _userRepository.GetUserByEmailAsync(Arg.Any<string>())
            .Returns((User?)null);
        _passwordHasher.HashPassword(command.Password)
            .Returns("hashedpassword");
        _userRepository.AddAsync(Arg.Any<User>())
            .Returns(ci => ci.Arg<User>());

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be("john@email.com");
        _passwordHasher.Received(1).HashPassword(command.Password);
        await _userRepository.Received(1).AddAsync(Arg.Any<User>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_DuplicateEmail_DoesNotCallAddOrSave()
    {
        // Arrange
        var command = new CreateUserCommand("john@email.com", "Password123", "Password123");
        _userRepository.GetUserByEmailAsync(command.Email)
            .Returns(new User("john@email.com", "hashedpassword"));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
