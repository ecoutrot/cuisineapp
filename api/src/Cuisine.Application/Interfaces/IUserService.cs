using Cuisine.Application.DTOs;

namespace Cuisine.Application.Interfaces;

public interface IUserService
{
    Task<UserDTO?> GetUserByIdAsync(Guid id);
}
