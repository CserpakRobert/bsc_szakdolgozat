using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Backend.Utils
{
    public class YouTubeLink : ValidationAttribute
    {
        private static readonly Regex YouTubeIdRegex = new(@"^[a-zA-Z0-9_-]{11}$", RegexOptions.Compiled);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value is not string videoId)
            {
                return new ValidationResult("Probléma a youtube linkkel");
            }

            if (!YouTubeIdRegex.IsMatch(videoId))
            {
                return new ValidationResult("Probléma a youtube linkkel");
            }

            return ValidationResult.Success;
        }
    }
}
