
using FileManager.Api.Contracts;
using FileManager.Api.Data;
using FileManager.Api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using System.Net.Http;

namespace FileManager.Api.Services
{
    public class FileService(IWebHostEnvironment hostEnvironment , ApplicationDbContext context , IHttpContextAccessor httpContext) : IFileService
    {
        private readonly string _filepath = $"{hostEnvironment.WebRootPath}/Uploads";
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContext = httpContext;

        public async Task<(byte[] fileContent, string FileName, string ContentType)> DownloadAsync(Guid Id, CancellationToken cancellationToken)
        {
            var file = await _context.Files.FindAsync(Id, cancellationToken);
            if(file is null)
                return ([],string.Empty,string.Empty);

            var path = Path.Combine(_filepath,file.StoredFileName);
            MemoryStream stream = new();
            using FileStream fileStream= new(path, FileMode.Open);
            fileStream.CopyTo(stream);
            stream.Position = 0;
            return (stream.ToArray(),file.ContentType,file.FileName);
        }

        public async Task<(FileStream? stream, string ContentType, string FileName)> StreamAsync(Guid Id, CancellationToken cancellationToken)
        {
            var file = await _context.Files.FindAsync(Id, cancellationToken);
            if (file is null)
                return (null, string.Empty, string.Empty);

            var path = Path.Combine(_filepath, file.StoredFileName);
            var filestream = File.OpenRead(path);
            return(filestream,file.ContentType,file.FileName);
        }

        public async Task<string> UploadFile(IFormFile file,CancellationToken cancellationToken)
        {
            if (file is null)
                throw new ArgumentException("Invalid file.");
            var httpContext = _httpContext.HttpContext;
            var fileUrl = await SaveFileAndGenerateUrlAsync(file,httpContext ,cancellationToken);
            return fileUrl;
        }
        //public async Task<IEnumerable<Guid>> UploadFiles(IFormFileCollection files, CancellationToken cancellationToken)
        //{
        //    List<UploadedFile> uploadedFiles = [];
        //    foreach (var file in files)
        //    {
        //        var uploadedfile = await SaveFile(file, cancellationToken);
        //        uploadedFiles.Add(uploadedfile);

        //    }
        //    await _context.AddRangeAsync(uploadedFiles, cancellationToken);
        //    await _context.SaveChangesAsync(cancellationToken);
        //    return uploadedFiles.Select(x => x.Id).ToList();
        //}
        //private async Task<UploadedFile> SaveFile(IFormFile file , CancellationToken cancellationToken)
        //{
        //    BinaryReader reader = new(file.OpenReadStream());
        //    var bytes = reader.ReadBytes(2);
        //    var filesequence = BitConverter.ToString(bytes);
        //    var fakeFileName = Path.GetRandomFileName();
        //    var uploadedfile = new UploadedFile
        //    {
        //        FileName = file.FileName,
        //        StoredFileName = fakeFileName,
        //        ContentType = file.ContentType,
        //        FileExtension = Path.GetExtension(file.FileName)
        //    };
        //    var path = Path.Combine(_filepath, fakeFileName);
        //    using var stream = File.Create(path);
        //    await file.CopyToAsync(stream, cancellationToken);
        //    return uploadedfile;
        //}
        private async Task<string> SaveFileAndGenerateUrlAsync(IFormFile file, HttpContext httpContext,CancellationToken cancellationToken)
        {
            var uniqueFileName = Path.GetRandomFileName();
            var fileExtension = Path.GetExtension(file.FileName);
            var storedFileName = uniqueFileName + fileExtension;

            var filePath = Path.Combine(_filepath, storedFileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream, cancellationToken);

            var fileUrl = GenerateFileUrl(httpContext,storedFileName);
            return fileUrl;
        }
        private string GenerateFileUrl(HttpContext httpContext,string storedFileName)
        {
            var request = httpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host.Value}/Uploads/{storedFileName}";

            return baseUrl;
        }

        public async Task<IEnumerable<string>> UploadFiles(IFormFileCollection formFiles, CancellationToken cancellationToken)
        {
            var fileUrls = new List<string>();
            var httpContext = _httpContext.HttpContext;
            foreach (var file in formFiles)
            {
                var fileUrl = await SaveFileAndGenerateUrlAsync(file,httpContext ,cancellationToken);
                fileUrls.Add(fileUrl);
            }
            return fileUrls;
        }
    }
}
