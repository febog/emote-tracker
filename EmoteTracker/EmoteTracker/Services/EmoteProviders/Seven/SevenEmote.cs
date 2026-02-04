namespace EmoteTracker.Services.EmoteProviders.Seven
{
    public class SevenEmote : ProviderEmote
    {
        public override EmoteProvider Provider => EmoteProvider.SevenEmote;

        public override string EmotePage => "https://7tv.app/emotes/" + Id;

        public override string ImageUrl => "https://cdn.7tv.app/emote/" + Id + "/1x.avif";
    }
}
