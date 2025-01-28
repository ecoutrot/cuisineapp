namespace Cuisine.Domain.Models;

public class Token
{
    public required Guid UserId { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
