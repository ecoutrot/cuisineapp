namespace Cuisine.Application.DTOs;

public sealed record RefreshTokenRequestDto (
    Guid UserId,
    string RefreshToken
);
