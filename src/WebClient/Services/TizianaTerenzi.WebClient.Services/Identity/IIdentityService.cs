namespace TizianaTerenzi.WebClient.Services.Identity
{
    using Refit;
    using TizianaTerenzi.WebClient.ViewModels.Identity;

    public interface IIdentityService
    {
        [Post("/Identity/Login")]
        Task<UserResponseModel> Login([Body] LoginUserInputModel loginInput);

        [Post("/Identity/Register")]
        Task<UserResponseModel> Register([Body] RegisterUserInputModel registerInput);
    }
}
