namespace Cuisine.Domain.Entities;

public class Recipe
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<RecipeIngredient>? RecipeIngredients { get; set; }
    public string? Steps { get; set; } = string.Empty;
    public Guid? RecipeCategoryId { get; set; }
    public RecipeCategory? RecipeCategory { get; set; }
    public int? PreparationTime { get; set; }
    public int? CookingTime { get; set; }
    public int? RestTime { get; set; }
    public int? Portions { get; set; }
    public int? Difficulty { get; set; }
    public int? Price { get; set; }
    public int? CookingType { get; set; }
    public int? Calories { get; set; }
    public string? Advice { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public bool IsPublic { get; set; } = false;
}