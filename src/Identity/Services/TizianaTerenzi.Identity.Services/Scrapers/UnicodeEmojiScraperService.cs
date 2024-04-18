namespace TizianaTerenzi.Identity.Services.Scrapers
{
    using AngleSharp.Html.Parser;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Identity.Services.Models.Scrapers;

    [TransientRegistration]
    public class UnicodeEmojiScraperService : IUnicodeEmojiScraperService
    {
        private const string BaseUrl = "https://unicode.org/emoji/charts/full-emoji-list.html";

        public async Task<IEnumerable<EmojiServiceModel>> ScrapeEmojisAsync()
        {
            var parser = new HtmlParser();
            var handler = new HttpClientHandler { AllowAutoRedirect = false };
            var client = new HttpClient(handler);

            var html = await client.GetStringAsync(BaseUrl);
            var document = await parser.ParseDocumentAsync(html);

            var emojis = document
                        .QuerySelectorAll(".chars")
                        .Select(x => x.InnerHtml)
                        .ToArray();

            var emojisToAdd = emojis.Select(e => new EmojiServiceModel
            {
                Image = e,
            });

            var listWithEmojis = new List<EmojiServiceModel>();

            listWithEmojis.AddRange(emojisToAdd);

            return listWithEmojis;
        }
    }
}
