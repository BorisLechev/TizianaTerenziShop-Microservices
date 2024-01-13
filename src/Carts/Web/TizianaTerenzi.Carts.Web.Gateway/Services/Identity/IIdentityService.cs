namespace TizianaTerenzi.Carts.Web.Gateway.Services.Identity
{
    using Refit;
    using TizianaTerenzi.Carts.Web.Models.Carts;
    using TizianaTerenzi.Common.Services;

    public interface IIdentityService
    {
        [Get("/Profile/GetProfileData")]
        Task<Result<ProfileDataResponseModel>> GetProfileData(string id);
    }
}
