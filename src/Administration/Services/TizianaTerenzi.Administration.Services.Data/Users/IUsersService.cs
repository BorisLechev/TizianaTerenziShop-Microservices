namespace TizianaTerenzi.Administration.Services.Data.Users
{
    using TizianaTerenzi.Administration.Web.Models.Users;
    using TizianaTerenzi.Common.Messages.Identity;

    public interface IUsersService
    {
        Task<bool> AddUserStatisticsAsync(UserAddedInAdminStatisticsMessage model);

        Task AddUserInRole(UsernamesRolesIndexViewModel model);
    }
}
