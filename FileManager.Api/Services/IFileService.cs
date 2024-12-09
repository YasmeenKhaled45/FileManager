using FileManager.Api.Contracts;

namespace FileManager.Api.Services
{
    public interface IFileService
    {
        Task<string> UploadFile(IFormFile file , CancellationToken cancellationToken);
        Task<IEnumerable<string>> UploadFiles(IFormFileCollection formFiles , CancellationToken cancellationToken);
        //Task<IEnumerable<Guid>> UploadFiles(IFormFileCollection files, CancellationToken cancellationToken);
        Task<(byte[] fileContent,string FileName , string ContentType)> DownloadAsync(Guid Id , CancellationToken cancellationToken);
        Task<(FileStream? stream , string ContentType , string FileName)> StreamAsync(Guid Id , CancellationToken cancellationToken);

    }
}
