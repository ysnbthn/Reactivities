namespace Domain
{
    // many to many için yapılmış ara table
    public class ActivityAttendee
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid ActivityId { get; set; }
        public Activity Activity { get; set; }
        // activiteye katılan adam host mu?
        public bool IsHost { get; set; }
    }
}