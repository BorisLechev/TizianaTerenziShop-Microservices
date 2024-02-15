namespace TizianaTerenzi.Administration.Services.Data.Users
{
    using MassTransit;
    using TizianaTerenzi.Administration.Data.Models;
    using TizianaTerenzi.Administration.Web.Models.Users;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Messages.Identity;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<UserStatistics> userStatisticsRepository;
        private readonly IBus publisher;

        public UsersService(
            IDeletableEntityRepository<UserStatistics> userStatisticsRepository,
            IBus publisher)
        {
            this.userStatisticsRepository = userStatisticsRepository;
            this.publisher = publisher;
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

        public async Task AddUserInRole(UsernamesRolesIndexViewModel model)
        {
            await this.publisher.Publish(new UserInRoleAddedMessage
            {
                UserId = model.UserId,
                RoleId = model.RoleId,
            });
        }
    }
}
