using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Services;

public class UnitService(IUnitRepository unitRepository) : IUnitService
{
    public async Task<Unit?> AddUnitAsync(Unit unit)
    {
        var addedUnit = await unitRepository.AddUnitAsync(unit);
        return addedUnit;
    }

    public async Task DeleteUnitAsync(Guid id)
    {
        await unitRepository.DeleteUnitAsync(id);
    }

    public async Task<List<Unit>?> GetUnitsAsync(int? page = null, int? limit = null)
    {
        var units = await unitRepository.GetUnitsAsync(page, limit);
        return units;
    }

    public async Task<Unit?> GetUnitByIdAsync(Guid id)
    {
        var unit = await unitRepository.GetUnitByIdAsync(id);
        return unit;
    }

    public async Task<Unit?> UpdateUnitAsync(Guid id, Unit unit)
    {
        var updatedUnit = await unitRepository.UpdateUnitAsync(id, unit);
        return updatedUnit;
    }
}
