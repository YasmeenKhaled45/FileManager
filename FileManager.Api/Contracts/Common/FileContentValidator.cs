using FileManager.Api.Settings;
using FluentValidation;

namespace FileManager.Api.Contracts.Common
{
    public class FileContentValidator : AbstractValidator<IFormFile>
    {
        public FileContentValidator() 
        {
            RuleFor(x => x).Must((request, context) =>
            {
                BinaryReader reader = new(request.OpenReadStream());
                var bytes = reader.ReadBytes(2);
                var fileSequence = BitConverter.ToString(bytes);

                foreach (var signature in FileSettings.blockedSignatures)
                {
                    if (signature.Equals(fileSequence, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
                return true;
            }).WithMessage("Not allowed file content").When(x => x is not null);
        }
    }
}
