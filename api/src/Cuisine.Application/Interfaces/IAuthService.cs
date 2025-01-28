using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cuisine.Application.Interfaces;

public interface IAuthService
{
    Task<TokenDTO?> RegisterAsync(UserLoginDTO userLoginDTO);
    Task<TokenDTO?> LoginAsync(UserLoginDTO userLoginDTO);
    Task<TokenDTO?> RefreshTokensAsync(RefreshTokenRequestDTO refreshTokenRequestDTO);
    Task RemoveRefreshTokenAsync(Guid userId, string refreshToken);
    void SetTokenCookie(TokenDTO tokenDTO, HttpContext httpContext);
    void RemoveTokenCookie(HttpContext httpContext);
    Task<TokenDTO?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId);
}