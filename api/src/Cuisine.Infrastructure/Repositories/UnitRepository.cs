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
        try
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
        catch (Exception)
        {
            return null;
        }
    }

    public async Task DeleteUnitAsync(Guid id)
    {
        try
        {
            var unit = await context.Units.FirstAsync(r => r.Id == id);
            context.Units.Remove(unit);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return;
        }
    }

    public async Task<Unit?> GetUnitByIdAsync(Guid id)
    {
        try
        {
            var unit = await context.Units.FindAsync(id);
            return unit;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<Unit>?> GetUnitsAsync(int? page = null, int? limit = null)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Unit?> UpdateUnitAsync(Guid id, Unit unit)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
    }
}
