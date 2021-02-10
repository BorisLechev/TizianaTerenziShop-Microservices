namespace TizianaTerenzi.Services.Data.Subscribe
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

        public async Task<bool> IsTheEmailAlreadySubscribedAsync(string email)
        {
            var isAlreadySubscribed = await this.subscribersRepository
                .AllAsNoTracking()
                .AnyAsync(s => s.Email == email);

            return isAlreadySubscribed;
        }

        public async Task<IEnumerable<Subscriber>> GetAllEmailsAsync()
        {
            var subscribers = await this.subscribersRepository
                .AllAsNoTracking()
                .ToListAsync();

            return subscribers;
        }

        public async Task<bool> SubscribeForNewsletterAsync(string email)
        {
            var subscriber = new Subscriber
            {
                Email = email,
            };

            await this.subscribersRepository.AddAsync(subscriber);
            var result = await this.subscribersRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
