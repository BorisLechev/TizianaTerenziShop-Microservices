namespace TizianaTerenzi.WebClient.Services.Products
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    public interface IIdentityGatewayService
    {
        [Post("/Profile/DownloadPersonalData")]
        Task<Result<DownloadPersonalDataFileResponseModel>> DownloadPersonalData(string password);
    }
}
