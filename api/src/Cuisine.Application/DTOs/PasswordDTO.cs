namespace Cuisine.Application.DTOs;

public sealed record PasswordDTO (
    string oldPassword,
    string newPassword
);