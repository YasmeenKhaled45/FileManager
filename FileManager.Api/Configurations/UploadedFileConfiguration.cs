using FileManager.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileManager.Api.Configurations
{
    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.Property(x => x.FileName).HasMaxLength(250);
            builder.Property(x => x.StoredFileName).HasMaxLength(250);
            builder.Property(x => x.ContentType).HasMaxLength(50);
            builder.Property(x => x.FileExtension).HasMaxLength(10);
        }
    }
}
