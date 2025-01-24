using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IIngredientRepository
{
    Task<List<Ingredient>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null);
    Task<Ingredient?> GetIngredientByIdAsync(Guid id);
    Task<Ingredient?> AddIngredientAsync(Ingredient ingredient);
    Task<Ingredient?> UpdateIngredientAsync(Guid id, Ingredient ingredient);
    Task DeleteIngredientAsync(Guid id);
}
