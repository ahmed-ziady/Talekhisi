using System.ComponentModel.DataAnnotations;

namespace Talekhisi.Entities
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<LectureNote> LectureNotes { get; set; } = new List<LectureNote>();
    }
}
