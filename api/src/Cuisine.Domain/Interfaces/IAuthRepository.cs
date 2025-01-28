using Cuisine.Domain.Models;

namespace Cuisine.Domain.Interfaces;

public interface IAuthRepository
{
    Task<Token?> RegisterAsync(string username, string password);
    Task<Token?> LoginAsync(string username, string password);
    Task<Token?> RefreshTokensAsync(Guid userId, string refreshToken);
    Task RemoveRefreshTokenAsync(Guid userId, string refreshToken);
    Task<Token?> ChangePasswordAsync(string oldPassword, string newPassword, Guid userId);
}