using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.CatalogService.Controllers;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Computers;
using RepairIT.Shared.Results;
using Shouldly;

namespace RepairIT.CatalogService.Tests;

public class ComputersControllerTests
{
    private readonly Mock<IComputerService> _computerServiceMock;
    private readonly ComputersController _computersController;

    public ComputersControllerTests()
    {
        _computerServiceMock = new Mock<IComputerService>();
        _computersController = new ComputersController(
            _computerServiceMock.Object,
            NullLogger<ComputersController>.Instance
        );
    }

    #region GetAll

    [Theory, AutoData]
    public async Task GetAll_ComputersExist_ReturnOk(List<Computer> computers)
    {
        // Arrange
        _computerServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(Result.Success<IEnumerable<Computer>>(computers));

        // Act
        var response = await _computersController.GetAll();

        // Assert
        var okResult = response.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(computers);
    }

    #endregion

    #region GetById

    [Theory, AutoData]
    public async Task GetById_ComputerExist_ReturnOk(Computer computer)
    {
        // Arrange
        _computerServiceMock.Setup(s => s.GetByIdAsync(computer.Id)).ReturnsAsync(computer);

        // Act
        var response = await _computersController.GetById(computer.Id);

        // Assert
        var okResult = response.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe(computer);
    }

    [Theory, AutoData]
    public async Task GetById_ComputerNotExist_ReturnNotFound(Guid id)
    {
        // Arrange
        _computerServiceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync(ComputerErrors.CouldNotFindComputerById);

        // Act
        var response = await _computersController.GetById(id);

        // Assert
        response.Result.ShouldBeOfType<NotFoundResult>();
    }

    #endregion

    #region Add

    [Theory, AutoData]
    public async Task Add_ValidComputer_ReturnCreated(Computer computer)
    {
        // Arrange
        _computerServiceMock.Setup(s => s.AddAsync(computer)).ReturnsAsync(computer);

        // Act
        var response = await _computersController.Add(computer);

        // Assert
        var createdResult = response.Result.ShouldBeOfType<CreatedAtActionResult>();
        createdResult.ActionName.ShouldBe(nameof(ComputersController.GetById));
        createdResult.Value.ShouldBe(computer);
    }

    #endregion

    #region Update

    [Theory, AutoData]
    public async Task Update_ComputerExist_ReturnOk(Computer computer)
    {
        // Arrange
        _computerServiceMock
            .Setup(s => s.UpdateAsync(computer.Id, computer))
            .ReturnsAsync(computer);

        // Act
        var response = await _computersController.Update(computer.Id, computer);

        // Assert
        response.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Theory, AutoData]
    public async Task Update_ComputerNotExist_ReturnNotFound(Computer computer)
    {
        // Arrange
        _computerServiceMock
            .Setup(s => s.UpdateAsync(computer.Id, computer))
            .ReturnsAsync(ComputerErrors.CouldNotFindComputerById);

        // Act
        var response = await _computersController.Update(computer.Id, computer);

        // Assert
        response.Result.ShouldBeOfType<NotFoundResult>();
    }

    [Theory, AutoData]
    public async Task Update_IdMismatch_ReturnBadRequest(Guid id, Computer computer)
    {
        // Act
        var response = await _computersController.Update(id, computer);

        // Assert
        response.Result.ShouldBeOfType<BadRequestResult>();
        _computerServiceMock.Verify(
            s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Computer>()),
            Times.Never
        );
    }

    #endregion

    #region Delete

    [Theory, AutoData]
    public async Task Delete_ComputerExist_ReturnNoContent(Guid id)
    {
        // Arrange
        _computerServiceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(Result.Success());

        // Act
        var response = await _computersController.Delete(id);

        // Assert
        response.ShouldBeOfType<NoContentResult>();
    }

    [Theory, AutoData]
    public async Task Delete_ComputerNotExist_ReturnNotFound(Guid id)
    {
        // Arrange
        _computerServiceMock
            .Setup(s => s.DeleteAsync(id))
            .ReturnsAsync(Result.Failure(ComputerErrors.CouldNotFindComputerById));

        // Act
        var response = await _computersController.Delete(id);

        // Assert
        response.ShouldBeOfType<NotFoundResult>();
    }

    #endregion
}
