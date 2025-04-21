namespace Talekhisi.Models
{
    public class LectureNoteReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public int DownloadsNumber { get; set; }
        public int SubjectId { get; set; }

        public string DownloadUrl { get; set; } = string.Empty; 

    }
}
