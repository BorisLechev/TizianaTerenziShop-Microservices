namespace TizianaTerenzi.WebClient.Services.Identity
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Identity;
    using TizianaTerenzi.WebClient.ViewModels.Profile;
    using TizianaTerenzi.WebClient.ViewModels.UserPenalties;
    using TizianaTerenzi.WebClient.ViewModels.Users;

    public interface IIdentityService
    {
        [Post("/Identity/Login")]
        Task<UserResponseModel> Login([Body] LoginUserInputModel loginInput);

        [Post("/Identity/Register")]
        Task<Result<UserResponseModel>> Register([Body] RegisterUserInputModel registerInput);

        [Get("/Profile/Index")]
        Task<Result<ProfileViewModel>> Profile(string id);

        [Get("/Profile/ChangePassword")]
        Task<UserChangePasswordInputModel> GetUserEmailWhenChangePassword();

        [Post("/Profile/ChangePassword")]
        Task<Result> ChangePassword([Body] UserChangePasswordInputModel inputModel);

        [Get("/Profile/SetPassword")]
        Task<UserSetPasswordInputModel> GetUserEmailWhenSetPassword();

        [Post("/Profile/SetPassword")]
        Task<Result> SetPassword([Body] UserSetPasswordInputModel inputModel);

        [Get("/Profile/Edit")]
        Task<UserEditInputModel> GetDetailsForUserEdit();

        [Multipart]
        [Put("/Profile/Edit")]
        Task<Result> EditUserDetails([Query] UserEditInputModel inputModel, [AliasAs("avatarPicture")] StreamPart avatarPicture);

        [Get("/Profile/All")]
        Task<AllUsersListViewModel> GetAllUsersExceptAdmins(int page);

        [Get("/Users/AllUsers")]
        Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync();

        [Get("/Users/AllBannedUsers")]
        Task<IEnumerable<BannedApplicationUserViewModel>> GetAllBannedUsersAsync();

        [Get("/Users/Roles")]
        Task<UsernamesRolesIndexViewModel> GetUsernamesRolesAsync();

        [Get("/Penalties/Index")]
        Task<UserPenaltiesInputModel> GetAllBlockedAndUnblockedUsersAsync();

        [Delete("/Profile/DeleteAccount")]
        Task<Result> DeleteAccount(string password);
    }
}
