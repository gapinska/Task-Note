using Application.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes
{
    public class BoardNameMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var board = (BoardForManipulationDto)validationContext.ObjectInstance;

            if (board.Name == board.Description)
            {
                return new ValidationResult("Provided description should be different from board's name",
                    new[] { nameof(BoardForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}
