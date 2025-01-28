using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IIngredientService
{
    Task<List<IngredientDTO>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null);
    Task<IngredientDTO?> GetIngredientByIdAsync(Guid id);
    Task<IngredientDTO?> AddIngredientAsync(NewIngredientDTO newIngredientDTO);
    Task<IngredientDTO?> UpdateIngredientAsync(Guid id, IngredientDTO ingredientDTO);
    Task DeleteIngredientAsync(Guid id);
}
