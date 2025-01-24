using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IRecipeRepository
{
    public Task<List<Recipe>?> GetRecipesAsync(Guid userId, int? page = null, int? limit = null, string? search = null);
    public Task<Recipe?> GetRecipeByIdAsync(Guid id);
    public Task<Recipe?> AddRecipeAsync(Recipe recipe);
    public Task<Recipe?> UpdateRecipeAsync(Guid id, Recipe recipe);
    public Task DeleteRecipeAsync(Guid id);
}
