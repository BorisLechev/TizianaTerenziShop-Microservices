namespace TizianaTerenzi.WebClient.Services.Identity
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Identity;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    public interface IIdentityService
    {
        [Post("/Identity/Login")]
        Task<UserResponseModel> Login([Body] LoginUserInputModel loginInput);

        [Post("/Identity/Register")]
        Task<UserResponseModel> Register([Body] RegisterUserInputModel registerInput);

        [Get("/Profile/Index")]
        Task<ProfileViewModel> Profile(string id);

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

        [Put("/Profile/Edit")]
        Task<Result> EditUserDetails([Body] UserEditInputModel inputModel);

        [Get("/Profile/All")]
        Task<AllUsersListViewModel> GetAllUsersExceptAdmins(int page);
    }
}
