namespace Domain.Entities
{
    public class BoardEditor
    {
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public int EditorId { get; set; }
        public User Editor { get; set; }
    }
}
