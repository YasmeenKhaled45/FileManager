namespace FileManager.Api.Entities
{
    public sealed class UploadedFile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty ;
        public string ContentType { get; set; } = string.Empty;
        public string FileExtension {  get; set; } = string.Empty;

    }
}
