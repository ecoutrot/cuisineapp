using Cuisine.Domain.Entities;
using System.Text.Json;

namespace Cuisine.Application.DTOs.Mappers;

public static class RecipeMapper
{
    public static RecipeDTO ToDTO(this Recipe recipe)
    {
        return new RecipeDTO
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            RecipeIngredients = recipe.RecipeIngredients?.Select(ri => ri.ToDTO()).ToList(),
            Steps = JsonSerializer.Deserialize<List<string>>(recipe.Steps ?? string.Empty),
            RecipeCategoryId = recipe.RecipeCategoryId,
            PreparationTime = recipe.PreparationTime,
            CookingTime = recipe.CookingTime,
            RestTime = recipe.RestTime,
            Portions = recipe.Portions,
            Difficulty = recipe.Difficulty,
            Price = recipe.Price,
            CookingType = recipe.CookingType,
            Calories = recipe.Calories,
            Advice = recipe.Advice,
            UserId = recipe.UserId
        };
    }

    public static NewRecipeDTO ToRecipeDTO(NewRecipeDTO newRecipeDTO, Guid userId)
    {
        return new NewRecipeDTO
        {
            Id = Guid.NewGuid(),
            Name = newRecipeDTO.Name,
            Description = newRecipeDTO.Description,
            RecipeIngredients = newRecipeDTO.RecipeIngredients,
            Steps = newRecipeDTO.Steps,
            RecipeCategoryId = newRecipeDTO.RecipeCategoryId,
            PreparationTime = newRecipeDTO.PreparationTime,
            CookingTime = newRecipeDTO.CookingTime,
            RestTime = newRecipeDTO.RestTime,
            Portions = newRecipeDTO.Portions,
            Difficulty = newRecipeDTO.Difficulty,
            Price = newRecipeDTO.Price,
            CookingType = newRecipeDTO.CookingType,
            Calories = newRecipeDTO.Calories,
            Advice = newRecipeDTO.Advice,
            UserId = userId
        };
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

    public static Recipe ToNewEntity(this NewRecipeDTO newRecipeDTO)
    {
        return new Recipe
        {
            Id = Guid.NewGuid(),
            Name = newRecipeDTO.Name,
            Description = newRecipeDTO.Description,
            RecipeIngredients = newRecipeDTO.RecipeIngredients?.Select(ri => ri.ToNewEntity()).ToList(),
            Steps = JsonSerializer.Serialize(newRecipeDTO.Steps),
            RecipeCategoryId = newRecipeDTO.RecipeCategoryId,
            PreparationTime = newRecipeDTO.PreparationTime,
            CookingTime = newRecipeDTO.CookingTime,
            RestTime = newRecipeDTO.RestTime,
            Portions = newRecipeDTO.Portions,
            Difficulty = newRecipeDTO.Difficulty,
            Price = newRecipeDTO.Price,
            CookingType = newRecipeDTO.CookingType,
            Calories = newRecipeDTO.Calories,
            Advice = newRecipeDTO.Advice,
            UserId = newRecipeDTO.UserId
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
