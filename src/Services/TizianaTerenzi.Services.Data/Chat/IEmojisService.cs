namespace TizianaTerenzi.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.WebClient.ViewModels.Emojis;

    public interface IEmojisService
    {
        Task<IEnumerable<EmojiViewModel>> GetAllEmojisAsync();
    }
}
