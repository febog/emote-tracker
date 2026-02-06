using EmoteTracker.Data;
using EmoteTracker.Services;
using EmoteTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmoteTracker.Pages
{
    public class TwitchChannelModel(IEmoteTrackerService tracker) : PageModel
    {
        private readonly IEmoteTrackerService _tracker = tracker;

        public TrackedChannel TrackedChannel { get; set; }

        public async Task<IActionResult> OnGetAsync(string channel, bool refresh = false)
        {
            if (string.IsNullOrEmpty(channel))
            {
                return NotFound();
            }

            TrackedChannel = await _tracker.GetChannelData(channel, refresh);

            if (TrackedChannel == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
