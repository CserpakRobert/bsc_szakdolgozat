using System.ComponentModel.DataAnnotations;

namespace Backend.Utils
{
    public class Title : ValidationAttribute
    {


            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value == null || value is not string description || description.Length > 200 || description.Length < 10)
                {
                    return new ValidationResult("A címnek 10 és 200 karakter között kell lennie");
                }


                return ValidationResult.Success;
            }
        
    }
}
