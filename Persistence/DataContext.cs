using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser> // DbContext-> abstraction of database
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // composite key yap
            builder.Entity<ActivityAttendee>(x=>x.HasKey(y=> new{y.AppUserId, y.ActivityId}));
            // many to many releationship ayarla
            // App user için one to many
            builder.Entity<ActivityAttendee>().HasOne(u=>u.AppUser).WithMany(a=>a.Activities).HasForeignKey(y=> y.AppUserId);
            // activities için one to many
            builder.Entity<ActivityAttendee>().HasOne(u=>u.Activity).WithMany(a=>a.Attendees).HasForeignKey(y=> y.ActivityId);
            // ikiside ortadaki table ile one to many yaptı birbirleriyle many to many yapmış oldular

            // commentler için özel relationship. Activity silinince ona bağlı commentlerde gidiyor 
            builder.Entity<Comment>().HasOne(a=>a.Activity).WithMany(c=>c.Comments).OnDelete(DeleteBehavior.Cascade);
        }
    }
}