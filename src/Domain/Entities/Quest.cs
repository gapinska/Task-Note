using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Quest : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? Deadline { get; set; }
        public int? BoardId { get; set; }
        public Board Board { get; set; }
        public int? LabelId { get; set; }
        public Label Label { get; set; }
    }
}