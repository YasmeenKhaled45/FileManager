namespace FileManager.Api.Settings
{
    public static class FileSettings
    {
        public const int MaxSizeInMB = 1;
        public const int MaxSizeInBytes = MaxSizeInMB * 1024 * 1024;
        public static readonly string[] blockedSignatures = ["4D-5A","2F-2A","D0-CF"];
    }
}
