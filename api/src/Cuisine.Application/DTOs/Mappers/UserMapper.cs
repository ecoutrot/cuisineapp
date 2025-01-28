using Cuisine.Domain.Entities;

namespace Cuisine.Application.DTOs.Mappers;

public static class UserMapper
{
    public static UserDTO ToDTO(this User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public static User ToEntity(this UserDTO userDTO)
    {
        return new User
        {
            Id = userDTO.Id,
            Username = userDTO.Username ?? string.Empty,
        };
    }

    public static User ToNewEntity(this NewUserDTO newUserDTO)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = newUserDTO.Username,
        };
    }
}