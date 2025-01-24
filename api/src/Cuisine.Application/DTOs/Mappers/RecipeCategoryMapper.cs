using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class RecipeCategoryMapper
{
    public static RecipeCategoryDTO ToRecipeCategoryDTO(this RecipeCategory recipeCategory)
    {
        return new RecipeCategoryDTO
        (
            recipeCategory.Id,
            recipeCategory.Name
        );
    }

    public static RecipeCategory ToEntity(this RecipeCategoryDTO recipeCategoryDTO)
    {
        return new RecipeCategory
        {
            Id = recipeCategoryDTO.Id,
            Name = recipeCategoryDTO.Name,
        };
    }
}