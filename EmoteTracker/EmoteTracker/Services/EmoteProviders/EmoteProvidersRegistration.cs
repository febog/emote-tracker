using EmoteTracker.Services.EmoteProviders.Bttv;
using EmoteTracker.Services.EmoteProviders.Franker;
using EmoteTracker.Services.EmoteProviders.Seven;

namespace EmoteTracker.Services.EmoteProviders
{
    public static class EmoteProvidersRegistration
    {
        public static IServiceCollection RegisterEmoteProviders(this IServiceCollection services, IConfigurationSection configuration)
        {
            var options = configuration.Get<EmoteProvidersOptions>();
            services.AddHttpClient<IBttvService, BttvService>(client => client.BaseAddress = new Uri(options.BttvServiceOptions.BaseAddress));
            services.AddHttpClient<IFrankerService, FrankerService>(client => client.BaseAddress = new Uri(options.FrankerServiceOptions.BaseAddress));
            services.AddHttpClient<ISevenService, SevenService>(client => client.BaseAddress = new Uri(options.SevenServiceOptions.BaseAddress));

            return services;
        }
    }
}
