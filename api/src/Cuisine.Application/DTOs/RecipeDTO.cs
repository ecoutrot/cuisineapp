
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cuisine.Application.Helpers;

namespace Cuisine.Application.DTOs;

public sealed record RecipeDTO {
    [Required]
    [JsonPropertyName("id")]
    [NotEmptyGuidValidation]
    public required Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("recipeIngredients")]
    public List<RecipeIngredientDTO>? RecipeIngredients { get; init; }

    [JsonPropertyName("steps")]
    public List<string>? Steps { get; init; }

    [JsonPropertyName("recipeCategoryId")]
    public Guid? RecipeCategoryId { get; init; }

    [JsonPropertyName("recipeCategory")]
    public string? RecipeCategory { get; init; }

    [JsonPropertyName("preparationTime")]
    public int? PreparationTime { get; init; }

    [JsonPropertyName("cookingTime")]
    public int? CookingTime { get; init; }

    [JsonPropertyName("restTime")]
    public int? RestTime { get; init; }

    [JsonPropertyName("portions")]
    public int? Portions { get; init; }

    [JsonPropertyName("difficulty")]
    public int? Difficulty { get; init; }

    [JsonPropertyName("price")]
    public int? Price { get; init; }

    [JsonPropertyName("cookingType")]
    public int? CookingType { get; init; }

    [JsonPropertyName("calories")]
    public int? Calories { get; init; }

    [JsonPropertyName("advice")]
    public string? Advice { get; init; }

    [JsonPropertyName("userId")]
    public Guid? UserId { get; init; }
}

public sealed record NewRecipeDTO {
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("recipeIngredients")]
    public List<NewRecipeIngredientDTO>? RecipeIngredients { get; init; }

    [JsonPropertyName("steps")]
    public List<string>? Steps { get; init; }

    [JsonPropertyName("recipeCategoryId")]
    public Guid? RecipeCategoryId { get; init; }

    [JsonPropertyName("recipeCategory")]
    public string? RecipeCategory { get; init; }

    [JsonPropertyName("preparationTime")]
    public int? PreparationTime { get; init; }

    [JsonPropertyName("cookingTime")]
    public int? CookingTime { get; init; }

    [JsonPropertyName("restTime")]
    public int? RestTime { get; init; }

    [JsonPropertyName("portions")]
    public int? Portions { get; init; }

    [JsonPropertyName("difficulty")]
    public int? Difficulty { get; init; }

    [JsonPropertyName("price")]
    public int? Price { get; init; }

    [JsonPropertyName("cookingType")]
    public int? CookingType { get; init; }

    [JsonPropertyName("calories")]
    public int? Calories { get; init; }

    [JsonPropertyName("advice")]
    public string? Advice { get; init; }

    [JsonPropertyName("userId")]
    public Guid? UserId { get; init; }
}