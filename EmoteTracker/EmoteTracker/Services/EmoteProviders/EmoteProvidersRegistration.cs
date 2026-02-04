using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;

namespace EmoteTracker.Services.EmoteProviders
{
    public static class EmoteProvidersRegistration
    {
        public static IServiceCollection RegisterEmoteProviders(this IServiceCollection services)
        {
            services.AddHttpClient<IBttvService, BttvService>(client => client.BaseAddress = new Uri("https://api.betterttv.net/3/cached/users/twitch/"));
            services.AddHttpClient<IFrankerService, FrankerService>(client => client.BaseAddress = new Uri("https://api.frankerfacez.com/v1/room/id/"));
            services.AddHttpClient<ISevenService, SevenService>(client => client.BaseAddress = new Uri("https://7tv.io/v3/users/twitch/"));

            return services;
        }
    }
}
