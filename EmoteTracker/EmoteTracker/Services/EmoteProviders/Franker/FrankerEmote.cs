namespace EmoteTracker.Services.EmoteProviders.Franker
{
    public class FrankerEmote : ProviderEmote
    {
        public override EmoteProvider Provider => EmoteProvider.FrankerEmote;

        public override string EmotePage => "https://www.frankerfacez.com/emoticon/" + Id + "-" + CanonicalName;

        public override string ImageUrl => "https://cdn.frankerfacez.com/emote/" + Id + "/1";
    }
}
