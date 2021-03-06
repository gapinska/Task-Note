using Application.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dtos
{
    [BoardNameMustBeDifferentFromDescription]
    public abstract class BoardForManipulationDto
    {
        [Required(ErrorMessage = "You must specify board's name")]
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters")]
        public string Name { get; set; }
        [StringLength(256, ErrorMessage = "Description length can't be more than 256 characters")]
        public string Description { get; set; }
    }
}