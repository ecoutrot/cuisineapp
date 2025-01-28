using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cuisine.Application.DTOs;

public sealed record TokenResponseDTO 
{
    [Required]
    [JsonPropertyName("accessToken")]
    public required string AccessToken { get; init; }
}