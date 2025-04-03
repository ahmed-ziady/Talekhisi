using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talekhisi.Entities;
using Talekhisi.Models;
using Talekhisi.Services;

namespace Talekhisi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.Register(request);
            if (user == null)
            {
                return BadRequest("User already exists");
            }
            return Ok(user);

        }



        [HttpPost("login")]
        public async Task<ActionResult<String>> Login(UserDto request)
        {
            var token = await authService.Login(request);
            if (token is null)
                return BadRequest("Invalid username of password");
            return Ok(token);
        }
        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            // This endpoint is only accessible to authenticated users
            return Ok("You are authenticated!");
        }

        [Authorize (Roles = "admin")]
        [HttpGet ("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            // This endpoint is only accessible to authenticated users
            return Ok("You are an Admin!");
        }

    }
}
