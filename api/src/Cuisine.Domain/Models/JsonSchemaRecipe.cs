using System.Text.Json.Serialization;

namespace Cuisine.Domain.Models
{
    public class JsonRecipe
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("recipeIngredients")]
        public List<JsonRecipeIngredient>? RecipeIngredients { get; set; }

        [JsonPropertyName("steps")]
        public List<string>? Steps { get; set; }

        [JsonPropertyName("preparationTime")]
        public int PreparationTime { get; set; }

        [JsonPropertyName("cookingTime")]
        public int CookingTime { get; set; }

        [JsonPropertyName("restTime")]
        public int RestTime { get; set; }

        [JsonPropertyName("portions")]
        public int Portions { get; set; }

        [JsonPropertyName("advice")]
        public string? Advice { get; set; }
    }

    public class JsonRecipeIngredient
    {
        [JsonPropertyName("ingredient")]
        public string? IngredientName { get; set; }

        [JsonPropertyName("quantity")]
        public object? Quantity { get; set; }

        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [JsonPropertyName("optional")]
        public bool? Optional { get; set; }
    }
}