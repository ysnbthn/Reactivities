namespace Domain
{
    public class Activity
    {
        // serverside yada clientside üretilebilir
        // biz clientsideda da üretmek için kullanıyoruz
        public Guid Id { get; set; } // Id yazınca otomatik key olarak tanıyor
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
    }
}