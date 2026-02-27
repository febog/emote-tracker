using EmoteTracker.Services.EmoteProviders;

namespace EmoteTracker.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterEmoteTrackerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEmoteProviders(configuration.GetSection(EmoteProvidersOptions.EmoteProviders));

            services.AddHttpClient<ITwitchService, TwitchService>();

            services.AddTransient<IEmoteTrackerService, EmoteTrackerService>();

            services.AddTransient<IPurpleChannelService, PurpleChannelService>();

            return services;
        }
    }
}
