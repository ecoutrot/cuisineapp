using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cuisine.Application.Helpers;

namespace Cuisine.Application.DTOs;

public sealed record UserDTO {
    [Required]
    [JsonPropertyName("id")]
    [NotEmptyGuidValidation]
    public Guid Id { get; init; }

    [Required]
    [JsonPropertyName("username")]
    [MinLength(3)]
    public required string Username { get; init; }
}

public sealed record NewUserDTO {
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [Required]
    [JsonPropertyName("username")]
    [MinLength(3)]
    public required string Username { get; init; }
}