using FileManager.Api.Contracts;
using FileManager.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IFileService fileService) : ControllerBase
    {
        private readonly IFileService _fileService = fileService;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm]UploadedFileRequest request , CancellationToken cancellationToken)
        {
            var fileurl = await _fileService.UploadFile(request.File ,cancellationToken);
            return Ok(fileurl);
        }
        //[HttpPost("uploadfiles")]
        //public async Task<IActionResult> UploadFiles([FromForm] UploadManyFileRequest uploadMany , CancellationToken cancellationToken)
        //{
        //    var files = await _fileService.UploadFiles(uploadMany.Files, cancellationToken);
        //    return Ok(files);
        //}
        [HttpPost("uploadfiles")]
        public async Task<IActionResult> UploadFiles([FromForm] UploadManyFileRequest request , CancellationToken cancellationToken)
        {
            var fileUrls = await _fileService.UploadFiles(request.Files, cancellationToken);
            return Ok(fileUrls);
        }
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var (fileContent, fileName, contentType) = await _fileService.DownloadAsync(id, cancellationToken);
             return fileContent is [] ? NotFound() : File(fileContent, fileName, contentType);  
        }

        [HttpGet("stream/{id}")]
        public async Task<IActionResult> Stream([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var (stream, fileName, contentType) = await _fileService.StreamAsync(id, cancellationToken);
            return stream is null ? NotFound() : File(stream, fileName, contentType,enableRangeProcessing:true);
        }
    }
}
