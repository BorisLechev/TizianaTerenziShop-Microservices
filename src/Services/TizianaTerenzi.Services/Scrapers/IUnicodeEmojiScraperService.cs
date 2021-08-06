namespace TizianaTerenzi.Services.Scrapers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Services.Models.Scrapers;

    public interface IUnicodeEmojiScraperService
    {
        Task<IEnumerable<EmojiServiceModel>> ScrapeEmojisAsync();
    }
}
