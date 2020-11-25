namespace TizianaTerenzi.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Web.ViewModels.Products;

    public interface IProductsService
    {
        Task<ProductsWithPaginationListingViewModel> GetAllProductsAsync(int take, int skip = 0);

        Task<ProductDetailsViewModel> GetProductByIdAsync(int id);

        Task<bool> CreateProductAsync(Product product);

        Task CreateProductsRangeAsync(IEnumerable<Product> products);

        Task<int> GetProductsCountAsync();
    }
}
