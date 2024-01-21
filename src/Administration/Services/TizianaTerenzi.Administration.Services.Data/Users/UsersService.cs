namespace TizianaTerenzi.Administration.Services.Data.Users
{
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Identity;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<UserStatistics> userStatisticsRepository;

        public UsersService(IDeletableEntityRepository<UserStatistics> userStatisticsRepository)
        {
            this.userStatisticsRepository = userStatisticsRepository;
        }

        public async Task<bool> AddUserStatisticsAsync(UserAddedInAdminStatisticsMessage model)
        {
            var user = new UserStatistics
            {
                UserId = model.UserId,
                RoleName = model.RoleName,
                IsBlocked = model.IsBlocked,
            };

            await this.userStatisticsRepository.AddAsync(user);
            var result = await this.userStatisticsRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
