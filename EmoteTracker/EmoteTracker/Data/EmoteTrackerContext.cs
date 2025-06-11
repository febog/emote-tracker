using EmoteTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmoteTracker.Data
{
    public class EmoteTrackerContext : DbContext
    {
        public EmoteTrackerContext (DbContextOptions<EmoteTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<TwitchChannel> TwitchChannels { get; set; }

        public DbSet<TwitchChannelEmote> TwitchChannelEmotes { get; set; }
    }
}
