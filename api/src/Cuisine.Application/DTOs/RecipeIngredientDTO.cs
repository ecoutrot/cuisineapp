namespace Cuisine.Application.DTOs;

public sealed record RecipeIngredientDTO (
    Guid? Id,
    Guid IngredientId,
    decimal Quantity,
    Guid? UnitId,
    bool? Optional
);