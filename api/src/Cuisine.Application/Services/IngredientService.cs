using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Services;

public class IngredientService(IIngredientRepository ingredientRepository) : IIngredientService
{
    public async Task<Ingredient?> AddIngredientAsync(Ingredient ingredient)
    {
        var addedIngredient = await ingredientRepository.AddIngredientAsync(ingredient);
        return addedIngredient;
    }

    public async Task DeleteIngredientAsync(Guid id)
    {
        await ingredientRepository.DeleteIngredientAsync(id);
    }

    public async Task<List<Ingredient>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null)
    {
        var ingredients = await ingredientRepository.GetIngredientsAsync(page, limit, search);
        return ingredients;
    }

    public async Task<Ingredient?> GetIngredientByIdAsync(Guid id)
    {
        var ingredient = await ingredientRepository.GetIngredientByIdAsync(id);
        return ingredient;
    }

    public async Task<Ingredient?> UpdateIngredientAsync(Guid id, Ingredient ingredient)
    {
        var updatedIngredient = await ingredientRepository.UpdateIngredientAsync(id, ingredient);
        return updatedIngredient;
    }
}
