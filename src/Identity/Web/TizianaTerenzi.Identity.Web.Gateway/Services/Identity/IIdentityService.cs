namespace TizianaTerenzi.Identity.Web.Gateway.Services.Identity
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Identity.Web.Gateway.Models;

    public interface IIdentityService
    {
        [Get("/Profile/GetUsersPersonalDataForExport")]
        Task<Result<UsersPersonalDataForExportResponseModel>> GetUsersPersonalDataForExport(string password);
    }
}
