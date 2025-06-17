namespace EmoteTracker.Services
{
    public class BttvEmote : ChannelEmote
    {
        public override ChannelEmoteType EmoteType => ChannelEmoteType.BttvEmote;

        public override string GetEmotePage()
        {
            return "https://betterttv.com/emotes/" + Id;
        }

        public override string GetImageUrl()
        {
            // Use the smallest image URL
            return "https://cdn.betterttv.net/emote/" + Id + "/1x";
        }
    }
}
