using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class RecipeCategoryMapper
{
    public static RecipeCategoryDTO ToDTO(this RecipeCategory recipeCategory)
    {
        return new RecipeCategoryDTO
        {
            Id = recipeCategory.Id,
            Name = recipeCategory.Name
        };
    }

    public static RecipeCategory ToEntity(this RecipeCategoryDTO recipeCategoryDTO)
    {
        return new RecipeCategory
        {
            Id = recipeCategoryDTO.Id,
            Name = recipeCategoryDTO.Name,
        };
    }

    public static RecipeCategory ToNewEntity(this NewRecipeCategoryDTO newRecipeCategoryDTO)
    {
        return new RecipeCategory
        {
            Id = Guid.NewGuid(),
            Name = newRecipeCategoryDTO.Name,
        };
    }
}