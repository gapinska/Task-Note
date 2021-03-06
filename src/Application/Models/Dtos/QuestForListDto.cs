using System;

namespace Application.Models.Dtos
{
    public class QuestForListDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
    }
}