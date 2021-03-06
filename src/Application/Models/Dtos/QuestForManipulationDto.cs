using Application.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dtos
{
    [QuestNameMustBeDifferentFromDescription]
    [QuestDeadlineMustBeInTheFuture]
    public class QuestForManipulationDto
    {
        [Required(ErrorMessage = "You must specify task's name")]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters")]
        public string Name { get; set; }
        [StringLength(256, ErrorMessage = "Description length can't be more than 256 characters")]
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsDone { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int? LabelId { get; set; }
        public int BoardId { get; set; }
    }
}