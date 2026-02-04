namespace EmoteTracker.Services.EmoteProviders.Bttv
{
    public class BttvEmote : ProviderEmote
    {
        public override EmoteProvider Provider => EmoteProvider.BttvEmote;

        public override string EmotePage => "https://betterttv.com/emotes/" + Id;

        public override string ImageUrl => "https://cdn.betterttv.net/emote/" + Id + "/1x";
    }
}
