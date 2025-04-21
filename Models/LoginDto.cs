using System.ComponentModel.DataAnnotations;

namespace Talekhisi.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Username must be less than 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password must be less than 100 characters.")]
        public string Password { get; set; } = string.Empty;

        public string? Role { get; set; } = "user";

    }
}
