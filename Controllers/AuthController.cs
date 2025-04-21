using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talekhisi.Entities;
using Talekhisi.Models;
using Talekhisi.Services;

namespace Talekhisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
                return BadRequest("User already exists.");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var result = await authService.LoginAsync(request);
            return result is null
                ? BadRequest("Invalid username or password.")
                : Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);
            return result is null
                ? Unauthorized("Invalid or expired refresh token.")
                : Ok(result);
        }

        [Authorize]
        [HttpGet("authenticated")]
        public IActionResult AuthenticatedOnlyEndpoint() => Ok("You are authenticated!");

        [Authorize(Roles = "admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint() => Ok("You are an Admin!");
    }
}
