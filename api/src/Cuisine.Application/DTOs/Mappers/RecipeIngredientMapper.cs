using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class RecipeIngredientMapper
{
    public static RecipeIngredientDTO ToRecipeIngredientDTO(this RecipeIngredient recipeIngredient)
    {
        return new RecipeIngredientDTO
        (
            recipeIngredient.Id,
            recipeIngredient.IngredientId,
            recipeIngredient.Quantity,
            recipeIngredient.UnitId,
            recipeIngredient.Optional
        );
    }

    public static RecipeIngredient ToEntity(this RecipeIngredientDTO recipeIngredientDTO)
    {
        return new RecipeIngredient
        {
            Id = recipeIngredientDTO.Id,
            IngredientId = recipeIngredientDTO.IngredientId,
            Quantity = recipeIngredientDTO.Quantity,
            UnitId = recipeIngredientDTO.UnitId,
            Optional = recipeIngredientDTO.Optional
        };
    }
}