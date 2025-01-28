using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cuisine.Application.Helpers;

namespace Cuisine.Application.DTOs;

public sealed record TokenDTO 
{
    [Required]
    [JsonPropertyName("userId")]
    [NotEmptyGuidValidation]
    public required Guid UserId { get; init; }

    [Required]
    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; init; }

    [Required]
    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; init; }
}