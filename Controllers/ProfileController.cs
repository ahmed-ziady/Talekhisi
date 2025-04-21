using Microsoft.AspNetCore.Mvc;
using Talekhisi.Models;
using Talekhisi.Services;

namespace Talekhisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserProfileAsync(Guid id)
        {
            try
            {
                var userProfile = await _profileService.GetUserProfileAsync(id);
                return Ok(userProfile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred.", Details = ex.Message });
            }
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUserProfileInfoAsync(Guid id, [FromBody] UpdateUserInfoDto userDto)
        {
            try
            {
                var updatedUserProfile = await _profileService.UpdateUserProfileInfoAsync(id, userDto);
                return Ok(updatedUserProfile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = "User not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPut("{id:guid}/password")]
        public async Task<IActionResult> UpdateUserProfilePasswordAsync(Guid id, [FromBody] UpdatePasswordDto passwordDto)
        {
            if (passwordDto == null || string.IsNullOrWhiteSpace(passwordDto.NewPassword) || string.IsNullOrWhiteSpace(passwordDto.OldPassword))
                return BadRequest(new { Error = "New password and old password cannot be empty." });

            if (passwordDto.NewPassword.Length < 6)
                return BadRequest(new { Error = "New password must be at least 6 characters long." });

            try
            {
                var result = await _profileService.UpdateUserProfilePasswordAsync(id, passwordDto.NewPassword, passwordDto.OldPassword);
                if (result)
                    return Ok(new { Message = "Password updated successfully." });

                return NotFound(new { Error = "User not found." }); // This line is a fallback — unlikely to be hit
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Error = "Old password is incorrect." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = "User not found." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

    }
}