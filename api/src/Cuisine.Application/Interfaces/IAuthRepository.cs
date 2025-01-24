
using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IAuthRepository
{
    Task<User?> RegisterAsync(UserLoginDTO userLoginDTO);
    Task<TokenDto?> LoginAsync(UserLoginDTO userLoginDTO);
    Task<TokenDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto);
    Task RemoveRefreshTokenAsync(Guid? userId, string? refreshToken);
    Task<TokenDto?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId);
}