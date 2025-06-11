using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmoteTracker.Pages
{
    public class SearchModel(ITwitchService twitchService) : PageModel
    {
        private readonly ITwitchService _twitchService = twitchService;

        public async Task<IActionResult> OnGetAsync(string q)
        {
            var userId = await _twitchService.GetTwitchId(q);
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            return RedirectToPage("./Index", new { id = userId });
        }
    }
}
