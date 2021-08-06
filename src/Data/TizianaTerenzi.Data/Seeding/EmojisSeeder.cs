namespace TizianaTerenzi.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Scrapers;

    public class EmojisSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
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
