namespace EmoteTracker.Services
{
    public class SevenEmote : ChannelEmote
    {
        public override ChannelEmoteType EmoteType => ChannelEmoteType.SevenEmote;

        public override string GetEmotePage()
        {
            return "https://cdn.7tv.app/emote/" + Id;
        }

        public override string GetImageUrl()
        {
            return "https://cdn.7tv.app/emote/" + Id + "/1x.avif";
        }
    }
}
