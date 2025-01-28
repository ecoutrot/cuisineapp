using Cuisine.Domain.Entities;

namespace Cuisine.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
}