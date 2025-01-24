using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;
using Cuisine.Infrastructure.Persistence.Data;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cuisine.Infrastructure.Repositories;

public class AuthRepository(UserDbContext context, IConfiguration configuration) : IAuthRepository
{
    public async Task<TokenDto?> LoginAsync(UserLoginDTO userLoginDTO)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == userLoginDTO.Username);
        if (user is null)
            return null;
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password)
            == PasswordVerificationResult.Failed)
            return null;

        return await CreateTokenResponse(user);
    }

    private async Task<TokenDto> CreateTokenResponse(User? user, string? refreshToken = null)
    {
        var userId = user?.Id ?? Guid.Empty;
        var AccessToken = CreateToken(user);
        var RefreshToken = refreshToken ?? await GenerateAndSaveRefreshTokenAsync(user);
        if (userId == Guid.Empty || string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(RefreshToken))
            throw new ArgumentNullException(nameof(user));
        return new TokenDto(userId, AccessToken, RefreshToken);
    }

    public async Task<User?> RegisterAsync(UserLoginDTO userLoginDTO)
    {
        if (await context.Users.AnyAsync(u => u.Username == userLoginDTO.Username))
            return null;

        var user = new User();
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, userLoginDTO.Password);

        user.Username = userLoginDTO.Username;
        user.PasswordHash = hashedPassword;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<TokenDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var user = await ValidateRefreshTokenAsync(refreshTokenRequestDto.UserId, refreshTokenRequestDto.RefreshToken);
        if (user is null)
            return null;
        var tokenHash = ToHashedToken(refreshTokenRequestDto.RefreshToken);
        var oldRefreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == refreshTokenRequestDto.UserId && rt.TokenHash == tokenHash);
        if (oldRefreshToken is null)
            return null;
        var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);
        return await CreateTokenResponse(user, refreshToken);
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null)
            return null;
        var tokenHash = ToHashedToken(refreshToken);
        var existingRefreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.TokenHash == tokenHash);
        if (existingRefreshToken is null || existingRefreshToken.ExpiryDate <= DateTime.UtcNow)
            return null;
        return user;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User? user, DateTime? ExpiryDate = null)
    {
        if(user?.Id is null)
            throw new ArgumentNullException(nameof(user));
        var refreshToken = GenerateRefreshToken();
        var refreshTokenHash = ToHashedToken(refreshToken);
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = (Guid)user.Id,
            TokenHash = refreshTokenHash,
            ExpiryDate = ExpiryDate ?? DateTime.UtcNow.AddDays(30)
        };
        context.RefreshTokens.Add(newRefreshToken);
        await context.SaveChangesAsync();
        return refreshToken;
    }

    private string CreateToken(User? user)
    {
        if(user is null)
            throw new ArgumentNullException(nameof(user));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public async Task RemoveRefreshTokenAsync(Guid? userId, string? refreshToken)
    {
        if (userId is null || string.IsNullOrEmpty(refreshToken))
            return;
        var tokenHash = new PasswordHasher<User>().HashPassword(new User(), refreshToken);
        var token = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.TokenHash == tokenHash);
        if (token is null)
            return;
        context.RefreshTokens.Remove(token);
        await context.SaveChangesAsync();
    }

    public async Task<TokenDto?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null)
            return null;
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, oldPassword)
            == PasswordVerificationResult.Failed)
            return null;
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, newPassword);
        await context.SaveChangesAsync();
        return await CreateTokenResponse(user);
    }

     
    private static string ToHashedToken(string token)
    {
        byte[] bytes = SHA512.HashData(Encoding.UTF8.GetBytes(token));

        StringBuilder builder = new();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }

}
