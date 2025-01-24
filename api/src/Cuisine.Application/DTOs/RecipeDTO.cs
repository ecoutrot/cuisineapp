
namespace Cuisine.Application.DTOs;

public sealed record RecipeDTO (
    Guid? Id,
    string? Name,
    string? Description,
    List<RecipeIngredientDTO>? RecipeIngredients,
    List<string>? Steps,
    Guid? RecipeCategoryId,
    string? RecipeCategory,
    int? PreparationTime,
    int? CookingTime,
    int? RestTime,
    int? Portions,
    int? Difficulty,
    int? Price,
    int? CookingType,
    int? Calories,
    string? Advice,
    Guid? UserId
);
