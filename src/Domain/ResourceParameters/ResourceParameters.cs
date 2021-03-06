namespace Domain.ResourceParameters
{
    public abstract class ResourceParameters
    {
        public string OrderBy { get; set; }
        public string SearchQuery { get; set; }
        const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}