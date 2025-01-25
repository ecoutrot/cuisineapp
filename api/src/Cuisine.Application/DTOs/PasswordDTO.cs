using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cuisine.Application.DTOs;

public sealed record PasswordDTO {
    [Required]
    [MinLength(12)]
    [MaxLength(24)]
    [JsonPropertyName("oldPassword")]
    public required string OldPassword { get; init; }

    [Required]
    [MinLength(12)]
    [MaxLength(24)]
    [JsonPropertyName("newPassword")]
    public required string NewPassword { get; init; }
}