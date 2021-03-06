using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class MessagePost
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Sent { get; set; }
        [Required]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        public User Sender { get; set; }
        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }
}
