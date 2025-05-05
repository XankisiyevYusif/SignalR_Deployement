using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork1.Entities;

namespace SocialNetwork1.Data
{
    public class SocialNetworkDbContext:IdentityDbContext<CustomIdentityUser,CustomIdentityRole,string>
    {
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options)
            :base(options) { }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CustomIdentityUser>().Ignore(u => u.HasRequestPending);
            builder.Entity<CustomIdentityUser>().Ignore(u => u.IsFriend);
        }
    }
}
