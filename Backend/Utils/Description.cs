using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Backend.Utils
{
    public class ValidDescription : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value is not string description || description.Length > 3000 || description.Length < 10 )
            {
                return new ValidationResult("A leírásnak 10 és 3000 karakter között kell lennie");
            }


            return ValidationResult.Success;
        }
    }
}
