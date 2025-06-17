namespace EmoteTracker.Services
{
    public class FrankerEmote : ChannelEmote
    {
        public override string GetEmotePage()
        {
            return "https://www.frankerfacez.com/emoticon/" + Id + "-" + CanonicalName;
        }

        public override string GetImageUrl()
        {
            return "https://cdn.frankerfacez.com/emote/" + Id + "/1";
        }
    }
}
