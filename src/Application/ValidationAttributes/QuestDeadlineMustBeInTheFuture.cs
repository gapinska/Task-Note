using Application.Models.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.ValidationAttributes
{
    public class QuestDeadlineMustBeInTheFuture : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quest = (QuestForManipulationDto)validationContext.ObjectInstance;
            if (quest.Deadline != null && quest.Deadline.Value > DateTime.Now)
            {
                return new ValidationResult("Provided deadline must be future date",
                    new[] { nameof(QuestForManipulationDto) });
            }

            return ValidationResult.Success;
        }
    }
}