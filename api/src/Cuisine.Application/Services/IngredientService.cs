using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Domain.Interfaces;

namespace Cuisine.Application.Services;

public class IngredientService(IIngredientRepository ingredientRepository) : IIngredientService
{
    public async Task<IngredientDTO?> AddIngredientAsync(NewIngredientDTO newIngredientDTO)
    {
        var addedIngredient = await ingredientRepository.AddIngredientAsync(newIngredientDTO.ToNewEntity());
        if (addedIngredient == null)
            return null;
        return addedIngredient.ToDTO();
    }

    public async Task DeleteIngredientAsync(Guid id)
    {
        await ingredientRepository.DeleteIngredientAsync(id);
    }

    public async Task<List<IngredientDTO>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null)
    {
        var ingredients = await ingredientRepository.GetIngredientsAsync(page, limit, search);
        if (ingredients == null)
            return null;
        return ingredients.Select(ingredient => ingredient.ToDTO()).ToList();
    }

    public async Task<IngredientDTO?> GetIngredientByIdAsync(Guid id)
    {
        var ingredient = await ingredientRepository.GetIngredientByIdAsync(id);
        if (ingredient == null)
            return null;
        return ingredient.ToDTO();
    }

    public async Task<IngredientDTO?> UpdateIngredientAsync(Guid id, IngredientDTO ingredientDTO)
    {
        var updatedIngredient = await ingredientRepository.UpdateIngredientAsync(id, ingredientDTO.ToEntity());
        if (updatedIngredient == null)
            return null;
        return updatedIngredient.ToDTO();
    }
}
