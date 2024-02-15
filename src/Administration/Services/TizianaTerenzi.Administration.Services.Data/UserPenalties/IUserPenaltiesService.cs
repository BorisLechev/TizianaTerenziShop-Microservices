namespace TizianaTerenzi.Administration.Services.Data.UserPenalties
{
    public interface IUserPenaltiesService
    {
        Task BlockUserAsync(string userId, string reasonToBeBlocked);

        Task UnblockUserAsync(string userId);
    }
}
