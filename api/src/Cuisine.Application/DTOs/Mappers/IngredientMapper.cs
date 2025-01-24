using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class IngredientMapper
{
    public static IngredientDTO ToIngredientDTO(this Ingredient ingredient)
    {
        return new IngredientDTO
        (
            ingredient.Id,
            ingredient.Name,
            ingredient.Description
        );
    }

    public static Ingredient ToEntity(this IngredientDTO ingredientDTO)
    {
        return new Ingredient
        {
            Id = ingredientDTO.Id,
            Name = ingredientDTO.Name,
            Description = ingredientDTO.Description,
        };
    }
}