using Cuisine.Application.Interfaces;
using Cuisine.Application.Services;
using Cuisine.Domain.Entities;
using Moq;
using Xunit;

namespace Cuisine.Test;

public class UnitServiceTests
{
    private Mock<IUnitRepository> _unitRepository;
    private IUnitService _unitService;

    public UnitServiceTests()
    {
        _unitRepository = new Mock<IUnitRepository>();
        _unitService = new UnitService(_unitRepository.Object);
    }

    [Fact]
    public async Task AddUnitAsync_ShouldReturnAddedUnit()
    {
        // Arrange
        var unit = new Unit
        {
            Id = Guid.NewGuid(),
            Name = "test",
        };

        _unitRepository.Setup(x => x.AddUnitAsync(unit)).ReturnsAsync(unit);

        // Act
        var addedUnit = await _unitService.AddUnitAsync(unit);

        // Assert
        Assert.Equal(unit, addedUnit);
    }

    [Fact]
    public async Task DeleteUnitAsync_ShouldCallDeleteUnitAsync()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _unitService.DeleteUnitAsync(id);

        // Assert
        _unitRepository.Verify(x => x.DeleteUnitAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetUnitsAsync_ShouldReturnUnits()
    {
        // Arrange
        var units = new List<Unit>
        {
            new Unit
            {
                Id = Guid.NewGuid(),
                Name = "test1",
            },
            new Unit
            {
                Id = Guid.NewGuid(),
                Name = "test2",
            }
        };

        _unitRepository.Setup(x => x.GetUnitsAsync(null, null)).ReturnsAsync(units);

        // Act
        var result = await _unitService.GetUnitsAsync();

        // Assert
        Assert.Equal(units, result);
    }

    [Fact]
    public async Task GetUnitByIdAsync_ShouldReturnUnit()
    {
        // Arrange
        var id = Guid.NewGuid();
        var unit = new Unit
        {
            Id = id,
            Name = "test",
        };

        _unitRepository.Setup(x => x.GetUnitByIdAsync(id)).ReturnsAsync(unit);

        // Act
        var result = await _unitService.GetUnitByIdAsync(id);

        // Assert
        Assert.Equal(unit, result);
    }

    [Fact]
    public async Task UpdateUnitAsync_ShouldReturnUpdatedUnit()
    {
        // Arrange
        var id = Guid.NewGuid();
        var unit = new Unit
        {
            Id = id,
            Name = "test",
        };

        _unitRepository.Setup(x => x.UpdateUnitAsync(id, unit)).ReturnsAsync(unit);

        // Act
        var result = await _unitService.UpdateUnitAsync(id, unit);

        // Assert
        Assert.Equal(unit, result);
    }
}
