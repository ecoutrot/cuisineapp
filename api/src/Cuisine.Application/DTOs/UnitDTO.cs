using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cuisine.Application.Helpers;

namespace Cuisine.Application.DTOs;

public sealed record UnitDTO {
    [Required]
    [JsonPropertyName("id")]
    [NotEmptyGuidValidation]
    public required Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

public sealed record NewUnitDTO {
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}