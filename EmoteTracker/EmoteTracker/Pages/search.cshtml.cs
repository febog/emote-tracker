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
            return RedirectToPage("./t", new { channel });
        }
    }
}