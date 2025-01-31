namespace Cuisine.Domain.Entities;

public class User
{
    public required Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<Recipe>? Recipes { get; set; }
}