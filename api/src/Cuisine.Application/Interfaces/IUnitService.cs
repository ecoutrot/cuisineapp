using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IUnitService
{
    Task<List<Unit>?> GetUnitsAsync(int? page = null, int? limit = null);
    Task<Unit?> GetUnitByIdAsync(Guid id);
    Task<Unit?> AddUnitAsync(Unit unit);
    Task<Unit?> UpdateUnitAsync(Guid id, Unit unit);
    Task DeleteUnitAsync(Guid id);
}
