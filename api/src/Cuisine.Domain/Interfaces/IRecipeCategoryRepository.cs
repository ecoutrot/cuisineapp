using Cuisine.Domain.Entities;

namespace Cuisine.Domain.Interfaces;

public interface IRecipeCategoryRepository
{
    Task<List<RecipeCategory>?> GetRecipeCategoriesAsync(int? page = null, int? limit = null);
    Task<RecipeCategory?> GetRecipeCategoryByIdAsync(Guid id);
    Task<RecipeCategory?> AddRecipeCategoryAsync(RecipeCategory recipeCategory);
    Task<RecipeCategory?> UpdateRecipeCategoryAsync(Guid id, RecipeCategory recipeCategory);
    Task DeleteRecipeCategoryAsync(Guid id);
}