namespace TizianaTerenzi.WebClient.Services.Products
{
    using Refit;
    using TizianaTerenzi.Common.Services;
    using TizianaTerenzi.WebClient.ViewModels.Products;
    using TizianaTerenzi.WebClient.ViewModels.Profile;

    public interface IProductsGatewayService
    {
        [Post("/Profile/DownloadPersonalData")]
        Task<Result<DownloadPersonalDataFileResponseModel>> DownloadPersonalData(string password);

        [Get("/Products/PrepareDropdownsForProductCreation")]
        Task<Result<CreateProductInputModel>> PrepareDropdownsForProductCreation();

        [Get("/Products/PrepareDropdownsForProductEditing")]
        Task<Result<CreateProductInputModel>> PrepareDropdownsForProductEditing(int productId);

        [Get("/Products/PrepareDataForProductEditing")]
        Task<Result<EditProductInputModel>> PrepareDataForProductEditing(int productId);
    }
}
