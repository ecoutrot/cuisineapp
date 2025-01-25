using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Cuisine.Infrastructure.Repositories;

public class IngredientRepository(UserDbContext context): IIngredientRepository
{
    public async Task<Ingredient?> AddIngredientAsync(Ingredient ingredient)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
    }

    public async Task DeleteIngredientAsync(Guid id)
    {
        try
        {
            var ingredient = await context.Ingredients.FirstAsync(r => r.Id == id);
            context.Ingredients.Remove(ingredient);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return;
        }
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(Guid id)
    {
        try
        {
            var ingredient = await context.Ingredients.FindAsync(id);
            return ingredient;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<Ingredient>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<Ingredient?> UpdateIngredientAsync(Guid id, Ingredient ingredient)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
    }
}
