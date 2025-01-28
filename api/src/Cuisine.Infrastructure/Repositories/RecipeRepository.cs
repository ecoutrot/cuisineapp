using Cuisine.Domain.Entities;
using Cuisine.Domain.Interfaces;
using Cuisine.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Cuisine.Infrastructure.Repositories;

public class RecipeRepository(UserDbContext context) : IRecipeRepository
{
    public async Task<Recipe?> AddRecipeAsync(Recipe recipe)
    {
        try
        {
            recipe.Id = Guid.NewGuid();
            if (recipe.RecipeIngredients is not null)
            {
                foreach (var recipeIngredient in recipe.RecipeIngredients)
                {
                    if (!await context.Ingredients.AnyAsync(i => i.Id == recipeIngredient.IngredientId))
                    {
                        throw new Exception($"Ingredient with ID {recipeIngredient.IngredientId} does not exist.");
                    }
                    if (recipeIngredient.UnitId is not null && !await context.Units.AnyAsync(u => u.Id == recipeIngredient.UnitId))
                    {
                        throw new Exception($"Unit with ID {recipeIngredient.UnitId} does not exist.");
                    }
                    recipeIngredient.Id = Guid.NewGuid();
                    var existingIngredient = await context.Ingredients.FirstOrDefaultAsync(i => i.Id == recipeIngredient.IngredientId);
                    if (existingIngredient is not null)
                    {
                        recipeIngredient.Ingredient = existingIngredient;
                    }
                    var existingUnit = await context.Units.FirstOrDefaultAsync(u => u.Id == recipeIngredient.UnitId);
                    if (existingUnit is not null)
                    {
                        recipeIngredient.Unit = existingUnit;
                    }
                }
            }
            context.Recipes.Add(recipe);
            await context.SaveChangesAsync();
            return recipe;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task DeleteRecipeAsync(Guid userId, Guid id)
    {
        try
        {
            var recipe = await context.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstAsync(r => r.Id == id && r.UserId == userId);
            context.Recipes.Remove(recipe);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return;
        }
    }

    public async Task<Recipe?> GetRecipeByIdAsync(Guid id)
    {
        try
        {
            var recipe = await context.Recipes
                .Include(ri => ri.RecipeIngredients!)
                    .ThenInclude(i => i.Ingredient)
                .Include(ri => ri.RecipeIngredients!)
                    .ThenInclude(u => u.Unit)
                .FirstOrDefaultAsync(r => r.Id == id);
            return recipe;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<Recipe>?> GetRecipesAsync(Guid userId, int? page = null, int? limit = null, string? search = null)
    {
        try
        {
            var query = context.Recipes
                .Include(ri => ri.RecipeIngredients!)
                    .ThenInclude(i => i.Ingredient)
                .Include(ri => ri.RecipeIngredients!)
                    .ThenInclude(u => u.Unit)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower().Trim();
                var arrayOfSearch = search.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                query = query.Where(r =>
                    arrayOfSearch.All(term =>
                        r.Name != null && r.Name.ToLower().Contains(term) ||
                        (
                            r.RecipeIngredients != null && r.RecipeIngredients.Any(i =>
                                i.Ingredient != null &&
                                i.Ingredient.Name != null && i.Ingredient.Name.ToLower().Contains(term)
                            )
                        )
                    )
                );
            }

            query = query.OrderBy(r => r.Name);
            if (page.HasValue && limit.HasValue)
            {
                int skip = (page.Value - 1) * limit.Value;
                query = query.Skip(skip).Take(limit.Value);
            }
            query = query.Where(r => r.UserId == userId);
            return await query.ToListAsync();
        }
        catch (Exception)
        {
            return null;
        }
    }


    public async Task<Recipe?> UpdateRecipeAsync(Guid id, Recipe recipe)
    {
        try
        {
            if (id != recipe.Id)
            {
                return null;
            }
            if (recipe.RecipeIngredients is not null)
            {
                foreach (var recipeIngredient in recipe.RecipeIngredients)
                {
                    if (!await context.Ingredients.AnyAsync(i => i.Id == recipeIngredient.IngredientId))
                    {
                        throw new Exception($"Ingredient with ID {recipeIngredient.IngredientId} does not exist.");
                    }
                    if (recipeIngredient.UnitId is not null && !await context.Units.AnyAsync(u => u.Id == recipeIngredient.UnitId))
                    {
                        throw new Exception($"Unit with ID {recipeIngredient.UnitId} does not exist.");
                    }
                }
            }
            var existingRecipe = await context.Recipes
                .Include(ri => ri.RecipeIngredients!)
                    .ThenInclude(i => i.Ingredient)
                .Include(ri => ri.RecipeIngredients!)
                    .ThenInclude(u => u.Unit)
                .FirstOrDefaultAsync(r => r.Id == recipe.Id);
            if (existingRecipe == null)
            {
                return null;
            }
            existingRecipe.Name = recipe.Name;
            existingRecipe.Description = recipe.Description;
            existingRecipe.RecipeIngredients = recipe.RecipeIngredients;
            existingRecipe.RecipeCategoryId = recipe.RecipeCategoryId;
            existingRecipe.RecipeCategory = recipe.RecipeCategory;
            existingRecipe.Steps = recipe.Steps;
            existingRecipe.PreparationTime = recipe.PreparationTime;
            existingRecipe.CookingTime = recipe.CookingTime;
            existingRecipe.RestTime = recipe.RestTime;
            existingRecipe.Portions = recipe.Portions;
            existingRecipe.Difficulty = recipe.Difficulty;
            existingRecipe.Price = recipe.Price;
            existingRecipe.CookingType = recipe.CookingType;
            existingRecipe.Calories = recipe.Calories;
            existingRecipe.Advice = recipe.Advice;

            await context.SaveChangesAsync();
            return existingRecipe;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
