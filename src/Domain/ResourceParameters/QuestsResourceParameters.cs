namespace Domain.ResourceParameters
{
    public class QuestsResourceParameters : ResourceParameters
    {
        public int? LabelId { get; set; }
        public int? BoardId { get; set; }
        public bool? IsDone { get; set; }
        public int? DaysToDeadline { get; set; }
    }
}