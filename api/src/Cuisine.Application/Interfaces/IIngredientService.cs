using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IIngredientService
{
    Task<List<Ingredient>?> GetIngredientsAsync(int? page = null, int? limit = null, string? search = null);
    Task<Ingredient?> GetIngredientByIdAsync(Guid id);
    Task<Ingredient?> AddIngredientAsync(Ingredient ingredient);
    Task<Ingredient?> UpdateIngredientAsync(Guid id, Ingredient ingredient);
    Task DeleteIngredientAsync(Guid id);
}
