namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;

    public class SubscribeService : ISubscribeService
    {
        private readonly IDeletableEntityRepository<Subscriber> subscribersRepository;

        public SubscribeService(IDeletableEntityRepository<Subscriber> subscribersRepository)
        {
            this.subscribersRepository = subscribersRepository;
        }

        public async Task DeleteEmailAsync(int emailId)
        {
            var email = await this.subscribersRepository
                .All()
                .SingleOrDefaultAsync(s => s.Id == emailId);

            this.subscribersRepository.Delete(email);
            await this.subscribersRepository.SaveChangesAsync();
        }

        public async Task<Subscriber> FindByNameAsync(string email)
        {
            var selectedEmail = await this.subscribersRepository
                .All()
                .SingleOrDefaultAsync(s => s.Email == email);

            return selectedEmail;
        }

        public async Task<IEnumerable<Subscriber>> GetAllEmails()
        {
            var subscribers = await this.subscribersRepository
                .All()
                .ToListAsync();

            return subscribers;
        }

        public async Task SubscribeForNewsletterAsync(Subscriber subscriber)
        {
            await this.subscribersRepository.AddAsync(subscriber);
            await this.subscribersRepository.SaveChangesAsync();
        }
    }
}
