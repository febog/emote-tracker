using EmoteTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace EmoteTracker.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new EmoteTrackerContext(serviceProvider.GetRequiredService<DbContextOptions<EmoteTrackerContext>>()))
            {
                if (context == null || context.EmoteServices == null || context.TwitchAppAccessTokens == null)
                {
                    throw new ArgumentNullException("Null EmoteTrackerContext");
                }

                if (!context.EmoteServices.Any())
                {
                    context.EmoteServices.AddRange(
                        new EmoteService
                        {
                            Name = "FFZ"
                        },
                        new EmoteService
                        {
                            Name = "BTTV"
                        },
                        new EmoteService
                        {
                            Name = "7TV"
                        });
                }

                if (!context.TwitchAppAccessTokens.Any())
                {
                    context.TwitchAppAccessTokens.AddRange(
                        new TwitchAppAccessToken
                        {
                            AccessToken = "token",
                            ExpiresIn = 0,
                            TokenType = "bearer"
                        });
                }

                context.SaveChanges();
            }
        }
    }
}
