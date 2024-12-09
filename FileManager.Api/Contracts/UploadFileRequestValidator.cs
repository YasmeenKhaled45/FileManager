using FileManager.Api.Contracts.Common;
using FileManager.Api.Settings;
using FluentValidation;

namespace FileManager.Api.Contracts
{
    public class UploadFileRequestValidator : AbstractValidator<UploadedFileRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(x => x.File).SetValidator(new FileSizeValidator());

            RuleFor(x => x.File).SetValidator(new FileContentValidator());

        }
    }
}
