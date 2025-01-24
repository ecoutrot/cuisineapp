using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
}