using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmoteTracker.Pages
{
    public class SearchModel() : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string channel)
        {
            return RedirectToPage("./t", new { channel });
        }
    }
}