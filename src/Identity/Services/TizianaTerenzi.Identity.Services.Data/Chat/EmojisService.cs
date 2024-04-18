namespace TizianaTerenzi.Identity.Services.Data.Chat
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Web.Models.Emojis;

    [TransientRegistration]
    public class EmojisService : IEmojisService
    {
        private readonly IDeletableEntityRepository<Emoji> emojisRepository;

        public EmojisService(IDeletableEntityRepository<Emoji> emojisRepository)
        {
            this.emojisRepository = emojisRepository;
        }

        public async Task<IEnumerable<EmojiViewModel>> GetAllEmojisAsync()
        {
            var emojis = await this.emojisRepository
                        .All()
                        .Select(e => new EmojiViewModel
                        {
                            Id = e.Id.ToString(),
                            Image = e.Image,
                        })
                        .ToListAsync();

            return emojis;
        }
    }
}
