namespace TizianaTerenzi.Administration.Services.Data.Subscribers
{
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;

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
                                            .All()
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
            var isTheEmailAlreadySubscribed = await this.IsTheEmailAlreadySubscribedAsync(email);

            if (isTheEmailAlreadySubscribed)
            {
                return false;
            }

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
