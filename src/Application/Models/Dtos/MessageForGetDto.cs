using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Dtos
{
    public class MessageForGetDto
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Sent { get; set; }
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int ReceiverId { get; set; }
    }
}
