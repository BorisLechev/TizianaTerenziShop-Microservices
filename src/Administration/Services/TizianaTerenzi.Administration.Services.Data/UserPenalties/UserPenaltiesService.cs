namespace TizianaTerenzi.Administration.Services.Data.UserPenalties
{
    using MassTransit;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;

    public class UserPenaltiesService : IUserPenaltiesService
    {
        private readonly IBus publisher;
        private readonly IDeletableEntityRepository<UserStatistics> userStatisticsRepository;

        public UserPenaltiesService(
            IBus publisher,
            IDeletableEntityRepository<UserStatistics> userStatisticsRepository)
        {
            this.publisher = publisher;
            this.userStatisticsRepository = userStatisticsRepository;
        }

        public async Task BlockUserAsync(string userId, string reasonToBeBlocked)
        {
            var messageData = new UserBlockedMessage
            {
                UserId = userId,
                ReasonToBeBlocked = reasonToBeBlocked,
            };

            var message = new EventMessageLog(messageData);

            await this.userStatisticsRepository.CreateEventMessageLog(message);
            await this.userStatisticsRepository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.userStatisticsRepository.MarkEventMessageLogAsPublished(message.Id);
        }

        public async Task UnblockUserAsync(string userId)
        {
            var messageData = new UserUnblockedMessage
            {
                UserId = userId,
            };

            var message = new EventMessageLog(messageData);

            await this.userStatisticsRepository.CreateEventMessageLog(message);
            await this.userStatisticsRepository.SaveChangesAsync();

            await this.publisher.Publish(messageData);

            await this.userStatisticsRepository.MarkEventMessageLogAsPublished(message.Id);
        }
    }
}
