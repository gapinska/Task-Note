using Application.Models.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes
{
    public class QuestNameMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quest = (QuestForManipulationDto)validationContext.ObjectInstance;

            if (quest.Name == quest.Description)
            {
                return new ValidationResult("Provided description should be different from quest's name",
                    new[] { nameof(QuestForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}