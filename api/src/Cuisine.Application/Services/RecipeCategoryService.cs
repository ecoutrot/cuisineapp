using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Services;

public class RecipeCategoryService(IRecipeCategoryRepository recipeCategoryRepository) : IRecipeCategoryService
{
    public async Task<RecipeCategory?> AddRecipeCategoryAsync(RecipeCategory recipeCategory)
    {
        var addedRecipeCategory = await recipeCategoryRepository.AddRecipeCategoryAsync(recipeCategory);
        return addedRecipeCategory;
    }

    public async Task DeleteRecipeCategoryAsync(Guid id)
    {
        await recipeCategoryRepository.DeleteRecipeCategoryAsync(id);
    }

    public async Task<List<RecipeCategory>?> GetRecipeCategoriesAsync(int? page = null, int? limit = null)
    {
        var tecipeCategories = await recipeCategoryRepository.GetRecipeCategoriesAsync(page, limit);
        return tecipeCategories;
    }

    public async Task<RecipeCategory?> GetRecipeCategoryByIdAsync(Guid id)
    {
        var recipeCategory = await recipeCategoryRepository.GetRecipeCategoryByIdAsync(id);
        return recipeCategory;
    }

    public async Task<RecipeCategory?> UpdateRecipeCategoryAsync(Guid id, RecipeCategory recipeCategory)
    {
        var updatedRecipeCategory = await recipeCategoryRepository.UpdateRecipeCategoryAsync(id, recipeCategory);
        return updatedRecipeCategory;
    }
}
