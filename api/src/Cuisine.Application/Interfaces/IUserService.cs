using Cuisine.Domain.Entities;

namespace Cuisine.Application.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByIdAsync(Guid id);
}
