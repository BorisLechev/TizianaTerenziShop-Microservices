namespace TizianaTerenzi.Services.Data.Subscribe
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface ISubscribeService
    {
        Task<bool> SubscribeForNewsletterAsync(string email);

        Task<IEnumerable<Subscriber>> GetAllEmailsAsync();

        Task<bool> IsTheEmailAlreadySubscribedAsync(string email);
    }
}
