using System.ComponentModel.DataAnnotations;

namespace Talekhisi.Models
{
    public class LectureNoteUploadDto
    {

        [Required(ErrorMessage = "Subject name is required.")]
        [StringLength(100, ErrorMessage = "Subject name cannot exceed 100 characters.")]
        public string SubjectName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Faculty name is required.")]
        [StringLength(100, ErrorMessage = "Faculty name cannot exceed 100 characters.")]
        public string FacultyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "A file is required.")]
        public IFormFile File { get; set; } = null!;

        [Required(ErrorMessage = "University name is required.")]
        [StringLength(100, ErrorMessage = "Faculty name cannot exceed 100 characters.")]

        public string UniversityName { get; set; } = string.Empty;
    }
}
