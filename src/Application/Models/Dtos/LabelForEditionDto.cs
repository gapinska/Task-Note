using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dtos
{
    public class LabelForEditionDto
    {
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters")]
        public string Name { get; set; }
        [StringLength(256, ErrorMessage = "Description length can't be more than 256 characters")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessage = "Color length can't be more than 50 characters")]
        public string Color { get; set; }
        public int BoardId { get; set; }
    }
}