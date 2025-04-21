using Talekhisi.Models;

namespace Talekhisi.Services
{
    public interface IFileManagementService
    {
        Task<string> UploadFileAsync(LectureNoteUploadDto uploadDto);
        Task<(byte[] FileContent, string ContentType, string FileName)> DownloadFileAsync(int lectureNoteId);
        Task<string> DeleteFileAsync(int id);
        Task<IEnumerable<LectureNoteReadDto>> GetLectureNotesAsync(int page = 1, int pageSize = 20);

        Task<IEnumerable<LectureNoteReadDto>> SearchLectureNotesAsync(string query);

    }
}
