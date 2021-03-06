using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Label : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public virtual ICollection<Quest> Quests { get; set; }
        [Required]
        public int BoardId { get; set; }
        public virtual Board Board { get; set; }
    }
}