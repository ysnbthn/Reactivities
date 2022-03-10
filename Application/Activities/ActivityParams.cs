using Application.Core;

namespace Application.Activities
{
    // paging üstüne yeni parametreler ekliyoruz
    public class ActivityParams : PagingParams
    {
        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}