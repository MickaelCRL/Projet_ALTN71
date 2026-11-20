using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.Shared.Results;
using RepairIT.Shared.Users;
using RepairIT.UserService.Controllers;
using RepairIT.UserService.Services;
using Shouldly;

namespace RepairIT.UserService.Tests;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UsersController _usersController;

    public UsersControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _usersController = new UsersController(
            _userServiceMock.Object,
            NullLogger<UsersController>.Instance
        );
    }

    #region GetAll

    [Theory, AutoData]
    public async Task GetAll_UsersExist_ReturnOk(List<User> users)
    {
        // Arrange
        _userServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(Result.Success<IEnumerable<User>>(users));

        // Act
        var response = await _usersController.GetAll();

        // Assert
        var okResult = response.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(users);
    }

    #endregion

    #region GetById

    [Theory, AutoData]
    public async Task GetById_UserExist_ReturnOk(User user)
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var response = await _usersController.GetById(user.Id);

        // Assert
        var okResult = response.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(user);
    }

    [Theory, AutoData]
    public async Task GetById_UserNotExist_ReturnNotFound(Guid id)
    {
        // Arrange
        _userServiceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(UserErrors.CouldNotFindUserById);

        // Act
        var response = await _usersController.GetById(id);

        // Assert
        response.Result.ShouldBeOfType<NotFoundResult>();
    }

    #endregion

    #region Add

    [Theory, AutoData]
    public async Task Add_NewUser_ReturnCreated(User user)
    {
        // Arrange
        _userServiceMock.Setup(s => s.AddAsync(user)).ReturnsAsync(user);

        // Act
        var response = await _usersController.Add(user);

        // Assert
        var createdResult = response.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdResult.ActionName.ShouldBe(nameof(UsersController.GetById));
        createdResult.Value.ShouldBe(user);
    }

    [Theory, AutoData]
    public async Task Add_EmailAlreadyUsed_ReturnConflict(User user)
    {
        // Arrange
        _userServiceMock.Setup(s => s.AddAsync(user)).ReturnsAsync(UserErrors.EmailAlreadyUsed);

        // Act
        var response = await _usersController.Add(user);

        // Assert
        response.Result.ShouldBeOfType<ConflictObjectResult>();
    }

    #endregion
}
