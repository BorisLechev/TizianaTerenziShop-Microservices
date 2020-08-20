namespace MelegPerfumes.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MelegPerfumes.Data.Models;

    public interface ISubscribeService
    {
        Task SubscribeForNewsletterAsync(Subscriber subscriber);

        Task DeleteEmailAsync(int emailId);

        Task<IEnumerable<Subscriber>> GetAllEmails();

        Task<Subscriber> FindByNameAsync(string email);
    }
}
