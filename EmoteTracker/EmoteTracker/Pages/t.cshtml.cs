using EmoteTracker.Data;
using EmoteTracker.Models;
using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EmoteTracker.Pages
{
    public class TwitchChannelModel(IEmoteTrackerService tracker,
        ITwitchService twitchService,
        EmoteTrackerContext context) : PageModel
    {
        private readonly IEmoteTrackerService _tracker = tracker;
        private readonly ITwitchService _twitchService = twitchService;
        private readonly EmoteTrackerContext _context = context;

        public TwitchChannel TwitchChannel { get; set; }

        public async Task<IActionResult> OnGetAsync(string channel)
        {
            if (string.IsNullOrEmpty(channel))
            {
                return RedirectToPage("./Index");
            }

            var channelId = await _twitchService.GetTwitchId(channel);
            if (string.IsNullOrEmpty(channelId))
            {
                return NotFound();
            }

            // Check if previously tracked
            if (await _context.TwitchChannels.FindAsync(channelId) == null)
            {
                await _tracker.RefreshChannelEmotes(channelId);
            }

            var channelData = await _context.TwitchChannels
                    .Include(c => c.TwitchChannelEmotes)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == channelId);

            if (channelData == null)
            {
                return NotFound();
            }

            TwitchChannel = channelData;

            return Page();
        }
    }
}
