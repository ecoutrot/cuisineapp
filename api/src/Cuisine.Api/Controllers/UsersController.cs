using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuisine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDTO>> GetById([FromRoute] Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        var userDTO = user.ToUserDTO();
        return Ok(userDTO);
    }
}
