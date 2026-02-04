namespace EmoteTracker.Services.EmoteProviders.Bttv
{
    public class BttvEmote : ProviderEmote
    {
        public override ChannelEmoteType EmoteType => ChannelEmoteType.BttvEmote;

        public override string EmotePage => "https://betterttv.com/emotes/" + Id;

        public override string ImageUrl => "https://cdn.betterttv.net/emote/" + Id + "/1x";
    }
}
