namespace EmoteTracker.Services.EmoteProviders.Franker
{
    public class FrankerEmote : ChannelEmote
    {
        public override ChannelEmoteType EmoteType => ChannelEmoteType.FrankerEmote;

        public override string EmotePage => "https://www.frankerfacez.com/emoticon/" + Id + "-" + CanonicalName;

        public override string ImageUrl => "https://cdn.frankerfacez.com/emote/" + Id + "/1";
    }
}
