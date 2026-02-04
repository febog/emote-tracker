using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;

namespace EmoteTracker.Services.EmoteProviders
{
    public class EmoteProvidersOptions
    {
        public const string EmoteProviders = "EmoteProviders";

        public BttvServiceOptions BttvServiceOptions { get; set; }
        public FrankerServiceOptions FrankerServiceOptions { get; set; }
        public SevenServiceOptions SevenServiceOptions { get; set; }
    }
}
