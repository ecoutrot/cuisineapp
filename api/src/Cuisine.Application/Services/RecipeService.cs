using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Domain.Interfaces;

namespace Cuisine.Application.Services;

public class RecipeService(IRecipeRepository recipeRepository) : IRecipeService
{
    public async Task<RecipeDTO?> AddRecipeAsync(NewRecipeDTO newRecipeDTO, Guid userId)
    {
        var addedRecipe = await recipeRepository.AddRecipeAsync(newRecipeDTO.ToNewEntity());
        if (addedRecipe == null)
            return null;
        addedRecipe.UserId = userId;
        return addedRecipe.ToDTO();
    }

    public async Task DeleteRecipeAsync(Guid userId, Guid id)
    {
       await recipeRepository.DeleteRecipeAsync(userId, id);
    }

    public async Task<List<RecipeDTO>?> GetRecipesAsync(Guid userId, int? page = null, int? limit = null, string? search = null)
    {
        var recipes = await recipeRepository.GetRecipesAsync(userId, page, limit, search);
        if (recipes == null)
            return null;
        return recipes.Select(recipe => recipe.ToDTO()).ToList();
    }

    public async Task<RecipeDTO?> GetRecipeByIdAsync(Guid id)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(id);
        if (recipe == null)
            return null;
        return recipe.ToDTO();
    }

    public async Task<RecipeDTO?> UpdateRecipeAsync(Guid id, RecipeDTO recipeDTO)
    {
        var updatedRecipe = await recipeRepository.UpdateRecipeAsync(id, recipeDTO.ToEntity());
        if (updatedRecipe == null)
            return null;
        return updatedRecipe.ToDTO();
    }
}
