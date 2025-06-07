using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmoteTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ISevenService _emoteService;

        public IndexModel(ILogger<IndexModel> logger, ISevenService emoteService)
        {
            _logger = logger;
            _emoteService = emoteService;
        }

        public IList<ChannelEmote> Emotes { get; set; }

        public async Task OnGetAsync()
        {
            Emotes = await _emoteService.GetChannelEmotes("123");
        }
    }
}
