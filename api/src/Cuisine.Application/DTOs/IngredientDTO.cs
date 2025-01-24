namespace Cuisine.Application.DTOs;

public sealed record IngredientDTO (
    Guid? Id,
    string? Name,
    string? Description
);