namespace TizianaTerenzi.Identity.Services.Data.UserPenalties
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common.Messages.Administration;

    public interface IUserPenaltiesService
    {
        Task<bool> BlockUserAsync(UserBlockedMessage message);

        Task<bool> UnblockUserAsync(UserUnblockedMessage message);

        Task<IEnumerable<SelectListItem>> GetAllBlockedUsersAsync();

        Task<IEnumerable<SelectListItem>> GetAllUnblockedUsersAsync();
    }
}
