using System.ComponentModel.DataAnnotations;

namespace Talekhisi.Entities
{
    public class LectureNote
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(100)]
        public string FacultyName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string UniversityName { get; set; } = string.Empty;

        public int DownloadsNumber { get; set; } = 0;
        public int UploadsNumber { get; set; } = 0;

        public byte[]? FileContent { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        [Required]
        public int SubjectId { get; set; }

        public Subject? Subject { get; set; }
    }
}
