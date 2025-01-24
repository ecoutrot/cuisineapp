
// API_KEY="YOUR_API_KEY"

// curl \
//   -X POST https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key=${API_KEY} \
//   -H 'Content-Type: application/json' \
//   -d @<(echo '{
//   "contents": [
//     {
//       "role": "user",
//       "parts": [
//         {
//           "text": "INSERT_INPUT_HERE"
//         }
//       ]
//     }
//   ],
//   "generationConfig": {
//     "temperature": 1,
//     "topK": 40,
//     "topP": 0.95,
//     "maxOutputTokens": 8192,
//     "responseMimeType": "text/plain"
//   }
// }')


using System.Text;
using System.Text.Json;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.DTOs;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Application.Helpers;
using Cuisine.Domain.Models;

namespace Cuisine.Application.Services;

public class GeminiApiService(HttpClient httpClient, IIngredientRepository ingredientRepository, IUnitRepository unitRepository)
{
    public async Task<RecipeDTO?> GenerateContentAsync(string apiKey, string prompt, string? filePath = null)
    {
        var recipe = new Recipe();
        var ingredients = await ingredientRepository.GetIngredientsAsync();
        var units = await unitRepository.GetUnitsAsync();
        try
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-exp:generateContent?key={apiKey}";

            var contents = Contents(prompt, filePath);

            var finalContents = contents.ToArray();

            var payload = new
            {
                contents,
                generationConfig = new
                {
                    temperature = 1,
                    topK = 40,
                    topP = 0.95,
                    maxOutputTokens = 8192,
                    responseMimeType = "text/plain"
                }
            };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            string jsonRequest = JsonSerializer.Serialize(requestContent);
            Console.WriteLine("JSON Request:");
            Console.WriteLine(jsonRequest);
            Console.WriteLine(jsonPayload);
            var response = await httpClient.PostAsync(url, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode} - {response}");
            }
            var responseString = await response.Content.ReadAsStringAsync();
            try
            {
                ApiResponse? apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseString);
                string? innerJsonString = apiResponse?.Candidates?[0].Content?.Parts?[0].Text?.Trim();
                innerJsonString = innerJsonString?.Replace("`", "");
                innerJsonString = innerJsonString?.Replace("json", "");
                JsonRecipe? jsonRecipe = (innerJsonString is not null) ? JsonSerializer.Deserialize<JsonRecipe?>(innerJsonString) : null;

                if (jsonRecipe is not null)
                {
                    recipe.Name = jsonRecipe.Name;
                    recipe.Description = jsonRecipe.Description;
                    recipe.Steps = JsonSerializer.Serialize(jsonRecipe.Steps);
                    recipe.PreparationTime = jsonRecipe.PreparationTime;
                    recipe.CookingTime = jsonRecipe.CookingTime;
                    recipe.RestTime = jsonRecipe.RestTime;
                    recipe.Portions = jsonRecipe.Portions;
                    recipe.Advice = jsonRecipe.Advice;
                    recipe.RecipeIngredients = [];

                    if (jsonRecipe?.RecipeIngredients is not null)
                    {
                        foreach (var recipeIngredient in jsonRecipe.RecipeIngredients)
                        {
                            if (ingredients is not null && units is not null)
                            {
                                var ingredient = ingredients.FirstOrDefault(i => i.Name == recipeIngredient.IngredientName);
                                var unit = units.FirstOrDefault(u => u.Name == recipeIngredient.Unit);
                                ingredient ??= await ingredientRepository.AddIngredientAsync(new Ingredient { Id = Guid.NewGuid(), Name = recipeIngredient.IngredientName });
                                if (unit is null && recipeIngredient.Unit is not null && recipeIngredient.Unit != "")
                                {
                                    unit = await unitRepository.AddUnitAsync(new Unit { Id = Guid.NewGuid(), Name = recipeIngredient.Unit });
                                }
                                if (ingredient?.Id is not null)
                                {
                                    decimal quantityValue = 0m;
                                    if (recipeIngredient.Quantity is JsonElement quantityElement)
                                    {
                                        if (quantityElement.ValueKind == JsonValueKind.String && decimal.TryParse(quantityElement.GetString(), out var parsedValue))
                                        {
                                            quantityValue = parsedValue;
                                        }
                                        else if (quantityElement.ValueKind == JsonValueKind.Number)
                                        {
                                            quantityValue = quantityElement.GetDecimal();
                                        }
                                    }
                                    else if (recipeIngredient.Quantity is string strQuantity && decimal.TryParse(strQuantity, out var parsedValue))
                                    {
                                        quantityValue = parsedValue;
                                    }
                                    else if (recipeIngredient.Quantity is int intQuantity)
                                    {
                                        quantityValue = intQuantity;
                                    }

                                    var newRecipeIngredient = new RecipeIngredient()
                                    {
                                        IngredientId = (Guid)ingredient.Id,
                                        Quantity = quantityValue,
                                        UnitId = unit?.Id ?? null,
                                        Optional = recipeIngredient.Optional ?? false
                                    };
                                    recipe.RecipeIngredients.Add(newRecipeIngredient);
                                }
                            }

                        }
                    }
                }
                var recipeDTO = recipe.ToRecipeDTO();
                return recipeDTO;
            }
            catch (JsonException ex)
            {
                throw new Exception(responseString + "|| ||  ||" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(responseString + "|| ||  ||" + ex.Message);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private List<object> Contents(string prompt, string? filePath = null)
    {
        var contents = new List<object>
        {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        };

        if (filePath is not null)
        {
            string mimeType = MimeTypeHelper.GetMimeType(filePath);

            byte[] imageBytes;
            try
            {
                imageBytes = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur de lecture du fichier image : {ex.Message}");
            }
            string base64Image = Convert.ToBase64String(imageBytes);


            contents.Insert(0, new
            {
                parts = new[]
                {
                    new
                    {
                        inlineData = new
                        {
                            mimeType,
                            data = base64Image
                        }
                    }
                }
            });
        }

        return contents;
    }
}