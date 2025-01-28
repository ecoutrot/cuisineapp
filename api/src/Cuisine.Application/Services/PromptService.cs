
using System.Text.Json;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Interfaces;

namespace Cuisine.Application.Services;

public class PromptService(IRecipeRepository recipeRepository) : IPromptService
{
    public string PromoptIdea(string idea)
    {
        var question = $@"Tu es un grand cuisinier. Tu vas me donner une recette avec comme sujet, thème ou instruction : {idea}.
        Ta réponse doit uniquement être un JSON qui décrit la recette et qui suit exactement ce schéma (quantity doit obligatoirement etre un nombre, preparationTime, cookingTime, restTime et portions doivent obligatoirement être des entiers) : ";

        var prompt = question + JsonSchema();

        return prompt;
    }

    public string PromptImage(string title)
    {

        var question = $@"Tu es un grand cuisinier. Voici une image qui contient une recette écrite. En utilisant les textes que tu peux trouver sur cette image, tu vas me donner très exactement la recette indiquée. S'il y a plusieurs recettes donne moi celle avec comme sujet : {title}.
        Ta réponse doit uniquement être un JSON qui décrit la recette et qui suit exactement ce schéma (quantity doit obligatoirement etre un nombre, preparationTime, cookingTime, restTime et portions doivent obligatoirement être des entiers) : ";

        var prompt = question + JsonSchema();

        return prompt;
    }

    public async Task<string> PromptImproveAsync(Guid id)
    {
        var recipe = await recipeRepository.GetRecipeByIdAsync(id);
        
        var recipeToString = RecipeMapper.RecipeToString(recipe);

        var question = $@"Tu es un grand cuisinier. Voici une de mes recettes : {recipeToString}.
        J'aimerais que, si possible, tu l'améliores un peu. Ta réponse doit uniquement être un JSON qui décrit la recette et qui suit exactement ce schéma (quantity doit obligatoirement être un nombre, preparationTime, cookingTime, restTime et portions doivent obligatoirement être des entiers) : ";

        var prompt = question + JsonSchema();

        return prompt;
    }


    public string PromptListIngredients(string[] listIngredients)
    {
        var ingredients = "Voici une liste d'ingrédients : ";
        foreach (var ingredient in listIngredients)
        {
            ingredients += ingredient + ", ";
        }

        var question = $@"Tu es un grand cuisinier. {ingredients}
        Tu vas me donner une recette réalisable avec ces ingrédients. Ta réponse doit uniquement être un JSON qui décrit la recette et qui suit exactement ce schéma (quantity doit obligatoirement etre un nombre, preparationTime, cookingTime, restTime et portions doivent obligatoirement être des entiers) : ";

        var prompt = question + JsonSchema();

        var contents = new object[]
        {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        };

        return prompt;
    }

    private static string JsonSchema()
    {
        var schema = new
        {
            name = "string",
            description = "string",
            recipeIngredients = new[]
            {
                new
                {
                    ingredient = "string",
                    quantity = "decimal",
                    unit = "string",
                    optional = "bool"
                }
            },
            steps = new[] { "string" },
            preparationTime = "int",
            cookingTime = "int",
            restTime = "int",
            portions = "int",
            advice = "string"
        };

        var jsonSchema = JsonSerializer.Serialize(schema);

        return jsonSchema;
    }
}
