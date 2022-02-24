using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        // identity üstüne gelecekleri ekle
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}