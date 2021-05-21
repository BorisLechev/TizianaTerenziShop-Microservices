namespace TizianaTerenzi.Services.Data.PersonalData
{
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Account;
    using TizianaTerenzi.Web.ViewModels.Profile;

    public interface IPersonalDataService
    {
        Task<string> GetPersonalDataForUserJsonAsync(string userId);

        Task<bool> DeleteUserAsync(string userId);

        Task<UserEditInputModel> GetDetailsForUserEditAsync(string userId);

        Task EditUserDetailsAsync(ApplicationUser user, UserEditInputModel inputModel);

        Task<ApplicationUser> GetUserByIdAsync(string userId);

        Task<AllUsersListViewModel> GetAllUsersExceptAdminsAsync(int page, int take, int skip = 0);
    }
}
