namespace TizianaTerenzi.Identity.Data.Seeding
{
    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Common.Data.Seeding;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Scrapers;

    public class EmojisSeeder : ISeeder<IdentityDbContext>
    {
        public async Task SeedAsync(IdentityDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Emojis.Any())
            {
                return;
            }

            var emojiScraperService = serviceProvider.GetRequiredService<IUnicodeEmojiScraperService>();
            var emojisDto = await emojiScraperService.ScrapeEmojisAsync();

            var emojis = emojisDto.Select(e => new Emoji
            {
                Image = e.Image,
            });

            await dbContext.Emojis.AddRangeAsync(emojis);
        }
    }
}
