namespace TizianaTerenzi.Administration.Services.Data.Users
{
    using TizianaTerenzi.Common.Messages.Identity;

    public interface IUsersService
    {
        Task<bool> AddUserStatisticsAsync(UserAddedInAdminStatisticsMessage model);
    }
}
