using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IRecipeService
{
    public Task<List<RecipeDTO>?> GetRecipesAsync(Guid userId, int? page = null, int? limit = null, string? search = null);
    public Task<RecipeDTO?> GetRecipeByIdAsync(Guid id);
    public Task<RecipeDTO?> AddRecipeAsync(NewRecipeDTO newRecipeDTO, Guid userId);
    public Task<RecipeDTO?> UpdateRecipeAsync(Guid id, RecipeDTO recipeDTO);
    public Task DeleteRecipeAsync(Guid userId, Guid id);
}
