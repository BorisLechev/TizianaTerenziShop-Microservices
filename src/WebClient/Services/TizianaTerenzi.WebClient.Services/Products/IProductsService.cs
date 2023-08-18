namespace TizianaTerenzi.WebClient.Services.Products
{
    using Refit;
    using TizianaTerenzi.Common.Enumerators;
    using TizianaTerenzi.WebClient.ViewModels.Products;

    public interface IProductsService
    {
        [Get("/Products/All")]
        Task<ProductsListViewModel> All([Query] string search, ProductSorting sorting, int page = 1);
    }
}
