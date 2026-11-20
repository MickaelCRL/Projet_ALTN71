using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RepairIT.Shared.Users;
using Shouldly;

namespace RepairIT.UserService.Tests;

public class UserServiceTests
{
    private readonly Mock<UserContext> _userContextMock;
    private readonly Services.UserService _userService;

    public UserServiceTests()
    {
        _userContextMock = new Mock<UserContext>(new DbContextOptions<UserContext>());
        _userService = new Services.UserService(_userContextMock.Object);
    }

    #region GetAll

    [Theory, AutoData]
    public async Task GetAllAsync_UsersExist_ReturnAllUsers(List<User> users)
    {
        // Arrange
        _userContextMock.Setup(c => c.Users).ReturnsDbSet(users);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(users);
    }

    #endregion

    #region GetById

    [Theory, AutoData]
    public async Task GetByIdAsync_UserExist_ReturnUser(User user)
    {
        // Arrange
        _userContextMock.Setup(c => c.Users).ReturnsDbSet([user]);

        // Act
        var result = await _userService.GetByIdAsync(user.Id);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(user);
    }

    [Theory, AutoData]
    public async Task GetByIdAsync_UserNotExist_ReturnFailure(Guid id)
    {
        // Arrange
        _userContextMock.Setup(c => c.Users).ReturnsDbSet([]);

        // Act
        var result = await _userService.GetByIdAsync(id);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(UserErrors.CouldNotFindUserById);
    }

    #endregion

    #region Add

    [Theory, AutoData]
    public async Task AddAsync_NewEmail_CreateUser(User user)
    {
        // Arrange
        _userContextMock.Setup(c => c.Users).ReturnsDbSet([]);

        // Act
        var result = await _userService.AddAsync(user);

        // Assert
        _userContextMock.Verify(c => c.Users.Add(user), Times.Once);
        _userContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(user);
    }

    [Theory, AutoData]
    public async Task AddAsync_EmailAlreadyUsed_ReturnFailure(User existingUser, User newUser)
    {
        // Arrange
        newUser.Email = existingUser.Email;
        _userContextMock.Setup(c => c.Users).ReturnsDbSet([existingUser]);

        // Act
        var result = await _userService.AddAsync(newUser);

        // Assert
        _userContextMock.Verify(c => c.Users.Add(It.IsAny<User>()), Times.Never);
        _userContextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(UserErrors.EmailAlreadyUsed);
    }

    [Fact]
    public void NewUser_KeycloakNotIntegrated_HasDefaultKeycloakUserId()
    {
        // Act
        var user = new User();

        // Assert
        user.KeycloakUserId.ShouldBe(Guid.Empty);
    }

    #endregion
}
