namespace Talekhisi.Models
{
    public class GetLectureNotesDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public int DownloadsNumber { get; set; }
        public int SubjectId { get; set; }
    }

}
