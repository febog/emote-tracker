namespace EmoteTracker.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterEmoteServices(this IServiceCollection services)
        {
            services.AddHttpClient<IFrankerService, FrankerService>(client => client.BaseAddress = new Uri("https://api.frankerfacez.com/v1/room/id/"));
            services.AddHttpClient<IBttvService, BttvService>(client => client.BaseAddress = new Uri("https://api.betterttv.net/3/cached/users/twitch/"));
            services.AddHttpClient<ISevenService, SevenService>(client => client.BaseAddress = new Uri("https://7tv.io/v3/users/twitch/"));

            return services;
        }
    }
}
