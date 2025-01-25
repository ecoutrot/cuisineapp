using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Cuisine.Infrastructure.Repositories;

public class RecipeCategoryRepository(UserDbContext context): IRecipeCategoryRepository
{
    public async Task<RecipeCategory?> AddRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        try
        {
            var existingRecipeCategory = await context.RecipeCategories.FirstOrDefaultAsync(r => r.Name == recipeCategory.Name);
            if (existingRecipeCategory != null)
            {
                return existingRecipeCategory;
            }
            recipeCategory.Id = Guid.NewGuid();
            context.RecipeCategories.Add(recipeCategory);
            await context.SaveChangesAsync();
            return recipeCategory;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task DeleteRecipeCategoryAsync(Guid id)
    {
        try
        {
            var recipeCategory = await context.RecipeCategories.FirstAsync(r => r.Id == id);
            context.RecipeCategories.Remove(recipeCategory);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return;
        }
    }

    public async Task<RecipeCategory?> GetRecipeCategoryByIdAsync(Guid id)
    {
        try
        {
            var recipeCategory = await context.RecipeCategories.FindAsync(id);
            return recipeCategory;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<RecipeCategory>?> GetRecipeCategoriesAsync(int? page = null, int? limit = null)
    {
        try
        {
            var query = context.RecipeCategories.AsQueryable();
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

    public async Task<RecipeCategory?> UpdateRecipeCategoryAsync(Guid id, RecipeCategory recipeCategory)
    {
        try
        {
            if (id != recipeCategory.Id)
            {
                return null;
            }
            var existingRecipeCategory = await context.RecipeCategories.FirstOrDefaultAsync(r => r.Name == recipeCategory.Name);
            if (existingRecipeCategory != null)
            {
                throw new Exception($"Ingredient with Name {existingRecipeCategory.Name} already exists.");
            }
            existingRecipeCategory = await context.RecipeCategories.FirstOrDefaultAsync(r => r.Id == recipeCategory.Id);
            if (existingRecipeCategory == null)
            {
                return null;
            }
            context.Entry(existingRecipeCategory).CurrentValues.SetValues(recipeCategory);
            await context.SaveChangesAsync();
            return existingRecipeCategory;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
