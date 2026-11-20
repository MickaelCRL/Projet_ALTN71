using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Computers;
using Shouldly;

namespace RepairIT.CatalogService.Tests;

public class ComputerServiceTests
{
    private readonly Mock<CatalogContext> _catalogContextMock;
    private readonly ComputerService _computerService;

    public ComputerServiceTests()
    {
        _catalogContextMock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
        _computerService = new ComputerService(_catalogContextMock.Object);
    }

    #region GetAll

    [Theory, AutoData]
    public async Task GetAllAsync_ComputersExist_ReturnAllComputers(List<Computer> computers)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet(computers);

        // Act
        var result = await _computerService.GetAllAsync();

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(computers);
    }

    #endregion

    #region GetById

    [Theory, AutoData]
    public async Task GetByIdAsync_ComputerExist_ReturnComputer(Computer computer)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet([computer]);

        // Act
        var result = await _computerService.GetByIdAsync(computer.Id);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(computer);
    }

    [Theory, AutoData]
    public async Task GetByIdAsync_ComputerNotExist_ReturnFailure(Guid id)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet([]);

        // Act
        var result = await _computerService.GetByIdAsync(id);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(ComputerErrors.CouldNotFindComputerById);
    }

    #endregion

    #region Add

    [Theory, AutoData]
    public async Task AddAsync_ValidComputer_CreateComputer(Computer computer)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers.Add(It.IsAny<Computer>()));

        // Act
        var result = await _computerService.AddAsync(computer);

        // Assert
        _catalogContextMock.Verify(c => c.Computers.Add(computer), Times.Once);
        _catalogContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(computer);
    }

    #endregion

    #region Update

    [Theory, AutoData]
    public async Task UpdateAsync_ComputerExist_UpdateComputer(
        Computer existingComputer,
        Computer updatedComputer
    )
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet([existingComputer]);

        // Act
        var result = await _computerService.UpdateAsync(existingComputer.Id, updatedComputer);

        // Assert
        _catalogContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldBe(existingComputer.Id);
        result.Value.Brand.ShouldBe(updatedComputer.Brand);
        result.Value.Price.ShouldBe(updatedComputer.Price);
        result.Value.Stock.ShouldBe(updatedComputer.Stock);
    }

    [Theory, AutoData]
    public async Task UpdateAsync_ComputerNotExist_ReturnFailure(Guid id, Computer updatedComputer)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet([]);

        // Act
        var result = await _computerService.UpdateAsync(id, updatedComputer);

        // Assert
        _catalogContextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(ComputerErrors.CouldNotFindComputerById);
    }

    #endregion

    #region Delete

    [Theory, AutoData]
    public async Task DeleteAsync_ComputerExist_DeleteComputer(Computer computer)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet([computer]);

        // Act
        var result = await _computerService.DeleteAsync(computer.Id);

        // Assert
        _catalogContextMock.Verify(c => c.Computers.Remove(computer), Times.Once);
        _catalogContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);

        result.IsSuccess.ShouldBeTrue();
    }

    [Theory, AutoData]
    public async Task DeleteAsync_ComputerNotExist_ReturnFailure(Guid id)
    {
        // Arrange
        _catalogContextMock.Setup(c => c.Computers).ReturnsDbSet([]);

        // Act
        var result = await _computerService.DeleteAsync(id);

        // Assert
        _catalogContextMock.Verify(c => c.Computers.Remove(It.IsAny<Computer>()), Times.Never);
        _catalogContextMock.Verify(c => c.SaveChangesAsync(default), Times.Never);

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(ComputerErrors.CouldNotFindComputerById);
    }

    #endregion
}
