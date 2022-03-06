namespace Domain
{
    public class UserFollowing
    {
        public string ObserverId { get; set; }
        // Takip eden
        public AppUser Observer { get; set; }
        public string TargetId { get; set; }
        // takip edilen
        public AppUser Target { get; set; }
    }
}