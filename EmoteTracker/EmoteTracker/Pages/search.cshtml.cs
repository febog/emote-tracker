using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmoteTracker.Pages
{
    public class SearchModel(ITwitchService twitchService) : PageModel
    {
        private readonly ITwitchService _twitchService = twitchService;

        public async Task<IActionResult> OnGetAsync(string channel)
        {
            var userId = await _twitchService.GetTwitchId(channel);
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            return RedirectToPage("./t", new { channel });
        }
    }
}