using FileManager.Api.Settings;
using FluentValidation;

namespace FileManager.Api.Contracts.Common
{
    public class FileSizeValidator : AbstractValidator<IFormFile>
    {
        public FileSizeValidator() 
        {
            RuleFor(x => x).Must((request, context) => request.Length <= FileSettings.MaxSizeInBytes)
                .WithMessage($" Max file size is {FileSettings.MaxSizeInMB} MB.")
                .When(x => x is not null);
        }
    }
}
