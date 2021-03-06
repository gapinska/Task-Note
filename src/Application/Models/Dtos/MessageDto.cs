using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Dtos
{
    public class MessageDto
    {
        [Required]
        public string Message { get; set; }
        [Required]
        public int ReceiverId { get; set; }
    }
}
