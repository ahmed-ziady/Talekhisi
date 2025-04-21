using System.ComponentModel.DataAnnotations;

namespace Talekhisi.Models
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Refresh token is required.")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
