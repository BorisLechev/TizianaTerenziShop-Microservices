namespace TizianaTerenzi.Identity.Services.Scrapers
{
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Identity.Services.Models.Scrapers;

    [TransientRegistration]
    public class UnicodeEmojiScraperService : IUnicodeEmojiScraperService
    {
        private const string BaseUrl = "https://unicode.org/Public/emoji/latest/emoji-test.txt";

        public async Task<IEnumerable<EmojiServiceModel>> ScrapeEmojisAsync()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10),
            };

            var text = await client.GetStringAsync(BaseUrl);

            var emojis = text
                        .Split('\n')
                        .Where(l => l.Contains("; fully-qualified"))
                        .Select(l => l.Split('#')[1].Trim().Split(' ')[0])
                        .Distinct()
                        .Select(e => new EmojiServiceModel
                        {
                            Image = e,
                        })
                        .ToList();

            return emojis;
        }
    }
}
