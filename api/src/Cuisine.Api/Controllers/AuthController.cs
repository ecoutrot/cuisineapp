using Cuisine.Application.DTOs;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Cuisine.Api.Controllers;

[ApiController]
[ValidateInputs]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
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

    [Authorize]
    [HttpPost("register")]
    public async Task<Results<Ok<TokenResponseDTO>, BadRequest<string>>> Register(UserLoginDTO userLoginDTO)
    {
        try
        {
            var user = await authService.RegisterAsync(userLoginDTO);
            if (user is null)
                return TypedResults.BadRequest("Username already exists.");
            var result = await authService.LoginAsync(userLoginDTO);
            if (result is null)
                return TypedResults.BadRequest("Invalid username or password.");
            authService.SetTokenCookie(result, HttpContext);
            var tokenResponseDTO = new TokenResponseDTO(){ AccessToken = result.AccessToken };
            return TypedResults.Ok(tokenResponseDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<Results<Ok<TokenResponseDTO>, BadRequest<string>>> Login(UserLoginDTO userLoginDTO)
    {
        try
        {
            var result = await authService.LoginAsync(userLoginDTO);
            if (result is null)
                return TypedResults.BadRequest("Invalid username or password.");

            authService.SetTokenCookie(result, HttpContext);
            var tokenResponseDTO = new TokenResponseDTO(){ AccessToken = result.AccessToken };
            return TypedResults.Ok(tokenResponseDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<Results<Ok<TokenResponseDTO>, BadRequest<string>>> RefreshToken()
    {
        try
        {
            Request.Cookies.TryGetValue("userId", out var userId);
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userId))
            {
                authService.RemoveTokenCookie(HttpContext);
                return TypedResults.BadRequest("Invalid refresh token.");
            }
            var refreshTokenRequestDTO = new RefreshTokenRequestDTO()
            {
                UserId = new Guid(userId),
                RefreshToken = refreshToken
            };
            var result = await authService.RefreshTokensAsync(refreshTokenRequestDTO);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                authService.RemoveTokenCookie(HttpContext);
                return TypedResults.BadRequest("Invalid refresh token.");
            }
            authService.SetTokenCookie(result, HttpContext);
            var tokenResponseDTO = new TokenResponseDTO(){ AccessToken = result.AccessToken };
            return TypedResults.Ok(tokenResponseDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("passwordchange")]
    public async Task<Results<Ok<TokenResponseDTO>,UnauthorizedHttpResult,BadRequest<string>>> PasswordChange(PasswordDTO passwordDTO)
    {
        try
        {
            var userId = GetUserId(User);
            if (userId is null)
            {
                return TypedResults.Unauthorized();
            }
            var result = await authService.ChangePasswordAsync(passwordDTO.OldPassword, passwordDTO.NewPassword, userId.Value);
            if (result is null)
                return TypedResults.BadRequest("Password change failed.");

            authService.SetTokenCookie(result, HttpContext);
            var tokenResponseDTO = new TokenResponseDTO(){ AccessToken = result.AccessToken };
            return TypedResults.Ok(tokenResponseDTO);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<Results<Ok<string>,BadRequest<string>>> Logout()
    {
        try
        {
            var userId = Request.Cookies["userId"];
            Response.Cookies.Delete("userId");
            var refreshToken = Request.Cookies["refreshToken"];
            Response.Cookies.Delete("refreshToken");
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(refreshToken))
            {
                await authService.RemoveRefreshTokenAsync(new Guid(userId), refreshToken);
            }
            return TypedResults.Ok("Logged out");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static Guid? GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return null;
        }
        return new Guid(userId);
    }

}

