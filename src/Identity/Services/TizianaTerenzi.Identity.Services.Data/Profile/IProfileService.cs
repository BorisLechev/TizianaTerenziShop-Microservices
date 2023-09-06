namespace TizianaTerenzi.Identity.Services.Data.Profile
{
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Web.Models.Profile;

    public interface IProfileService
    {
        Task<DownloadPersonalDataViewModel> GetPersonalDataForUserJsonAsync(string userId);

        //Task<bool> DeleteUserAsync(ApplicationUser user);

        Task<UserEditInputModel> GetDetailsForUserEditAsync(string userId);

        Task<bool> EditUserDetailsAsync(ApplicationUser user, UserEditInputModel inputModel);

        Task<AllUsersListViewModel> GetAllUsersExceptAdminsAsync(int page, int take, int skip = 0);
    }
}
