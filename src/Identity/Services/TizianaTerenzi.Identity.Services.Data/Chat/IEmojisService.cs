namespace TizianaTerenzi.Identity.Services.Data.Chat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Identity.Web.Models.Emojis;

    public interface IEmojisService
    {
        Task<IEnumerable<EmojiViewModel>> GetAllEmojisAsync();
    }
}
