using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IUnitService
{
    Task<List<UnitDTO>?> GetUnitsAsync(int? page = null, int? limit = null);
    Task<UnitDTO?> GetUnitByIdAsync(Guid id);
    Task<UnitDTO?> AddUnitAsync(NewUnitDTO newUnitDTO);
    Task<UnitDTO?> UpdateUnitAsync(Guid id, UnitDTO unitDTO);
    Task DeleteUnitAsync(Guid id);
}
