using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class IngredientMapper
{
    public static IngredientDTO ToIngredientDTO(this Ingredient ingredient)
    {
        return new IngredientDTO
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Description = ingredient.Description,
        };
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

    public static Ingredient ToNewEntity(this NewIngredientDTO newIngredientDTO)
    {
        return new Ingredient
        {
            Id = Guid.NewGuid(),
            Name = newIngredientDTO.Name,
            Description = newIngredientDTO.Description,
        };
    }
}