/**
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public List<RecipeIngredientDTO>? RecipeIngredients { get; set; }
    public List<string>? Steps { get; set; }
    public Guid? RecipeCategoryId { get; set; }
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
 */

import { RecipeIngredient } from "./RecipeIngredient";

export type Recipe = {
  id: string | null;
  name: string | null;
  description: string | null;
  recipeIngredients: RecipeIngredient[] | null;
  steps: string[] | null;
  recipeCategoryId: string | null;
  preparationTime: number | null;
  cookingTime: number | null;
  restTime: number | null;
  portions: number | null;
  difficulty: number | null;
  price: number | null;
  cookingType: number | null;
  calories: number | null;
  advice: string | null;
  userId: string | null;
};
