namespace TizianaTerenzi.Identity.Services.Scrapers
{
    using TizianaTerenzi.Identity.Services.Models.Scrapers;

    public interface IUnicodeEmojiScraperService
    {
        Task<IEnumerable<EmojiServiceModel>> ScrapeEmojisAsync();
    }
}
