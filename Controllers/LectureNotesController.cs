using Microsoft.AspNetCore.Mvc;
using Talekhisi.Models;
using Talekhisi.Services;

namespace Talekhisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LectureNotesController : ControllerBase
    {
        private readonly IFileManagementService _fileService;
        private readonly ILogger<LectureNotesController> _logger;

        public LectureNotesController(
            IFileManagementService fileService,
            ILogger<LectureNotesController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] LectureNoteUploadDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var lectureNoteId = await _fileService.UploadFileAsync(dto);
                return Ok(new
                {
                    Message = "Lecture note uploaded successfully.",
                    LectureNoteId = lectureNoteId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during lecture note upload.");
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("download/{id:int}")]
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var (fileContent, contentType, fileName) = await _fileService.DownloadFileAsync(id);
                return File(fileContent, contentType, fileName);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning("Lecture note not found for download. ID: {Id}", id);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during download. ID: {Id}", id);
                return StatusCode(500, new { Error = "Unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var resultMessage = await _fileService.DeleteFileAsync(id);
                return Ok(new { Message = resultMessage });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Lecture note not found for deletion. ID: {Id}", id);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during deletion. ID: {Id}", id);
                return StatusCode(500, new { Error = "Unexpected error occurred.", Details = ex.Message });
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var notes = await _fileService.GetLectureNotesAsync(page, pageSize);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching paginated lecture notes");
                return StatusCode(500, new { Error = "Failed to fetch lecture notes." });
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            try
            {
                var notes = await _fileService.SearchLectureNotesAsync(query);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching lecture notes");
                return StatusCode(500, new { Error = "Failed to search lecture notes." });
            }
        }

    }
}
