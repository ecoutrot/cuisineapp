using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Cuisine.Domain.Entities;
using Cuisine.Domain.Interfaces;

namespace Cuisine.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{

    public async Task<UserDTO?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
            return null;
        return user.ToDTO();
    }
}
