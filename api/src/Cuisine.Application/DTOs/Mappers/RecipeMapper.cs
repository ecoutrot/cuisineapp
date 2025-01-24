using Cuisine.Domain.Entities;
using System.Text.Json;

namespace Cuisine.Application.DTOs.Mappers;

public static class RecipeMapper
{
    public static RecipeDTO ToRecipeDTO(this Recipe recipe)
    {
        return new RecipeDTO
        (
            recipe.Id,
            recipe.Name,
            recipe.Description,
            recipe.RecipeIngredients?.Select(ri => ri.ToRecipeIngredientDTO()).ToList(),
            (recipe.Steps != null) ? JsonSerializer.Deserialize<List<string>>(recipe.Steps) : null,
            recipe.RecipeCategoryId,
            recipe.RecipeCategory?.Name,
            recipe.PreparationTime,
            recipe.CookingTime,
            recipe.RestTime,
            recipe.Portions,
            recipe.Difficulty,
            recipe.Price,
            recipe.CookingType,
            recipe.Calories,
            recipe.Advice,
            recipe.UserId
        );
    }

    public static Recipe ToEntity(this RecipeDTO recipeDTO)
    {
        return new Recipe
        {
            Id = recipeDTO.Id,
            Name = recipeDTO.Name,
            Description = recipeDTO.Description,
            RecipeIngredients = recipeDTO.RecipeIngredients?.Select(ri => ri.ToEntity()).ToList(),
            Steps = JsonSerializer.Serialize(recipeDTO.Steps),
            RecipeCategoryId = recipeDTO.RecipeCategoryId,
            PreparationTime = recipeDTO.PreparationTime,
            CookingTime = recipeDTO.CookingTime,
            RestTime = recipeDTO.RestTime,
            Portions = recipeDTO.Portions,
            Difficulty = recipeDTO.Difficulty,
            Price = recipeDTO.Price,
            CookingType = recipeDTO.CookingType,
            Calories = recipeDTO.Calories,
            Advice = recipeDTO.Advice,
            UserId = recipeDTO.UserId
        };
    }

    public static string RecipeToString(Recipe? recipe)
    {
        if (recipe == null)
        {
            return string.Empty;
        }
        var stringRecipe = "Nom : " + recipe.Name + "\n";
        stringRecipe += "Description : " + recipe.Description + "\n";
        stringRecipe += "Ingrédients : \n";
        if (recipe.RecipeIngredients != null)
        {
            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                stringRecipe += recipeIngredient.Ingredient?.Name + " : " + recipeIngredient.Quantity + " " + recipeIngredient.Unit?.Name + "\n";
            }
        }
        stringRecipe += "Etapes : \n";
        if (recipe.Steps != null)
        {
            foreach (var step in recipe.Steps)
            {
                stringRecipe += step + "\n";
            }
        }
        stringRecipe += "Temps de préparation : " + recipe.PreparationTime + " minutes\n";
        stringRecipe += "Temps de cuisson : " + recipe.CookingTime + " minutes\n";
        stringRecipe += "Temps de repos : " + recipe.RestTime + " minutes\n";
        stringRecipe += "Nombre de parts : " + recipe.Portions + "\n";
        stringRecipe += "Conseils : " + recipe.Advice + "\n";

        return stringRecipe;
    }
}
