namespace TizianaTerenzi.Administration.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Administration.Services.Data.Users;
    using TizianaTerenzi.Common.Messages.Identity;

    public class UserAddedInAdminStatisticsConsumer : IConsumer<UserAddedInAdminStatisticsMessage>
    {
        private readonly IUsersService usersService;

        public UserAddedInAdminStatisticsConsumer(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task Consume(ConsumeContext<UserAddedInAdminStatisticsMessage> context)
        {
            var message = context.Message;

            await this.usersService.AddUserStatisticsAsync(message);

            await Task.CompletedTask;
        }
    }
}
