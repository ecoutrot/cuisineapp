using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cuisine.Application.Helpers;

namespace Cuisine.Application.DTOs;

public sealed record RecipeCategoryDTO {
    [Required]
    [JsonPropertyName("id")]
    [NotEmptyGuidValidation]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

public sealed record NewRecipeCategoryDTO {
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}