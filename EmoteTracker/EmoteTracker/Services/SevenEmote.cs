namespace EmoteTracker.Services
{
    public class SevenEmote : ChannelEmote
    {
        public override ChannelEmoteType EmoteType => ChannelEmoteType.SevenEmote;

        public override string EmotePage => "https://7tv.app/emotes/" + Id;

        public override string ImageUrl => "https://cdn.7tv.app/emote/" + Id + "/1x.avif";
    }
}
