using System.Collections.Generic;

namespace Domain.Entities
{
    public class Board : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Label> Labels { get; set; }
        public ICollection<BoardViewer> BoardViewers { get; set; }
        public ICollection<BoardEditor> BoardEditors { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}