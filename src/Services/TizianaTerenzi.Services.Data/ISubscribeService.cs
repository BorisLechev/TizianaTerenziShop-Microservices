namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;

    public interface ISubscribeService
    {
        Task<bool> SubscribeForNewsletterAsync(Subscriber subscriber);

        Task<bool> DeleteEmailAsync(int emailId);

        Task<IEnumerable<Subscriber>> GetAllEmailsAsync();

        Task<Subscriber> FindByNameAsync(string email);
    }
}
