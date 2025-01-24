using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class UserMapper
{
    public static UserDTO ToUserDTO(this User user)
    {
        return new UserDTO
        (
            user.Id,
            user.Username
        );
    }

    public static User ToEntity(this UserDTO userDTO)
    {
        return new User
        {
            Id = userDTO.Id,
            Username = userDTO.Username ?? string.Empty,
        };
    }
}