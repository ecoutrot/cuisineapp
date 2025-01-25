using Cuisine.Application.DTOs;
using Cuisine.Application.DTOs.Mappers;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuisine.Api.Controllers;

[ApiController]
[Authorize]
[ValidateInputs]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Action filter that could be added either on method or controller to ensure that Model state validation method is called before executing
    /// </summary>
    public class ValidateInputsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<UserDTO>,NotFound,BadRequest<string>>> GetById([FromRoute] Guid id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user is null)
            {
                return TypedResults.NotFound();
            }
            var userDTO = user.ToUserDTO();
            return TypedResults.Ok(userDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}
