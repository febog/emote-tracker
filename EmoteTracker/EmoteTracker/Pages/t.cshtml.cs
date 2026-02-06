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

        public async Task<IActionResult> OnGetAsync(string channel)
        {
            if (string.IsNullOrEmpty(channel))
            {
                return NotFound();
            }

            TrackedChannel = await _tracker.GetChannelData(channel);

            if (TrackedChannel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string channel, bool refresh)
        {
            if (string.IsNullOrEmpty(channel))
            {
                return NotFound();
            }

            TrackedChannel = await _tracker.GetChannelData(channel, true);

            if (TrackedChannel == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
