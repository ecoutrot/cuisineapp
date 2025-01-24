using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Cuisine.Infrastructure.Repositories;

public class IngredientRepository(UserDbContext context): IIngredientRepository
{
    public async Task<Ingredient?> AddIngredientAsync(Ingredient ingredient)
    {
        var existingIngredient = await context.Ingredients.FirstOrDefaultAsync(r => r.Name == ingredient.Name);
        if (existingIngredient != null)
        {
            return existingIngredient;
        }
        ingredient.Id = Guid.NewGuid();
        context.Ingredients.Add(ingredient);
        await context.SaveChangesAsync();
        return ingredient;
    }

    public async Task DeleteIngredientAsync(Guid id)
    {
        var ingredient = await context.Ingredients.FirstOrDefaultAsync(r => r.Id == id);
        if (ingredient == null)
        {
            return;
        }
        context.Ingredients.Remove(ingredient);
        await context.SaveChangesAsync();
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(Guid id)
    {
        var ingredient = await context.Ingredients.FindAsync(id);
        return ingredient;
    }

    public async Task<List<Ingredient>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null)
    {
        var query = context.Ingredients.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower().Trim();
            var arrayOfSearch = search.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            query = query.Where(r =>
                arrayOfSearch.All(term => r.Name != null && r.Name.ToLower().Contains(term))
            );
        }
        query = query.OrderBy(r => r.Name);
        if (page.HasValue && limit.HasValue)
        {
            int skip = (page.Value - 1) * limit.Value;
            query = query.Skip(skip).Take(limit.Value);
        }
        return await query.ToListAsync();
    }

    public async Task<Ingredient?> UpdateIngredientAsync(Guid id, Ingredient ingredient)
    {
        if (id != ingredient.Id)
        {
            return null;
        }
        var existingIngredient = await context.Ingredients.FirstOrDefaultAsync(r => r.Name == ingredient.Name);
        if (existingIngredient != null)
        {
            throw new Exception($"Ingredient with Name {existingIngredient.Name} already exists.");
        }
        existingIngredient = await context.Ingredients.FirstOrDefaultAsync(r => r.Id == ingredient.Id);
        if (existingIngredient == null)
        {
            return null;
        }
        context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
        await context.SaveChangesAsync();
        return existingIngredient;
    }
}
