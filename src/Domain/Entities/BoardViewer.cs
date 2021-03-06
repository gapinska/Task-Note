namespace Domain.Entities
{
    public class BoardViewer
    {
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public int ViewerId { get; set; }
        public User Viewer { get; set; }
    }
}
