using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cuisine.Application.Interfaces;

public interface IAuthService
{
    Task<TokenDto?> RegisterAsync(UserLoginDTO userLoginDTO);
    Task<TokenDto?> LoginAsync(UserLoginDTO userLoginDTO);
    Task<TokenDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto);
    Task RemoveRefreshTokenAsync(Guid? userId, string? refreshToken);
    void SetTokenCookie(TokenDto tokenDto, HttpContext httpContext);
    void RemoveTokenCookie(HttpContext httpContext);
    Task<TokenDto?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId);
}