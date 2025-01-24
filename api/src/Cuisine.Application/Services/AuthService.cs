using Cuisine.Application.Interfaces;
using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Cuisine.Application.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<TokenDto?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId)
    {
        var tokenDto = await authRepository.ChangePasswordAsync(oldPassword, newPassword, userId);

        return tokenDto;
    }

    public async Task<TokenDto?> LoginAsync(UserLoginDTO userLoginDTO)
    {
        var tokenDto = await authRepository.LoginAsync(userLoginDTO);

        return tokenDto;
    }

    public async Task<TokenDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var tokenDto = await authRepository.RefreshTokensAsync(refreshTokenRequestDto);
        return tokenDto;
    }

    public async Task<User?> RegisterAsync(UserLoginDTO userLoginDTO)
    {
        var user = await authRepository.RegisterAsync(userLoginDTO);
        return user;
    }

    public async Task RemoveRefreshTokenAsync(Guid? userId, string? refreshToken)
    {
        await authRepository.RemoveRefreshTokenAsync(userId, refreshToken);
    }

    public void SetTokenCookie(TokenDto tokenDto, HttpContext httpContext)
    {
        var cookies = httpContext.Response.Cookies;
        cookies.Delete("userId");
        cookies.Delete("refreshToken");
        cookies.Append("userId", tokenDto.UserId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(30)
        });
        cookies.Append("refreshToken", tokenDto.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(30)
        });
    }

    public void RemoveTokenCookie(HttpContext httpContext)
    {
        var cookies = httpContext.Response.Cookies;
        cookies.Delete("userId");
        cookies.Delete("refreshToken");
    }
}