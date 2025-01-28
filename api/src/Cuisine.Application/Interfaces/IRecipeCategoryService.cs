using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IRecipeCategoryService
{
    Task<List<RecipeCategoryDTO>?> GetRecipeCategoriesAsync(int? page = null, int? limit = null);
    Task<RecipeCategoryDTO?> GetRecipeCategoryByIdAsync(Guid id);
    Task<RecipeCategoryDTO?> AddRecipeCategoryAsync(NewRecipeCategoryDTO newRecipeCategoryDTO);
    Task<RecipeCategoryDTO?> UpdateRecipeCategoryAsync(Guid id, RecipeCategoryDTO recipeCategoryDTO);
    Task DeleteRecipeCategoryAsync(Guid id);
}
