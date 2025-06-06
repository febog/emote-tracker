using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmoteTracker.Models;

namespace EmoteTracker.Data
{
    public class EmoteTrackerContext : DbContext
    {
        public EmoteTrackerContext (DbContextOptions<EmoteTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<EmoteTracker.Models.Emote> Emote { get; set; } = default!;
    }
}
