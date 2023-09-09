namespace TizianaTerenzi.Products.Web.Gateway.Services.Identity
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.Products.Web.Gateway.Models;

    public interface IIdentityService
    {
        [Get("/Profile/GetUsersPersonalDataForExport")]
        Task<Result<UsersPersonalDataForExportResponseModel>> GetUsersPersonalDataForExport(string password);

        [Delete("/Profile/DeleteAccount")]
        Task<Result> DeleteAccount(string password);
    }
}
