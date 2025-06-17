namespace EmoteTracker.Services
{
    public class SevenEmote : ChannelEmote
    {
        public override ChannelEmoteType EmoteType => ChannelEmoteType.SevenEmote;

        public override string EmotePage => "https://cdn.7tv.app/emote/" + Id;

        public override string ImageUrl => "https://cdn.7tv.app/emote/" + Id + "/1x.avif";
    }
}
