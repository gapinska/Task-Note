using System.ComponentModel.DataAnnotations;

namespace Application.Models.Dtos
{
    public class UserForCreationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [StringLength(50, ErrorMessage = "Name length can't be more than 50 characters")]
        public string FullName { get; set; }
        [StringLength(256, ErrorMessage = "Description length can't be more than 256 characters")]
        public string Bio { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}