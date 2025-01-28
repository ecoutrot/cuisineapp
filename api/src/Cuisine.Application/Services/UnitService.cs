using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Domain.Interfaces;

namespace Cuisine.Application.Services;

public class UnitService(IUnitRepository unitRepository) : IUnitService
{
    public async Task<UnitDTO?> AddUnitAsync(NewUnitDTO newUnitDTO)
    {
        var addedUnit = await unitRepository.AddUnitAsync(newUnitDTO.ToNewEntity());
        if (addedUnit == null)
            return null;
        return addedUnit.ToDTO();
    }

    public async Task DeleteUnitAsync(Guid id)
    {
        await unitRepository.DeleteUnitAsync(id);
    }

    public async Task<List<UnitDTO>?> GetUnitsAsync(int? page = null, int? limit = null)
    {
        var units = await unitRepository.GetUnitsAsync(page, limit);
        if (units == null)
            return null;
        return units.Select(unit => unit.ToDTO()).ToList();
    }

    public async Task<UnitDTO?> GetUnitByIdAsync(Guid id)
    {
        var unit = await unitRepository.GetUnitByIdAsync(id);
        if (unit == null)
            return null;
        return unit.ToDTO();
    }

    public async Task<UnitDTO?> UpdateUnitAsync(Guid id, UnitDTO unitDTO)
    {
        var updatedUnit = await unitRepository.UpdateUnitAsync(id, unitDTO.ToEntity());
        if (updatedUnit == null)
            return null;
        return updatedUnit.ToDTO();
    }
}
