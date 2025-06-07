using EmoteTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmoteTracker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFrankerService _emoteService;
        private readonly ITwitchService _twitchService;

        public IndexModel(ILogger<IndexModel> logger,
            IFrankerService emoteService,
            ITwitchService twitchService)
        {
            _logger = logger;
            _emoteService = emoteService;
            _twitchService = twitchService;
        }

        [BindProperty(SupportsGet = true)]
        public string Username { get; set; }

        public IList<ChannelEmote> Emotes { get; set; }

        public async Task OnGetAsync()
        {
            var userId = await _twitchService.GetTwitchId(Username);
            Emotes = await _emoteService.GetChannelEmotes(userId);
            if (Emotes == null) { Emotes = new List<ChannelEmote>(); }
        }
    }
}
