using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Infrastructure.Persistence.Data;
using Cuisine.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Cuisine.Infrastructure.Repositories;

public class UnitRepository(UserDbContext context): IUnitRepository
{
    public async Task<Unit?> AddUnitAsync(Unit unit)
    {
        var existingUnit = await context.Units.FirstOrDefaultAsync(r => r.Name == unit.Name);
        if (existingUnit != null)
        {
            return existingUnit;
        }
        unit.Id = Guid.NewGuid();
        context.Units.Add(unit);
        await context.SaveChangesAsync();
        return unit;
    }

    public async Task DeleteUnitAsync(Guid id)
    {
        var unit = await context.Units.FirstOrDefaultAsync(r => r.Id == id);
        if (unit == null)
        {
            return;
        }
        context.Units.Remove(unit);
        await context.SaveChangesAsync();
    }

    public async Task<Unit?> GetUnitByIdAsync(Guid id)
    {
        var unit = await context.Units.FindAsync(id);
        return unit;
    }

    public async Task<List<Unit>?> GetUnitsAsync(int? page = null, int? limit = null)
    {
        var query = context.Units.AsQueryable();
        query = query.OrderBy(r => r.Name);
        if (page.HasValue && limit.HasValue)
        {
            int skip = (page.Value - 1) * limit.Value;
            query = query.Skip(skip).Take(limit.Value);
        }
        return await query.ToListAsync();
    }

    public async Task<Unit?> UpdateUnitAsync(Guid id, Unit unit)
    {
        if (id != unit.Id)
        {
            return null;
        }
        var existingUnit = await context.Units.FirstOrDefaultAsync(r => r.Name == unit.Name);
        if (existingUnit != null)
        {
            throw new Exception($"Ingredient with Name {existingUnit.Name} already exists.");
        }
        existingUnit = await context.Units.FirstOrDefaultAsync(r => r.Id == unit.Id);
        if (existingUnit == null)
        {
            return null;
        }
        context.Entry(existingUnit).CurrentValues.SetValues(unit);
        await context.SaveChangesAsync();
        return existingUnit;
    }
}
