namespace Cuisine.Application.DTOs;

public sealed record TokenDto (
    Guid UserId,
    string AccessToken,
    string RefreshToken
);
