using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<Recipe?> AddRecipeAsync(Recipe recipe)
    {
        var addedRecipe = await recipeRepository.AddRecipeAsync(recipe);
        return addedRecipe;
    }

    public async Task DeleteRecipeAsync(Guid id)
    {
        await recipeRepository.DeleteRecipeAsync(id);
    }

    public async Task<List<Recipe>?> GetRecipesAsync(Guid userId, int? page = null, int? limit = null, string? search = null)
    {
        var recipes = await recipeRepository.GetRecipesAsync(userId, page, limit, search);
        return recipes;
    }

    public async Task<Recipe?> GetRecipeByIdAsync(Guid id)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(id);
        return recipe;
    }

    public async Task<Recipe?> UpdateRecipeAsync(Guid id, Recipe recipe)
    {
        var updatedRecipe = await recipeRepository.UpdateRecipeAsync(id, recipe);
        return updatedRecipe;
    }
}
