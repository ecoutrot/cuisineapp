using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class RecipeIngredientMapper
{
    public static RecipeIngredientDTO ToDTO(this RecipeIngredient recipeIngredient)
    {
        return new RecipeIngredientDTO
        {
            Id = recipeIngredient.Id,
            IngredientId = recipeIngredient.IngredientId,
            Quantity = recipeIngredient.Quantity,
            UnitId = recipeIngredient.UnitId,
            Optional = recipeIngredient.Optional
        };
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

    public static RecipeIngredient ToNewEntity(this NewRecipeIngredientDTO newRecipeIngredientDTO)
    {
        return new RecipeIngredient
        {
            Id = Guid.NewGuid(),
            IngredientId = newRecipeIngredientDTO.IngredientId,
            Quantity = newRecipeIngredientDTO.Quantity,
            UnitId = newRecipeIngredientDTO.UnitId,
            Optional = newRecipeIngredientDTO.Optional
        };
    }
}