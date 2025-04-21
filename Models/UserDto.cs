using System.ComponentModel.DataAnnotations;

namespace Talekhisi.Models
{
    public class UserDto
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


        [Required(ErrorMessage = "First name is required.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "First name must be less than 50 characters.")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required.")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "Last name must be less than 50 characters.")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [MaxLength(100, ErrorMessage = "Email must be less than 100 characters.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "University is required.")]
        [MinLength(2, ErrorMessage = "University must be at least 2 characters.")]
        [MaxLength(100, ErrorMessage = "University must be less than 100 characters.")]
        public string University { get; set; } = string.Empty;
        [Required(ErrorMessage = "Faculty is required.")]
        [MinLength(2, ErrorMessage = "Faculty must be at least 2 characters.")]
        [MaxLength(100, ErrorMessage = "Faculty must be less than 100 characters.")]
        public string Faculty { get; set; } = string.Empty;
        [Required(ErrorMessage = "Department is required.")]
        [MinLength(2, ErrorMessage = "Department must be at least 2 characters.")]
        [MaxLength(100, ErrorMessage = "Department must be less than 100 characters.")]
        public string Department { get; set; } = string.Empty;
        [Required(ErrorMessage = "Grade is required.")]
        [MinLength(1, ErrorMessage = "Grade must be at least 1 character.")]
        [MaxLength(10, ErrorMessage = "Grade must be less than 10 characters.")]
        public string Grade { get; set; } = string.Empty;

    }    
}
