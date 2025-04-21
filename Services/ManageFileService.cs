using Microsoft.EntityFrameworkCore;
using Talekhisi.Data;
using Talekhisi.Entities;
using Talekhisi.Models;

namespace Talekhisi.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        private static readonly HashSet<string> AllowedExtensions = [".pdf", ".docx", ".pptx"];

        public FileManagementService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> UploadFileAsync(LectureNoteUploadDto uploadDto)
        {
            if (uploadDto.File == null || uploadDto.File.Length == 0)
                throw new InvalidOperationException("Uploaded file is empty.");

            var extension = Path.GetExtension(uploadDto.File.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid file type. Allowed types: .pdf, .docx, .pptx");

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Name.ToLower() == uploadDto.SubjectName.Trim().ToLower());

            if (subject == null)
                throw new KeyNotFoundException($"Subject '{uploadDto.SubjectName}' not found.");

            using var memoryStream = new MemoryStream();
            await uploadDto.File.CopyToAsync(memoryStream);

            var lectureNote = new LectureNote
            {
                Title = uploadDto.Title.Trim(),
                Description = uploadDto.Description?.Trim(),
                FacultyName = uploadDto.FacultyName.Trim(),
                UniversityName = uploadDto.UniversityName.Trim(),
                FileName = uploadDto.File.FileName,
                ContentType = uploadDto.File.ContentType,
                FileContent = memoryStream.ToArray(),
                SubjectId = subject.Id,
                UploadsNumber = 1,
                DownloadsNumber = 0
            };

            _context.LectureNotes.Add(lectureNote);
            await _context.SaveChangesAsync();

            return lectureNote.Id.ToString();
        }

        public async Task<(byte[] FileContent, string ContentType, string FileName)> DownloadFileAsync(int lectureNoteId)
        {
            var note = await _context.LectureNotes.FirstOrDefaultAsync(n => n.Id == lectureNoteId);

            if (note == null || note.FileContent == null)
                throw new FileNotFoundException("Lecture note not found.");

            note.DownloadsNumber++;
            await _context.SaveChangesAsync();

            return (note.FileContent, note.ContentType ?? "application/octet-stream", note.FileName ?? "download");
        }

        public async Task<string> DeleteFileAsync(int id)
        {
            var note = await _context.LectureNotes.FindAsync(id);
            if (note == null)
                throw new KeyNotFoundException("Lecture note not found.");

            _context.LectureNotes.Remove(note);
            await _context.SaveChangesAsync();

            return $"Lecture note '{note.Title}' was deleted successfully.";
        }

        public async Task<IEnumerable<LectureNoteReadDto>> GetLectureNotesAsync(int page = 1, int pageSize = 20)
        {
            if (page <= 0) page = 1;

            return await _context.LectureNotes
                .AsNoTracking()
                .OrderByDescending(n => n.DownloadsNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new LectureNoteReadDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    FileName = n.FileName ?? "",
                    ContentType = n.ContentType ?? "",
                    DownloadsNumber = n.DownloadsNumber,
                    SubjectId = n.SubjectId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LectureNoteReadDto>> SearchLectureNotesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Search query cannot be empty.", nameof(query));

            query = query.Trim().ToLower();

            return await _context.LectureNotes
                .AsNoTracking()
                .Include(n => n.Subject)
                .Where(n =>
                    EF.Functions.Like(n.Title.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(n.Description!.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(n.FacultyName.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(n.UniversityName.ToLower(), $"%{query}%") ||
                    EF.Functions.Like(n.Subject!.Name.ToLower(), $"%{query}%"))
                .Select(n => new LectureNoteReadDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    FileName = n.FileName ?? "",
                    ContentType = n.ContentType ?? "",
                    DownloadsNumber = n.DownloadsNumber,
                    SubjectId = n.SubjectId
                })
                .ToListAsync();
        }
    }
}
