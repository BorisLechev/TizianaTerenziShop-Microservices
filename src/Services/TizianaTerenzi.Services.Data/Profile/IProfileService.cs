namespace TizianaTerenzi.Services.Data.Profile
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    public interface IProfileService
    {
        Task<string> GetPersonalDataForUserJsonAsync(string userId);

        Task<bool> DeleteUserAsync(ApplicationUser user);

        Task<UserEditInputModel> GetDetailsForUserEditAsync(string userId);

        Task<bool> EditUserDetailsAsync(ApplicationUser user, UserEditInputModel inputModel);

        Task<AllUsersListViewModel> GetAllUsersExceptAdminsAsync(int page, int take, int skip = 0);
    }
}
