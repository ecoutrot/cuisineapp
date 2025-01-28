using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cuisine.Application.Helpers;

namespace Cuisine.Application.DTOs;

public sealed record RecipeIngredientDTO {
    [Required]
    [JsonPropertyName("id")]
    [NotEmptyGuidValidation]
    public required Guid Id { get; set; }

    [JsonPropertyName("ingredientId")]
    public Guid IngredientId { get; init; }

    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; init; }

    [JsonPropertyName("unitId")]
    public Guid? UnitId { get; init; }

    [JsonPropertyName("optional")]
    public bool? Optional { get; init; }
}

public sealed record NewRecipeIngredientDTO {
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("ingredientId")]
    public Guid IngredientId { get; init; }

    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; init; }

    [JsonPropertyName("unitId")]
    public Guid? UnitId { get; init; }

    [JsonPropertyName("optional")]
    public bool? Optional { get; init; }
}
