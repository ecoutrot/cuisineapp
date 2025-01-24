using Cuisine.Application.DTOs;
using Cuisine.Domain.Entities;
using Cuisine.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cuisine.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [Authorize]
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserLoginDTO userLoginDTO)
    {
        var user = await authService.RegisterAsync(userLoginDTO);
        if (user is null)
            return BadRequest("Username already exists.");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDTO userLoginDTO)
    {
        var result = await authService.LoginAsync(userLoginDTO);
        if (result is null)
            return BadRequest("Invalid username or password.");

        authService.SetTokenCookie(result, HttpContext);

        return Ok(result.AccessToken);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken()
    {
        Request.Cookies.TryGetValue("userId", out var userId);
        Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
        if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(userId))
        {
            authService.RemoveTokenCookie(HttpContext);
            return BadRequest("Invalid refresh token.");
        }
        var refreshTokenRequestDto = new RefreshTokenRequestDto(new Guid(userId), refreshToken);
        var result = await authService.RefreshTokensAsync(refreshTokenRequestDto);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
        {
            authService.RemoveTokenCookie(HttpContext);
            return BadRequest("Invalid refresh token.");
        }
        authService.SetTokenCookie(result, HttpContext);
        return Ok(result.AccessToken);
    }

    [Authorize]
    [HttpPost("passwordchange")]
    public async Task<ActionResult<TokenResponseDto>> PasswordChange(PasswordDTO passwordDTO)
    {
        var userId = GetUserId(User);
        if (userId is null)
        {
            return Unauthorized();
        }
        var result = await authService.ChangePasswordAsync(passwordDTO.oldPassword, passwordDTO.newPassword, userId.Value);
        if (result is null)
            return BadRequest("Password change failed.");

        authService.SetTokenCookie(result, HttpContext);

        return Ok(result.AccessToken);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = Request.Cookies["userId"];
        Response.Cookies.Delete("userId");
        var refreshToken = Request.Cookies["refreshToken"];
        Response.Cookies.Delete("refreshToken");
        if (!string.IsNullOrEmpty(userId))
        {
            await authService.RemoveRefreshTokenAsync(new Guid(userId), refreshToken);
        }
        return Ok("Logged out");
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

