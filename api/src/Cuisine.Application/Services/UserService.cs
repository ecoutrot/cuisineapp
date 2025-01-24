using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;

namespace Cuisine.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        return user;
    }
}
