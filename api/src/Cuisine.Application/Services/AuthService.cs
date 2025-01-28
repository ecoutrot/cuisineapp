using Cuisine.Application.Interfaces;
using Cuisine.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Cuisine.Domain.Interfaces;
using Cuisine.Application.DTOs.Mappers;

namespace Cuisine.Application.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<TokenDTO?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId)
    {
        var token = await authRepository.ChangePasswordAsync(oldPassword, newPassword, userId);
        if (token == null)
            return null;
        var tokenDTO = token.ToDTO();
        return tokenDTO;
    }

    public async Task<TokenDTO?> LoginAsync(UserLoginDTO userLoginDTO)
    {
        var token = await authRepository.LoginAsync(userLoginDTO.Username, userLoginDTO.Password);
        if (token == null)
            return null;
        var tokenDTO = token.ToDTO();
        return tokenDTO;
    }

    public async Task<TokenDTO?> RefreshTokensAsync(RefreshTokenRequestDTO refreshTokenRequestDTO)
    {
        var token = await authRepository.RefreshTokensAsync(refreshTokenRequestDTO.UserId, refreshTokenRequestDTO.RefreshToken);
        if (token == null)
            return null;
        var tokenDTO = token.ToDTO();
        return tokenDTO;
    }

    public async Task<TokenDTO?> RegisterAsync(UserLoginDTO userLoginDTO)
    {
        var token = await authRepository.RegisterAsync(userLoginDTO.Username, userLoginDTO.Password);
        if (token == null)
            return null;
        var tokenDTO = token.ToDTO();
        return tokenDTO;
    }

    public async Task RemoveRefreshTokenAsync(Guid userId, string refreshToken)
    {
        await authRepository.RemoveRefreshTokenAsync(userId, refreshToken);
    }

    public void SetTokenCookie(TokenDTO tokenDTO, HttpContext httpContext)
    {
        var cookies = httpContext.Response.Cookies;
        cookies.Append("userId", tokenDTO.UserId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(30)
        });
        cookies.Append("refreshToken", tokenDTO.RefreshToken, new CookieOptions
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