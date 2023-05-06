namespace TizianaTerenzi.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.WebClient.ViewModels.Emojis;

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
                        .AllAsNoTracking()
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
