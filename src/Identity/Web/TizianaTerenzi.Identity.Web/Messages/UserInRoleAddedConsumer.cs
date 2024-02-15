namespace TizianaTerenzi.Identity.Web.Messages
{
    using MassTransit;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Identity.Services.Data.Users;

    public class UserInRoleAddedConsumer : IConsumer<UserInRoleAddedMessage>
    {
        private readonly IUsersService usersService;

        public UserInRoleAddedConsumer(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task Consume(ConsumeContext<UserInRoleAddedMessage> context)
        {
            var message = context.Message;

            var updateUserRole = await this.usersService.AddUserInRole(message.UserId, message.RoleId);

            if (!updateUserRole)
            {
                //Result.Failure(NotificationMessages.UserIsAlreadyInThisRole);
            }
            else
            {
                //Result.Success(NotificationMessages.SuccessfullyAddedUserInRole);
            }

            await Task.CompletedTask;
        }
    }
}
