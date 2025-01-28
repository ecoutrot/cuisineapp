using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Domain.Interfaces;

namespace Cuisine.Application.Services;

public class RecipeCategoryService(IRecipeCategoryRepository recipeCategoryRepository) : IRecipeCategoryService
{
    public async Task<RecipeCategoryDTO?> AddRecipeCategoryAsync(NewRecipeCategoryDTO newRecipeCategoryDTO)
    {
        var addedRecipeCategory = await recipeCategoryRepository.AddRecipeCategoryAsync(newRecipeCategoryDTO.ToNewEntity());
        if (addedRecipeCategory == null)
            return null;
        return addedRecipeCategory.ToDTO();
    }

    public async Task DeleteRecipeCategoryAsync(Guid id)
    {
        await recipeCategoryRepository.DeleteRecipeCategoryAsync(id);
    }

    public async Task<List<RecipeCategoryDTO>?> GetRecipeCategoriesAsync(int? page = null, int? limit = null)
    {
        var tecipeCategories = await recipeCategoryRepository.GetRecipeCategoriesAsync(page, limit);
        if (tecipeCategories == null)
            return null;
        return tecipeCategories.Select(recipeCategory => recipeCategory.ToDTO()).ToList();
    }

    public async Task<RecipeCategoryDTO?> GetRecipeCategoryByIdAsync(Guid id)
    {
        var recipeCategory = await recipeCategoryRepository.GetRecipeCategoryByIdAsync(id);
        if (recipeCategory == null)
            return null;
        return recipeCategory.ToDTO();
    }

    public async Task<RecipeCategoryDTO?> UpdateRecipeCategoryAsync(Guid id, RecipeCategoryDTO recipeCategoryDTO)
    {
        var updatedRecipeCategory = await recipeCategoryRepository.UpdateRecipeCategoryAsync(id, recipeCategoryDTO.ToEntity());
        if (updatedRecipeCategory == null)
            return null;
        return updatedRecipeCategory.ToDTO();
    }
}
