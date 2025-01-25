using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cuisine.Application.DTOs;

public sealed record UserLoginDTO {
    [Required]
    [JsonPropertyName("username")]
    [MinLength(3)]
    public required string Username { get; init; }

    [Required]
    [JsonPropertyName("password")]
    [MinLength(12)]
    [MaxLength(24)]
    public required string Password { get; init; }
}