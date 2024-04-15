namespace TizianaTerenzi.Administration.Services.Data.UserPenalties
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;

    public class UserPenaltiesService : IUserPenaltiesService
    {
        private readonly IDeletableEntityRepository<UserStatistics> userStatisticsRepository;

        public UserPenaltiesService(
            IDeletableEntityRepository<UserStatistics> userStatisticsRepository)
        {
            this.userStatisticsRepository = userStatisticsRepository;
        }

        public async Task BlockUserAsync(string userId, string reasonToBeBlocked)
        {
            var message = new UserBlockedMessage
            {
                UserId = userId,
                ReasonToBeBlocked = reasonToBeBlocked,
            };

            await this.userStatisticsRepository.SaveAndPublishEventMessageAsync(message);
        }

        public async Task UnblockUserAsync(string userId)
        {
            var message = new UserUnblockedMessage
            {
                UserId = userId,
            };

            await this.userStatisticsRepository.SaveAndPublishEventMessageAsync(message);
        }
    }
}
