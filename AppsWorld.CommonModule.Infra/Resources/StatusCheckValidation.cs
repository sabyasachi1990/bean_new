using System.ComponentModel.DataAnnotations;

namespace AppsWorld.CommonModule.Infra
{
    public class StatusValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null) // lets check if we have some value
            {
                //if (value is int) // check if it is a valid integer
                //{
                int suppliedValue = (int)value;
                if (suppliedValue == 0)
                {
                    // let the user know about the validation error
                    return new ValidationResult("Status is required.");
                }
                //}
            }

            return ValidationResult.Success;
        }
    }
}
