using System.Collections.Generic;

namespace Domain.Entities
{
    public class BoardWithContributors
    {
        public int BoardId { get; set; }
        public IEnumerable<int> ContributorsIds { get; set; }
    }
}
