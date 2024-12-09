using FileManager.Api.Contracts.Common;
using FluentValidation;

namespace FileManager.Api.Contracts
{
    public class UploadManyFileRequestValidator : AbstractValidator<UploadManyFileRequest>
    {
        public UploadManyFileRequestValidator()
        {
            RuleForEach(x=>x.Files).SetValidator(new FileSizeValidator());

            RuleForEach(x=>x.Files).SetValidator(new FileContentValidator());
        }
    }
}
