namespace TizianaTerenzi.WebClient.Services.Products
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    public interface IProductsGatewayService
    {
        [Post("/Profile/DownloadPersonalData")]
        Task<Result<DownloadPersonalDataFileResponseModel>> DownloadPersonalData(string password);

        [Delete("/Profile/DeleteAccount")]
        Task<Result> DeleteAccount(string password);
    }
}
