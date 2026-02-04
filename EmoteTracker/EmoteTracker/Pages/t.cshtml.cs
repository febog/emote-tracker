using EmoteTracker.Data;
using EmoteTracker.Models;
using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public List<ChannelEmote> ChannelEmotes { get; set; }

        public async Task<IActionResult> OnGetAsync(string channel)
        {
            var channelId = await _twitchService.GetTwitchId(channel);
            if (string.IsNullOrEmpty(channelId))
            {
                return NotFound();
            }

            // Check if previously tracked
            var channelData = await _context.TwitchChannels.FindAsync(channelId);

            if (channelData == null)
            {
                await _tracker.RefreshChannelEmotes(channelId);
                channelData = await _context.TwitchChannels.FindAsync(channelId);
            }

            TwitchChannel = channelData;

            ChannelEmotes = await _tracker.GetChannelEmotes(channelId);

            return Page();
        }
    }
}
