namespace TizianaTerenzi.Administration.Services.Data.Subscribers
{
    using TizianaTerenzi.Administration.Data.Models;

    public interface ISubscribeService
    {
        Task<bool> SubscribeForNewsletterAsync(string email);

        Task<IEnumerable<Subscriber>> GetAllEmailsAsync();

        Task<bool> IsTheEmailAlreadySubscribedAsync(string email);
    }
}
