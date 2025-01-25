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
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == userLoginDTO.Username);
            if (user is null)
                return null;
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password)
                == PasswordVerificationResult.Failed)
                return null;

            return await CreateTokenResponse(user);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<TokenDto> CreateTokenResponse(User user, string? refreshToken = null)
    {
        try
        {
            var userId = user.Id;
            var AccessToken = CreateToken(user);
            var RefreshToken = refreshToken ?? await GenerateAndSaveRefreshTokenAsync(user);
            if (userId == Guid.Empty || string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(RefreshToken))
                throw new ArgumentNullException(nameof(user));
            return new TokenDto
            {
                UserId = userId,
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
        }
        catch (Exception)
        {
            throw new Exception(nameof(CreateTokenResponse));
        }
    }

    public async Task<TokenDto?> RegisterAsync(UserLoginDTO userLoginDTO)
    {
        try
        {
            if (await context.Users.AnyAsync(u => u.Username == userLoginDTO.Username))
                return null;

            var user = new User { Id = Guid.NewGuid() };
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, userLoginDTO.Password);

            user.Username = userLoginDTO.Username;
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return await CreateTokenResponse(user);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<TokenDto?> RefreshTokensAsync(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        try
        {
            var user = await ValidateRefreshTokenAsync(refreshTokenRequestDto.UserId, refreshTokenRequestDto.RefreshToken);
            if (user is null)
                return null;
            var tokenHash = ToHashedToken(refreshTokenRequestDto.RefreshToken);
            var oldRefreshToken = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == refreshTokenRequestDto.UserId && rt.TokenHash == tokenHash);
            if (oldRefreshToken is null)
                return null;
            // await RemoveRefreshTokenAsync(refreshTokenRequestDto.UserId, refreshTokenRequestDto.RefreshToken);
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);
            return await CreateTokenResponse(user, refreshToken);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
    }

    private string GenerateRefreshToken()
    {
        try
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        catch (Exception)
        {
            throw new Exception(nameof(GenerateRefreshToken));
        }
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User? user, DateTime? ExpiryDate = null)
    {
        try
        {
            if(user?.Id is null)
                throw new ArgumentNullException(nameof(user));
            var refreshToken = GenerateRefreshToken();
            var refreshTokenHash = ToHashedToken(refreshToken);
            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                ExpiryDate = ExpiryDate ?? DateTime.UtcNow.AddDays(30)
            };
            context.RefreshTokens.Add(newRefreshToken);
            await context.SaveChangesAsync();
            return refreshToken;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private string CreateToken(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.NameIdentifier, user.Id.ToString() ?? string.Empty),
                new(ClaimTypes.Role, user.Role)
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
        catch (Exception)
        {
            throw new Exception(nameof(CreateToken));
        }
    }

    public async Task RemoveRefreshTokenAsync(Guid? userId, string? refreshToken)
    {
        try
        {
            if (userId is null || string.IsNullOrEmpty(refreshToken))
                return;
            var tokenHash = ToHashedToken(refreshToken);
            var token = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.TokenHash == tokenHash);
            if (token is null)
                return;
            context.RefreshTokens.Remove(token);
            var expiredRefreshToken = await context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.ExpiryDate <= DateTime.UtcNow)
                .ToArrayAsync();
            if (expiredRefreshToken is not null)
                context.RefreshTokens.RemoveRange(expiredRefreshToken);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return;
        }
    }

    public async Task<TokenDto?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId)
    {
        try
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
        catch (Exception)
        {
            return null;
        }
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
