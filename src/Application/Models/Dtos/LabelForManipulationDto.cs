using Application.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dtos
{
    [LabelNameMustBeDifferentFromDescription]
    public abstract class LabelForManipulationDto
    {
        [Required(ErrorMessage = "You must specify label's name")]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters")]
        public string Name { get; set; }
        [StringLength(256, ErrorMessage = "Description length can't be more than 256 characters")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessage = "Color length can't be more than 50 characters")]
        public string Color { get; set; }
    }
}