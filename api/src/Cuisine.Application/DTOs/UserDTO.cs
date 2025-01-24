namespace Cuisine.Application.DTOs;

public sealed record UserDTO (
    Guid? Id,
    string? Username
);