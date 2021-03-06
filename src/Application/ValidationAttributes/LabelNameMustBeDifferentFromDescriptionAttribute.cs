using Application.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes
{
    public class LabelNameMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var label = (LabelForManipulationDto)validationContext.ObjectInstance;

            if (label.Name == label.Description)
            {
                return new ValidationResult("Provided description should be different from label's name",
                    new[] { nameof(LabelForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}